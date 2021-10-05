using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerFollow;
    [SerializeField] CinemachineVirtualCamera cineCam1;
    [SerializeField] float playerFollowTransTimeCineCam1;

    private void Start()
    {
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = playerFollowTransTimeCineCam1;;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(playerFollow.Priority > cineCam1.Priority)
        {
            cineCam1.Priority = playerFollow.Priority + 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(playerFollow.Priority < cineCam1.Priority)
        {
            playerFollow.Priority = cineCam1.Priority + 1;
        }
    }
}
