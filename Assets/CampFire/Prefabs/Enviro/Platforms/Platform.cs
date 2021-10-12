using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Toggleable
{
    void ToggleOn();
    void ToggleOff();
}
public class Platform : MonoBehaviour, Toggleable
{
    [SerializeField] Transform objectToMove;
    [SerializeField] float TransitionTime;
    [SerializeField] Transform StartTrans;
    [SerializeField] Transform EndTrans;
    PlatformMovementComp platformMovementComp;
    CameraTransition cineMachineChange;
    public void SetCameraTransition(CameraTransition newCameraTrans) { cineMachineChange = newCameraTrans; }
    private void Start()
    {
        platformMovementComp = GetComponent<PlatformMovementComp>();
        platformMovementComp.SetObjectToMove(objectToMove);
        platformMovementComp.SetStartEndTrans(StartTrans, EndTrans);
    }
    public void ToggleOn()
    {
        Debug.Log("Toggle On");
        platformMovementComp.MoveTo(true);
    }
    public void ToggleOff()
    {
        platformMovementComp.MoveTo(false);
    }

    public void CameraMovementSyncWithObjectMoving(float camSpeed1,float camSpeed2)
    {
        if (cineMachineChange != null)
        {
            StartCoroutine(WaitForPlatformMovement(camSpeed1, camSpeed2));
        }
    }
    public void CameraMovementSyncWithObjectMoving(float camSpeed1, float camSpeed2,MonoBehaviour ClassToReenabled)
    {
        if (cineMachineChange != null)
        {
            StartCoroutine(WaitForPlatformMovement(camSpeed1, camSpeed2,ClassToReenabled));
        }
    }
    IEnumerator WaitForPlatformMovement(float camSpeed1, float camSpeed2,MonoBehaviour ClassToReenabled = null)
    {
        cineMachineChange.SetTransitionSpeed(camSpeed1);
        cineMachineChange.SwitchCameraPriority(1);
        while (platformMovementComp.GetMovingCoroutine() != null)
        {

            yield return new WaitForFixedUpdate();
        }
        cineMachineChange.SwitchCameraPriority(0);
        cineMachineChange.SetTransitionSpeed(camSpeed2);
        if(ClassToReenabled != null)
        {
            Debug.Log(ClassToReenabled.name);
            ClassToReenabled.enabled = true;
        }
    }


}
