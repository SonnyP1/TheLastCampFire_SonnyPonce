using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] float WalkingSpeed = 5f;
    [SerializeField] float RotSpeed = 5f;
    [Header("Other")]
    [SerializeField] Transform GroundCheck;
    [SerializeField] float LadderTimeToSnap = 1f;
    [SerializeField] float EdgeCheckTracingDepth = 1f;
    [SerializeField] Transform CheckIfNextGround;
    [SerializeField] float GroundCheckRadius = .1f;
    [SerializeField] LayerMask GroundLayerMask;
    [SerializeField] float LadderClimbCommitAngleDegrees = 20f;

    IEnumerator GetOnLadderCoroutine;
    CharacterController characterController;
    PlayerInputs playerInputs;
    Vector2 MoveInput;
    Vector3 Velocity;
    const float gravity = -9.8f;
    LadderScript CurrentClimbingLadder;
    List<LadderScript> LaddersNearby = new List<LadderScript>();
    public void NotifyLadderNearby(LadderScript ladderNearby)
    {
        LaddersNearby.Add(ladderNearby);
    }
    public void NotifyLadderExit(LadderScript ladderExit)
    {
        if(ladderExit == CurrentClimbingLadder)
        {
            CurrentClimbingLadder = null;
            Velocity.y = 0f;
        }
        LaddersNearby.Remove(ladderExit);
    }

    LadderScript FindPlayerClimbingLadder()
    {
        Vector3 PlayerDesiredMoveDir = GetPlayerDesiredMoveDir();
        LadderScript ChosenLadder = null;
        float ClosestAngle = 180.0f;


        foreach(LadderScript ladder in LaddersNearby)
        {
            Vector3 LadderDir = ladder.transform.position - transform.position;
            LadderDir.y = 0;
            LadderDir.Normalize();
            float Dot = Vector3.Dot(PlayerDesiredMoveDir, LadderDir);
            float AngleDegrees = Mathf.Acos(Dot) * Mathf.Rad2Deg;
            if(AngleDegrees < LadderClimbCommitAngleDegrees && AngleDegrees < ClosestAngle)
            {
                ChosenLadder = ladder;
                ClosestAngle = AngleDegrees;
            }

        }
        return ChosenLadder;
    }
    bool IsOnGround()
    {
        return Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
    }
    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }
    private void OnDisable()
    {
        playerInputs.Disable();
    }
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInputs.Gameplay.Move.performed += OnMoveInputUpdated;
        playerInputs.Gameplay.Move.canceled += OnMoveInputUpdated;
    }
    void OnMoveInputUpdated(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    void HopOnLadder(LadderScript ladderToHop)
    {
        if (ladderToHop == null) return;

        if(ladderToHop != CurrentClimbingLadder)
        {
            Transform snapToTransform = ladderToHop.GetClosestSnappingTransform(transform.position);
            GetOnLadderCoroutine = GetOnLadder(snapToTransform);
            Debug.Log("Start Coroutine!");
            StartCoroutine(GetOnLadderCoroutine);
            CurrentClimbingLadder = ladderToHop;
        }
    }

    IEnumerator GetOnLadder(Transform transfromToSnap)
    {
        Vector3 startLoc = transform.position;
        Vector3 endLoc = transfromToSnap.position;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = transfromToSnap.rotation;
        float currentTime = 0;
        while (currentTime < LadderTimeToSnap)
        {
            currentTime += Time.deltaTime;
            Vector3 deltaOffest = Vector3.Lerp(startLoc, endLoc, currentTime / LadderTimeToSnap) - transform.position;
            characterController.Move(deltaOffest);

            transform.rotation = Quaternion.Lerp(startRot, endRot, currentTime / LadderTimeToSnap);
            yield return new WaitForEndOfFrame();
        }

    }
    private void Update()
    {
        if(CurrentClimbingLadder==null)
        {
            HopOnLadder(FindPlayerClimbingLadder());
        }
        if(CurrentClimbingLadder)
        {
            CalcuateClimbingVelocity();
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(CheckIfNextGround.transform.position, CheckIfNextGround.transform.TransformDirection(Vector3.down), out hit, EdgeCheckTracingDepth, GroundLayerMask))
            {
                CalcuateWalkingVelocity();
                Debug.DrawRay(CheckIfNextGround.transform.position, CheckIfNextGround.transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            }
            else
            {
                Velocity = new Vector3(0, 0, 0);
            }
        }
        characterController.Move(Velocity * Time.deltaTime);
        UpdateRotation();
    }

    void CalcuateClimbingVelocity()
    {
        Velocity = Vector3.zero;
        Velocity.y = MoveInput.y * WalkingSpeed;
        if (IsOnGround())
        {
            UpdateRotation();
            Velocity = GetPlayerDesiredMoveDir() * WalkingSpeed;
        }
    }
    void CalcuateWalkingVelocity()
    {
        if(IsOnGround())
        {
            Velocity.y = -0.2f;
        }
        Velocity.x = GetPlayerDesiredMoveDir().x * WalkingSpeed;
        Velocity.z = GetPlayerDesiredMoveDir().z * WalkingSpeed;
        Velocity.y += gravity * Time.deltaTime;
       
    }
    Vector3 GetPlayerDesiredMoveDir()
    {
        return new Vector3(-MoveInput.y, 0, MoveInput.x).normalized;
    }

    void UpdateRotation()
    {
        Vector3 PlayerDesiredDir = GetPlayerDesiredMoveDir();
        if(PlayerDesiredDir.magnitude ==0)
        {
            PlayerDesiredDir = transform.forward;
        }
        if(CurrentClimbingLadder != null && !IsOnGround())
        {
            PlayerDesiredDir = transform.forward;
        }
        Quaternion DesiredRotation = Quaternion.LookRotation(PlayerDesiredDir,Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation,DesiredRotation,Time.deltaTime * RotSpeed);
    }
}
