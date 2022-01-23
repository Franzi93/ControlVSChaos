using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Dmdrn.UnityDebug;

[RequireComponent(typeof(EventSystem))]
public class InputSystemEventHandler : MonoBehaviour
{
    private EventSystem eventSystem;

    public void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
        
        InputSystem.onLockChanged += SetInputActive;
    }

    public void SetInputActive(bool active)
    {
        eventSystem.enabled = active;  
    }
}
