using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerInputManager : MonoBehaviour
{
    public MultiSpriteUI locomotionState;
    public int modifyCount = 0;
    public static PlayerInputManager Instance { get; private set; }
    public TeleportationProvider teleportationProvider;
    private LocomotionPhase lastLocomotionPhase;
    public bool controllerInTeleSelection = false;

    public event EventHandler StartMove, EndMove;
    // EventHandler是一种返回void类型的标准委托
    public event EventHandler StartSelection_EventHandler;
    
    public class ModifyCountEventArgs : EventArgs
    {
        public int count;
    }
    public event EventHandler EndSelection_EventHandler;
    public event EventHandler TeleportDone_EventHandler;
    public event EventHandler TransitionStarted_EventHandler;
    public event EventHandler SecondaryPressed_EventHandler;

    public event EventHandler<ModifyCountEventArgs> TeleportModify_Action;

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
        xriDefaultInputActions.XRILeftHandLocomotion.Enable();
        xriDefaultInputActions.XRIRightHandLocomotion.TeleportModeActivate.performed += TeleportActivate;
        xriDefaultInputActions.XRIRightHandLocomotion.TeleportModeActivate.canceled += TeleportDisactivate;
        xriDefaultInputActions.XRIRightHandLocomotion.TeleportModifyActivate.performed += TeleportModifyAction;
        xriDefaultInputActions.XRIRightHandLocomotion.TransitionActivate.performed += TransitionActivate;
        xriDefaultInputActions.XRIRightHandLocomotion.SecondaryActivate.performed += SecondaryActivate;
        xriDefaultInputActions.XRILeftHandLocomotion.Move.performed += StartContinuesMove;
        xriDefaultInputActions.XRILeftHandLocomotion.Move.canceled += EndContinuesMove;
    }

    private void SecondaryActivate(InputAction.CallbackContext obj)
    {
        SecondaryPressed_EventHandler?.Invoke(this,EventArgs.Empty);
    }

    private void EndContinuesMove(InputAction.CallbackContext obj)
    {
        EndMove?.Invoke(this,EventArgs.Empty);
        locomotionState.gameObject.SetActive(false);
    }

    private void StartContinuesMove(InputAction.CallbackContext obj)
    {
        StartMove?.Invoke(this,EventArgs.Empty);
        locomotionState.gameObject.SetActive(true);
        locomotionState.SetupSprite(true);
    }

    private void TransitionActivate(InputAction.CallbackContext obj)
    {
        TransitionStarted_EventHandler?.Invoke(this,EventArgs.Empty);
    }

    private void TeleportModifyAction(InputAction.CallbackContext obj)
    {
        if (controllerInTeleSelection)
        {
            modifyCount++;
        }
        else
        {
            modifyCount = 0;
        }
        TeleportModify_Action?.Invoke(this,new ModifyCountEventArgs {count = modifyCount});
    }

    private void Update()
    {
        LocomotionPhaseUpdate();
    }
    
    // 传送激活事件
    private void TeleportActivate(InputAction.CallbackContext obj)
    {
        StartSelection_EventHandler?.Invoke(this,EventArgs.Empty);
        PlayerManager.Instance.isTeleportLearned = true;
        controllerInTeleSelection = true;
        locomotionState.gameObject.SetActive(true);
        locomotionState.SetupSprite(false);
    }    
    private void TeleportDisactivate(InputAction.CallbackContext obj)
    {
        EndSelection_EventHandler?.Invoke(this,EventArgs.Empty);
        controllerInTeleSelection = false;
        locomotionState.gameObject.SetActive(false);
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

