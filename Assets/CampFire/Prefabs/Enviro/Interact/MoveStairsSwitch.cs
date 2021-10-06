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
    public override void Interact()
    {
        base.Interact();
    }
    public override void SwitchOn()
    {
        base.SwitchOn();
        stairsToMove.MoveTo(true);
        StartCoroutine(WaitForStairMovement());
    }
    public override void SwitchOff()
    {
        base.SwitchOff();
        stairsToMove.MoveTo(false);
        StartCoroutine(WaitForStairMovement());
    }

    IEnumerator WaitForStairMovement()
    {
        while(stairsToMove.GetMovingCoroutine() != null)
        {
            if (cineMachineChange != null)
            {
                cineMachineChange.SetTransitionSpeed(playerCamToCineCamSpeed);
                cineMachineChange.SwitchCameraPriority(1);
            }
            yield return new WaitForEndOfFrame();
        }
        if (cineMachineChange != null)
        {
            cineMachineChange.SetTransitionSpeed(cineCamToMainCamSpeed);
            cineMachineChange.SwitchCameraPriority(0);
        }
        Debug.Log("Stairs stop moving");
    }
}
