using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrouchHandler : MonoBehaviour
{
    public bool isCrouching;
    public UnityEvent startCrouchingEvent;
    public UnityEvent stopCrouchingEvent;
    public void StartCrouching()
    {
        isCrouching = true;
        startCrouchingEvent.Invoke();
    }
    public void StopCrouching()
    {
        isCrouching = false;
        stopCrouchingEvent.Invoke();
    }
}
