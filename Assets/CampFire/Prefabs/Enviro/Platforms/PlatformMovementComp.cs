using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void OnMoveStatusChange(bool MovementStarted);
public class PlatformMovementComp : MonoBehaviour
{
    [SerializeField] float TransitionTime;
    Coroutine MovingCoroutine;
    Transform objectToMove;
    Transform StartTrans;
    Transform EndTrans;

    public Coroutine GetMovingCoroutine() { return MovingCoroutine; }
    public void SetStartEndTrans(Transform startTrans,Transform endTrans)
    {
        StartTrans = startTrans;
        EndTrans = endTrans;
    }
    public void SetObjectToMove(Transform newObjectToMove)
    {
        objectToMove = newObjectToMove;
    }
    IEnumerator MoveToTrans(Transform Destination, float MaxTime)
    {
        float startTime = 0f;
        Vector3 startPos = objectToMove.position;
        Quaternion startRot = objectToMove.rotation;

        while (startTime < MaxTime)
        {
            startTime += Time.deltaTime;
            float percentOfStartMax = startTime / MaxTime;
            objectToMove.position = Vector3.Lerp(startPos, Destination.position, percentOfStartMax);
            objectToMove.rotation = Quaternion.Lerp(startRot, Destination.rotation, percentOfStartMax);
            yield return new WaitForEndOfFrame();
        }
        MovingCoroutine = null;
    }
    public void MoveTo(bool ToEnd)
    {
        if (ToEnd)
        {
            MoveTo(EndTrans);
        }
        else
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
        MovingCoroutine = StartCoroutine(MoveToTrans(Destination, TransitionTime));
    }
}
