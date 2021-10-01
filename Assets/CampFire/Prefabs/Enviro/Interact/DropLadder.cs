using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLadder : Switch
{
    // Start is called before the first frame update
    [SerializeField] Platform LadderToMove;
    LadderScript ladderScript;
    void Start()
    {
        ladderScript = GetComponent<LadderScript>();
        ladderScript.enabled = false;
    }
    public override void Interact()
    {
        base.Interact();
    }
    public override void SwitchOn()
    {
        base.SwitchOn();
        LadderToMove.MoveTo(LadderToMove.EndTrans);
        StartCoroutine(WaitForLadderMovement());
        ladderScript.enabled = true;
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
        



        Destroy(LadderToMove);
    }
}
