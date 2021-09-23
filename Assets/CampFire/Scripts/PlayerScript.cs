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
    [SerializeField] float EdgeCheckTracingDepth = 1f;
    [SerializeField] Transform CheckIfNextGround;
    [SerializeField] float GroundCheckRadius = .1f;
    [SerializeField] LayerMask GroundLayerMask;
    [SerializeField] float LadderClimbCommitAngleDegrees = 20f;

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

    private void Update()
    {
        if(CurrentClimbingLadder==null)
        {
            CurrentClimbingLadder = FindPlayerClimbingLadder();
        }
        if(CurrentClimbingLadder !=null)
        {
            Debug.Log("I WANT TO CLIMB BOI");
        }
        if (IsOnGround())
        {
            Velocity.y = -0.2f;
        }
        Velocity.x = GetPlayerDesiredMoveDir().x * WalkingSpeed;
        Velocity.z = GetPlayerDesiredMoveDir().z * WalkingSpeed;
        Velocity.y += gravity * Time.deltaTime;
        UpdateRotation();

        RaycastHit hit;
        if (Physics.Raycast(CheckIfNextGround.transform.position, CheckIfNextGround.transform.TransformDirection(Vector3.down), out hit, EdgeCheckTracingDepth, GroundLayerMask))
        {
            characterController.Move(Velocity * Time.deltaTime);
            Debug.DrawRay(CheckIfNextGround.transform.position, CheckIfNextGround.transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
        }
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
        Quaternion DesiredRotation = Quaternion.LookRotation(PlayerDesiredDir,Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation,DesiredRotation,Time.deltaTime * RotSpeed);
    }
}
