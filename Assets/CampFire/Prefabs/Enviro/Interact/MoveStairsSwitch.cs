using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MoveStairsSwitch : Switch
{
    [SerializeField] Platform stairsToMove;
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
            //maybe lock playermovement if I need too
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Stairs stop moving");
    }
}
