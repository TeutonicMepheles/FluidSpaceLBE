using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }
    public TeleportationProvider teleportationProvider;
    private LocomotionPhase lastLocomotionPhase;
    public bool controllerInTeleSelection = false;

    // EventHandler是一种返回void类型的标准委托
    public event EventHandler StartSelection_EventHandler;
    public event EventHandler EndSelection_EventHandler;
    public event EventHandler TeleportDone_EventHandler;

    // XRIDefaultInputActions这个类需要用Input Action Asset来生成，每次更改后需要更新
    private XRIDefaultInputActions xriDefaultInputActions;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player Input instance");
        }
        Instance = this;
    }

    private void Start()
    {
        xriDefaultInputActions = new XRIDefaultInputActions();
        lastLocomotionPhase = teleportationProvider.locomotionPhase;
        // 处理右手移动相关的事件，传送激活
        xriDefaultInputActions.XRIRightHandLocomotion.Enable();
        xriDefaultInputActions.XRIRightHandLocomotion.TeleportModeActivate.performed += TeleportActivate;
        xriDefaultInputActions.XRIRightHandLocomotion.TeleportModeActivate.canceled += TeleportDisactivate;
    }

    private void Update()
    {
        LocomotionPhaseUpdate();
    }
    
    // 传送激活事件
    private void TeleportActivate(InputAction.CallbackContext obj)
    {
        StartSelection_EventHandler?.Invoke(this,EventArgs.Empty);
        controllerInTeleSelection = true;
    }    
    private void TeleportDisactivate(InputAction.CallbackContext obj)
    {
        EndSelection_EventHandler?.Invoke(this,EventArgs.Empty);
        controllerInTeleSelection = false;
    }

    // 更新传送状态，在传送完成时发送委托
    private void LocomotionPhaseUpdate()
    {
        if (teleportationProvider.locomotionPhase == LocomotionPhase.Done && lastLocomotionPhase != LocomotionPhase.Done)
        {
            TeleportDone_EventHandler?.Invoke(this,EventArgs.Empty);
        }
        lastLocomotionPhase = teleportationProvider.locomotionPhase;
    }

}

