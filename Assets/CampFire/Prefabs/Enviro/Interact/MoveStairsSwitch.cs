using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class MoveStairsSwitch : Switch
{
    [SerializeField] Platform stairsToMove;
    [SerializeField] CameraTransition cineMachineChange;
    [SerializeField] float playerCamToCineCamSpeed;
    [SerializeField] float cineCamToMainCamSpeed;

    void Start()
    {
        stairsToMove.onMoveStatusChange += MoveStatusChanged;
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
        stairsToMove.MoveTo(true);
    }
    public override void SwitchOff()
    {
        base.SwitchOff();
        stairsToMove.MoveTo(false);
    }
}
