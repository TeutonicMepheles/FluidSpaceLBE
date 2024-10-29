using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class VRInputManager : MonoBehaviour
{
    #region Singleton_Definition

    public static VRInputManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player Input instance");
        }
        Instance = this;
    }

    #endregion
    
    // XRIDefaultInputActions这个类需要用Input Action Asset来生成，每次更改后需要更新
    private XRIDefaultInputActions xriDefaultInputActions;

    private void OnEnable()
    {
        EventManager.Instance.AddEvent(EventNames.TestEvent, OnTestEvent);
        EventManager.Instance.AddEvent(EventNames.TestEventWithParam, OnTestEventWithParam);
        EventManager.Instance.AddEvent(EventNames.TestEventWithGameObject, OnTestEventWithGameObject);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveEvent(EventNames.TestEvent, OnTestEvent);
        EventManager.Instance.RemoveEvent(EventNames.TestEventWithParam, OnTestEventWithParam);
        EventManager.Instance.RemoveEvent(EventNames.TestEventWithGameObject, OnTestEventWithGameObject);
    }
    
    
    private void Start()
    {
        xriDefaultInputActions = new XRIDefaultInputActions();
        xriDefaultInputActions.XRIRightHandLocomotion.Enable();
        xriDefaultInputActions.XRILeftHandLocomotion.Enable();
        
    }
}
