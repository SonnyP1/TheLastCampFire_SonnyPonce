using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnMoveStatusChange(bool MovementStarted);

public class Platform : MonoBehaviour
{
    [SerializeField] Transform objectToMove;
    [SerializeField] float TransitionTime;
    Coroutine MovingCoroutine;
    public OnMoveStatusChange onMoveStatusChange;

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
        Vector3 startPos = objectToMove.position;
        Quaternion startRot = objectToMove.rotation;

        if (onMoveStatusChange != null)
            onMoveStatusChange.Invoke(true);
        while(startTime < MaxTime)
        {
            startTime += Time.deltaTime;
            float percentOfStartMax = startTime / MaxTime;
            objectToMove.position = Vector3.Lerp(startPos, Destination.position, percentOfStartMax);
            objectToMove.rotation = Quaternion.Lerp(startRot, Destination.rotation, percentOfStartMax);
            yield return new WaitForEndOfFrame();
        }
        if(onMoveStatusChange != null)
            onMoveStatusChange.Invoke(false);
    }
}
