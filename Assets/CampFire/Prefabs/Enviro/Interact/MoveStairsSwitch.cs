using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStairsSwitch : Switch
{
    [SerializeField] Platform stairsToMove;
    GameObject playerOwner;
    public override void Interact()
    {
        playerOwner = GetOwnerGameObject().transform.parent.gameObject;
        base.Interact();
    }
    public override void SwitchOn()
    {
        base.SwitchOn();
        stairsToMove.MoveTo(stairsToMove.EndTrans);
        StartCoroutine(WaitForStairMovement());
    }
    public override void SwitchOff()
    {
        base.SwitchOff();
        stairsToMove.MoveTo(stairsToMove.StartTrans);
        StartCoroutine(WaitForStairMovement());
    }

    IEnumerator WaitForStairMovement()
    {
        while(stairsToMove.GetMovingCoroutine() != null)
        {
            //maybe lock playermovement if I need too
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("I GET HERE");
    }
}
