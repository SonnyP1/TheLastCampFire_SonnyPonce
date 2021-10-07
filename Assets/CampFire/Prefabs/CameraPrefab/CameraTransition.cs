using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerFollow;
    [SerializeField] CinemachineVirtualCamera cineCam1;
    Vector2 OrginCameraPriority;
    [SerializeField] float startTranstionTime;

    private void Start()
    {
        OrginCameraPriority = new Vector2(playerFollow.Priority,cineCam1.Priority);
        SetTransitionSpeed(startTranstionTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SetTransitionSpeed(startTranstionTime);
            SwitchCameraPriority(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            SwitchCameraPriority(0);
            SetTransitionSpeed(startTranstionTime);
        }
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
