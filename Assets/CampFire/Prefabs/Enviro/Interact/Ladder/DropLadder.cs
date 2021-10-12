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
        LadderToMove.SetCameraTransition(cineMachineChange);
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
            LadderToMove.ToggleOn();
            LadderToMove.CameraMovementSyncWithObjectMoving(playerCamToCineCamSpeed, cineCamToMainCamSpeed, ladderScript);

        }
    }
    public override void SwitchOff()
    {
        return;
    }

}
