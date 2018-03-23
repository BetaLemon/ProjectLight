using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera virtualCameraToSet;
    public Cinemachine.CinemachineFreeLook freeLookCameraToSet;
    public int priorityOnEnter = 10;
    public int priorityOnExit = 1;

    private void Start()
    {
        if (freeLookCameraToSet != null) freeLookCameraToSet.Priority = priorityOnExit;
        else
            virtualCameraToSet.Priority = priorityOnExit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (freeLookCameraToSet != null) freeLookCameraToSet.Priority = priorityOnEnter;
            else
                virtualCameraToSet.Priority = priorityOnEnter;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (freeLookCameraToSet != null) freeLookCameraToSet.Priority = priorityOnExit;
            else
                virtualCameraToSet.Priority = priorityOnExit;
        }
    }
}
