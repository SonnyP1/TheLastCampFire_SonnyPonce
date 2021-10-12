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
        stairsToMove.SetCameraTransition(cineMachineChange);
    }

    public override void Interact()
    {
        base.Interact();
    }
    public override void SwitchOn()
    {
        base.SwitchOn();
        stairsToMove.ToggleOn();
        stairsToMove.CameraMovementSyncWithObjectMoving(playerCamToCineCamSpeed, cineCamToMainCamSpeed);
    }
    public override void SwitchOff()
    {
        base.SwitchOff();
        stairsToMove.ToggleOff();
        stairsToMove.CameraMovementSyncWithObjectMoving(playerCamToCineCamSpeed, cineCamToMainCamSpeed);
    }


}
