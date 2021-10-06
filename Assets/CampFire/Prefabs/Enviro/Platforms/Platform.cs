using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] Transform objectToMove;
    [SerializeField] float TransitionTime;
    Coroutine MovingCoroutine;

    public Transform StartTrans;
    public Transform EndTrans;

    public void MoveTo(bool ToEnd)
    {
        if(ToEnd)
        {
            MoveTo(EndTrans);
        }else
        {
            MoveTo(StartTrans);
        }
    }
    public void MoveTo(Transform Destination)
    {
        if (MovingCoroutine != null)
        {
            StopCoroutine(MovingCoroutine);
            MovingCoroutine = null;
        }
        Debug.Log("Move to is called");
        MovingCoroutine = StartCoroutine(MoveToTrans(Destination, TransitionTime));
    }

    IEnumerator MoveToTrans(Transform Destination,float MaxTime)
    {
        float startTime = 0f;

        while(startTime < MaxTime)
        {
            startTime += Time.deltaTime;
            float percentOfStartMax = startTime / MaxTime;
            objectToMove.position = Vector3.Lerp(objectToMove.position,Destination.position, percentOfStartMax);
            objectToMove.rotation = Quaternion.Lerp(objectToMove.rotation, Destination.rotation, percentOfStartMax);
            yield return new WaitForEndOfFrame();
        }
        MovingCoroutine = null;
    }


    public Coroutine GetMovingCoroutine() { return MovingCoroutine; }
}
