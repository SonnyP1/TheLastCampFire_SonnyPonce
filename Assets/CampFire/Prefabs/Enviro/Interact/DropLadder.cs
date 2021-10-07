using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLadder : Switch
{
    // Start is called before the first frame update
    [SerializeField] Platform LadderToMove;
    [SerializeField] LadderScript ladderScript;
    [SerializeField] CameraTransition cineMachineChange;
    [SerializeField] float playerCamToCineCamSpeed;
    [SerializeField] float cineCamToMainCamSpeed;
    void Start()
    {
        ladderScript.enabled = false;
        LadderToMove.onMoveStatusChange += MoveStatusChanged;
    }

    void MoveStatusChanged(bool startedMovement)
    {
        if (startedMovement)
        {
            cineMachineChange.SetTransitionSpeed(playerCamToCineCamSpeed);
            cineMachineChange.SwitchCameraPriority(1);
        }
        else
        {
            ladderScript.enabled = true;
            cineMachineChange.SetTransitionSpeed(cineCamToMainCamSpeed);
            cineMachineChange.SwitchCameraPriority(0);
        }
    }
    public override void Interact()
    {
        base.Interact();
    }
    public override void SwitchOn()
    {
        base.SwitchOn();
        if (LadderToMove != null)
        {
            LadderToMove.MoveTo(true);
        }
    }
    public override void SwitchOff()
    {
        return;
    }

}
