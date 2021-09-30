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
    public void MoveTo(Transform Destination)
    {
        if (MovingCoroutine != null)
        {
            StopCoroutine(MovingCoroutine);
            MovingCoroutine = null;
        }
        MovingCoroutine = StartCoroutine(MoveToTrans(Destination, TransitionTime));
    }

    IEnumerator MoveToTrans(Transform Destination,float MaxTime)
    {
        float startTime = 0f;
        while(startTime < MaxTime)
        {
            startTime += Time.deltaTime;
            objectToMove.position = Vector3.Lerp(objectToMove.position,Destination.position,startTime/ MaxTime);
            objectToMove.rotation = Quaternion.Lerp(objectToMove.rotation, Destination.rotation, startTime / MaxTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
