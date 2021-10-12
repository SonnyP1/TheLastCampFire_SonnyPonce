using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderMovement : MonoBehaviour
{
    [SerializeField] float MaxTimeToTransition = 1f;
    [SerializeField] float LadderClimbCommitAngleDegrees = 20f;
    MovementComp movementComp;
    List<LadderScript> LaddersNearby = new List<LadderScript>();
    LadderScript CurrentClimbingLadder;
    bool isClimbing;
    Vector3 LadderDir;
    IEnumerator GetOnLadderCoroutine;
    IInputActionCollection InputActions;

    public void SetInput(IInputActionCollection inputAction)
    {
        InputActions = inputAction;
    }
    private void Start()
    {
        movementComp = GetComponent<MovementComp>();
    }
    public bool GetIsClimbing() { return isClimbing; }
    private void Update()
    {
        if (CurrentClimbingLadder == null)
        {
            HopOnLadder(FindPlayerClimbingLadder());
        }
        if (CurrentClimbingLadder)
        {
            SetClimbingInfo(CurrentClimbingLadder.transform.forward, true);
            CalcuateClimbingVelocity();
        }
        else
        {
            movementComp.CheckIfOnGroundThenCalcuate();
        }
    }
    void EnableInputs()
    {
        InputActions.Enable();
    }
    void HopOnLadder(LadderScript ladderToHop)
    {

        if (ladderToHop == null) return;

        if (ladderToHop != CurrentClimbingLadder)
        {
            Transform snapToTransform = ladderToHop.GetClosestSnappingTransform(transform.position);
            InputActions.Disable();
            GetOnLadderCoroutine = movementComp.MoveToTransform(snapToTransform, MaxTimeToTransition);
            StartCoroutine(GetOnLadderCoroutine);
            Invoke("EnableInputs", MaxTimeToTransition);
            CurrentClimbingLadder = ladderToHop;
        }
    }
    public void NotifyLadderNearby(LadderScript ladderNearby)
    {
        LaddersNearby.Add(ladderNearby);
    }
    public void NotifyLadderExit(LadderScript ladderExit)
    {
        if (ladderExit == CurrentClimbingLadder)
        {
            CurrentClimbingLadder = null;
            SetClimbingInfo(Vector3.zero, false);
            movementComp.ClearVerticalVelcoity();
        }
        LaddersNearby.Remove(ladderExit);
    }

    LadderScript FindPlayerClimbingLadder()
    {
        Vector3 PlayerDesiredMoveDir = movementComp.GetPlayerDesiredMoveDir();
        LadderScript ChosenLadder = null;
        float ClosestAngle = 180.0f;


        foreach (LadderScript ladder in LaddersNearby)
        {
            Vector3 LadderDir = ladder.transform.position - transform.position;
            LadderDir.y = 0;
            LadderDir.Normalize();
            float Dot = Vector3.Dot(PlayerDesiredMoveDir, LadderDir);
            float AngleDegrees = Mathf.Acos(Dot) * Mathf.Rad2Deg;
            if (AngleDegrees < LadderClimbCommitAngleDegrees && AngleDegrees < ClosestAngle && ladder.enabled == true)
            {
                ChosenLadder = ladder;
                ClosestAngle = AngleDegrees;
            }

        }
        return ChosenLadder;
    }
    public void CalcuateClimbingVelocity()
    {
        Vector3 Velocity = movementComp.GetVelocity();
        float WalkingSpeed = movementComp.GetWalkingSpeed();
        
        Velocity = Vector3.zero;
        if (movementComp.GetMoveInput().magnitude == 0)
        {
            Velocity = Vector3.zero;
            movementComp.SetVelocity(Velocity);
            return;
        }
        Vector3 PlayerDisiredMoveDir = movementComp.GetPlayerDesiredMoveDir();
        float Dot = Vector3.Dot(LadderDir, PlayerDisiredMoveDir);

        if (Dot < 0)
        {
            Velocity.y = WalkingSpeed;
        }
        else
        {
            if (movementComp.IsOnGround())
            {
                movementComp.UpdateRotation();
                Velocity = movementComp.GetPlayerDesiredMoveDir() * WalkingSpeed;
            }
            Velocity.y = -WalkingSpeed;
        }
        movementComp.SetVelocity(Velocity);
    }
    public void SetClimbingInfo(Vector3 ladderDir, bool climbing)
    {
        LadderDir = ladderDir;
        isClimbing = climbing;
        Debug.Log(isClimbing);
    }
}
