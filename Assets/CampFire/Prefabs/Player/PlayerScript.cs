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
    [SerializeField] float MaxTimeToLerp = 1f;
    [SerializeField] float EdgeCheckTracingDepth = 1f;
    [SerializeField] Transform CheckIfNextGround;
    [SerializeField] float GroundCheckRadius = .1f;
    [SerializeField] LayerMask GroundLayerMask;
    [SerializeField] float LadderClimbCommitAngleDegrees = 20f;
    [SerializeField] GameObject Owner;

    IEnumerator GetOnLadderCoroutine;
    CharacterController characterController;
    PlayerInputs playerInputs;
    Vector2 MoveInput;
    Vector3 Velocity;
    const float GRAVITY = -9.8f;
    LadderScript CurrentClimbingLadder;
    List<LadderScript> LaddersNearby = new List<LadderScript>();
    Vector3 PreviousWorldPos;
    Transform currentFloor;
    Vector3 PreviousFloorLocalPos;
    Quaternion PreviousWorldRot;
    Quaternion PreviousFloorLocalRot;

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
            if(AngleDegrees < LadderClimbCommitAngleDegrees && AngleDegrees < ClosestAngle && ladder.enabled == true)
            {
                ChosenLadder = ladder;
                ClosestAngle = AngleDegrees;
            }

        }
        return ChosenLadder;
    }

    void CheckFloor()
    {
        Collider[] cols = Physics.OverlapSphere(GroundCheck.position,GroundCheckRadius,GroundLayerMask);
        if(cols.Length !=0)
        {
            if(currentFloor != cols[0].transform)
            {
                currentFloor = cols[0].transform;
                SnapShotPositionAndRotation();
            }
        }
    }
    void SnapShotPositionAndRotation()
    {
        PreviousWorldPos = transform.position;
        PreviousWorldRot = transform.rotation;
        if(currentFloor != null)
        {
            PreviousFloorLocalPos = currentFloor.InverseTransformPoint(transform.position);
            PreviousFloorLocalRot = Quaternion.Inverse(currentFloor.rotation)*transform.rotation;
            //to add quaternion you QA * QB   --->>> QA + QB = WORLD ROT
            //to sub quaternion you Inverse(QA) * QB ---->>> QA - QB = LOCAL ROT
        }
    }


    void FollowFloor()
    {
        if(currentFloor)
        {
            Vector3 DeltaMove = currentFloor.TransformPoint(PreviousFloorLocalPos) - PreviousWorldPos;
            Velocity += DeltaMove / Time.deltaTime;

            Quaternion DestinationRot = currentFloor.rotation * PreviousFloorLocalRot;
            Quaternion DeltaRot = Quaternion.Inverse(PreviousWorldRot) * DestinationRot;
            transform.rotation = transform.rotation * DeltaRot;
        }
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
        playerInputs.Gameplay.Interact.performed += OnInteractPressed;
    }

    void OnInteractPressed(InputAction.CallbackContext ctx)
    {
        InteractComponent interactComp = GetComponentInChildren<InteractComponent>();
        if(interactComp!=null)
        {
            interactComp.Interact(Owner);
        }
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
            GetOnLadderCoroutine = LerpToTransform(snapToTransform);
            Debug.Log("Start Coroutine!");
            StartCoroutine(GetOnLadderCoroutine);
            CurrentClimbingLadder = ladderToHop;
        }
    }

    IEnumerator LerpToTransform(Transform transfromToSnap)
    {
        playerInputs.Gameplay.Move.Disable();

        Vector3 startLoc = transform.position;
        Vector3 endLoc = transfromToSnap.position;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = transfromToSnap.rotation;
        float currentTime = 0;
        while (currentTime < MaxTimeToLerp)
        {
            currentTime += Time.deltaTime;
            Vector3 deltaOffset = Vector3.Lerp(startLoc, endLoc, currentTime / MaxTimeToLerp) - transform.position;
            characterController.Move(deltaOffset);

            transform.rotation = Quaternion.Lerp(startRot, endRot, currentTime / MaxTimeToLerp);
            yield return new WaitForEndOfFrame();
        }

        playerInputs.Gameplay.Move.Enable();

    }
    private void Update()
    {
        CheckFloor();
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
        FollowFloor();
        characterController.Move(Velocity * Time.deltaTime);
        UpdateRotation();
        SnapShotPositionAndRotation();
    }

    void CalcuateClimbingVelocity()
    {
        Velocity = Vector3.zero;
        if (MoveInput.magnitude == 0)
        {
            Velocity = Vector3.zero;
            return;
        }

        Vector3 LadderDir = CurrentClimbingLadder.transform.forward;
        Vector3 PlayerDisiredMoveDir = GetPlayerDesiredMoveDir();
        float Dot = Vector3.Dot(LadderDir, PlayerDisiredMoveDir);

        if(Dot < 0)
        {
            Velocity.y = WalkingSpeed;
        }
        else
        {
            if(IsOnGround())
            {
                UpdateRotation();
                Velocity = GetPlayerDesiredMoveDir() * WalkingSpeed;
            }
            Velocity.y = -WalkingSpeed;
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
        Velocity.y += GRAVITY * Time.deltaTime;
       
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
