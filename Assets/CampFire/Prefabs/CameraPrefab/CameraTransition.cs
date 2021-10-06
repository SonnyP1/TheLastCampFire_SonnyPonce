using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerFollow;
    [SerializeField] CinemachineVirtualCamera cineCam1;
    Vector2 OrginCameraPriority;
    [SerializeField] float playerFollowTransTimeCineCam1;

    private void Start()
    {
        OrginCameraPriority = new Vector2(playerFollow.Priority,cineCam1.Priority);
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = playerFollowTransTimeCineCam1;
    }
    private void OnTriggerEnter(Collider other)
    {
        SwitchCameraPriority(1);
    }

    private void OnTriggerExit(Collider other)
    {
        SwitchCameraPriority(0);
    }

    public void SwitchCameraPriority(int cameraToSwitch)
    {
        switch(cameraToSwitch)
        {
            case 0:
                SetBackToOrginCameraPriority();
                break;
            case 1:
                cineCam1.Priority = playerFollow.Priority + 1;
                break;
            default:
                SetBackToOrginCameraPriority();
                break;
        }
    }
    public void SetTransitionSpeed(float transitionTime)
    {
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = transitionTime;
    }
    private void SetBackToOrginCameraPriority()
    {
        playerFollow.Priority = (int)OrginCameraPriority.x;
        cineCam1.Priority = (int)OrginCameraPriority.y;
    }
}
