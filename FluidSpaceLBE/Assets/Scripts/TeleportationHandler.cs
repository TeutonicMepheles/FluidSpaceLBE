using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationHandler : MonoBehaviour
{
    public XRRayInteractor xRRayInteractor;
    // 开闭传送功能需要控制的GameObject
    public LayerMask validLayer;
    public LayerMask unvalidLayer;
    public static event Action ReticleInBoundary;
    public static event Action ReticleOutBoundary;
    
    private void OnEnable()
    {
        // 订阅传送功能开启的事件
        TeleportationActivator.TeleportEnalbed += OnTeleportEnabled;
        TeleportationActivator.TeleportDisalbed += OnTeleportDisabled;
    }

    private void OnDisable()
    {
        // 取消订阅传送功能关闭的事件
        TeleportationActivator.TeleportEnalbed -= OnTeleportEnabled;
        TeleportationActivator.TeleportDisalbed -= OnTeleportDisabled;
    }

    void OnTeleportEnabled()
    {
        TargetManager.OnTargetInBoundary += OnTargetInBoundary;
        TargetManager.OnTargetOutBoundary += OnTargetOutBoundary;
        xRRayInteractor.raycastMask = validLayer;
    }
    
    void OnTeleportDisabled()
    {
        TargetManager.OnTargetInBoundary -= OnTargetInBoundary;
        TargetManager.OnTargetOutBoundary -= OnTargetOutBoundary;
        xRRayInteractor.raycastMask = unvalidLayer;
    }

    void OnTargetInBoundary()
    {
        // 指定的传送目标点在Boundary内，Reticle改变为对应形式
        ReticleInBoundary?.Invoke();
    }
    void OnTargetOutBoundary()
    {
        // 指定的传送目标点在Boundary外，Reticle改变为对应形式
        ReticleOutBoundary?.Invoke();
    }
}
