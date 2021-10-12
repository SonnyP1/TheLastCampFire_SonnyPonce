using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComp : MonoBehaviour
{
    CharacterController characterController;
    [Header("Walking")]
    [SerializeField] float WalkingSpeed = 5f;
    [SerializeField] float RotSpeed = 5f;
    [SerializeField] float EdgeCheckTracingDepth = 1f;
    [Header("GroundCheck")]
    [SerializeField] Transform GroundCheck;
    [SerializeField] Transform CheckIfNextGround;
    [SerializeField] float GroundCheckRadius = .1f;
    [SerializeField] LayerMask GroundLayerMask;
    LadderMovement ladderMovementComp;
    Vector2 MoveInput;
    Vector3 Velocity;
    const float GRAVITY = -9.8f;
    Vector3 PreviousWorldPos;
    Vector3 PreviousFloorLocalPos;
    Quaternion PreviousWorldRot;
    Quaternion PreviousFloorLocalRot;
    Transform currentFloor;
    public float GetWalkingSpeed() { return WalkingSpeed; }
    public Vector3 GetVelocity() { return Velocity; }
    public void SetVelocity(Vector3 newVelocity) { Velocity = newVelocity; }
  

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        ladderMovementComp = GetComponent<LadderMovement>();
    }
    private void Update()
    {
        CheckFloor();
        FollowFloor();
        characterController.Move(Velocity * Time.deltaTime);
        UpdateRotation();
        SnapShotPositionAndRotation();
    }

    public Vector2 GetMoveInput() { return MoveInput; }
    public void SetMovementInput(Vector2 inputval)
    {
        MoveInput = inputval;
    }
    public void ClearVerticalVelcoity()
    {
        Velocity.y = 0;
    }
    public void CheckIfOnGroundThenCalcuate()
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
    void CalcuateWalkingVelocity()
    {
        if (IsOnGround())
        {
            Velocity.y = -0.2f;
        }
        Velocity.x = GetPlayerDesiredMoveDir().x * WalkingSpeed;
        Velocity.z = GetPlayerDesiredMoveDir().z * WalkingSpeed;
        Velocity.y += GRAVITY * Time.deltaTime;

    }


    public void UpdateRotation()
    {
        Vector3 PlayerDesiredDir = GetPlayerDesiredMoveDir();
        if (PlayerDesiredDir.magnitude == 0)
        {
            PlayerDesiredDir = transform.forward;
        }
        if (ladderMovementComp.GetIsClimbing() && !IsOnGround())
        {
            PlayerDesiredDir = transform.forward;
        }
        Quaternion DesiredRotation = Quaternion.LookRotation(PlayerDesiredDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, DesiredRotation, Time.deltaTime * RotSpeed);
    }
    public bool IsOnGround()
    {
        return Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
    }
    public Vector3 GetPlayerDesiredMoveDir()
    {
        return new Vector3(-MoveInput.y, 0, MoveInput.x).normalized;
    }
    void FollowFloor()
    {
        if (currentFloor)
        {
            Vector3 DeltaMove = currentFloor.TransformPoint(PreviousFloorLocalPos) - PreviousWorldPos;
            Velocity += DeltaMove / Time.deltaTime;

            Quaternion DestinationRot = currentFloor.rotation * PreviousFloorLocalRot;
            Quaternion DeltaRot = Quaternion.Inverse(PreviousWorldRot) * DestinationRot;
            transform.rotation = transform.rotation * DeltaRot;
        }
    }
    void SnapShotPositionAndRotation()
    {
        PreviousWorldPos = transform.position;
        PreviousWorldRot = transform.rotation;
        if (currentFloor != null)
        {
            PreviousFloorLocalPos = currentFloor.InverseTransformPoint(transform.position);
            PreviousFloorLocalRot = Quaternion.Inverse(currentFloor.rotation) * transform.rotation;
            //to add quaternion you QA * QB   --->>> QA + QB = WORLD ROT
            //to sub quaternion you Inverse(QA) * QB ---->>> QA - QB = LOCAL ROT
        }
    }
    void CheckFloor()
    {
        Collider[] cols = Physics.OverlapSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
        if (cols.Length != 0)
        {
            if (currentFloor != cols[0].transform)
            {
                currentFloor = cols[0].transform;
                SnapShotPositionAndRotation();
            }
        }
    }

    public IEnumerator MoveToTransform(Transform transfromToSnap,float TransitionTime)
    {
        Vector3 startLoc = transform.position;
        Vector3 endLoc = transfromToSnap.position;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = transfromToSnap.rotation;
        float currentTime = 0;
        while (currentTime < TransitionTime)
        {
            currentTime += Time.deltaTime;
            Vector3 deltaOffset = Vector3.Lerp(startLoc, endLoc, currentTime / TransitionTime) - transform.position;
            characterController.Move(deltaOffset);
            transform.rotation = Quaternion.Lerp(startRot, endRot, currentTime / TransitionTime);
            yield return new WaitForEndOfFrame();
        }
    }


}
