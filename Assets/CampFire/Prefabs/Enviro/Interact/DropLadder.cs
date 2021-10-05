using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLadder : Switch
{
    // Start is called before the first frame update
    [SerializeField] Platform LadderToMove;
    [SerializeField] LadderScript ladderScript;
    void Start()
    {
        ladderScript.enabled = false;
    }
    public override void Interact()
    {
        base.Interact();
    }
    public override void SwitchOn()
    {
        Debug.Log("Switch On!");
        base.SwitchOn();
        if (LadderToMove != null)
        {
            LadderToMove.MoveTo(true);
            StartCoroutine(WaitForLadderMovement());
        }
    }
    public override void SwitchOff()
    {
        return;
    }


    IEnumerator WaitForLadderMovement()
    {
        while (LadderToMove.GetMovingCoroutine() != null)
        {
            yield return new WaitForEndOfFrame();
        }
        ladderScript.enabled = true;
        Destroy(LadderToMove);
    }
}
