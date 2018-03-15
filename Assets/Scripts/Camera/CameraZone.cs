using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera virtualCameraToSet;
    public int priorityOnEnter = 10;
    public int priorityOnExit = 1;

    private void Start()
    {
        virtualCameraToSet.Priority = priorityOnExit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCameraToSet.Priority = priorityOnEnter;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCameraToSet.Priority = priorityOnExit;
        }
    }
}
