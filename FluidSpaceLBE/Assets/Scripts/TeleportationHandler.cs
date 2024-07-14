using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationHandler : MonoBehaviour
{
    // 这个代码用来抓取传送目标点位置，并且执行相关的逻辑判断。
    public XRRayInteractor rayInteractor;
    public TeleportationProvider teleportationProvider;

    private bool isInBoundary;

    public bool IsInBoundary
    {
        get { return isInBoundary;}
        set
        {
            if (isInBoundary != value)
            {
                isInBoundary = value;
                OnStateChange?.Invoke(isInBoundary);
            }
        }
    }
    public XRInteractorLineVisual xrline;
    
    private Vector3 selectPt;
    
    // 每次传送到空区域时生成的Boundary
    public GameObject unitBoundary;
    public LayerMask boundaryLayer;
    public static event Action<bool> OnStateChange;
    
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
        
    }
    
    void OnTeleportDisabled()
    {
        
    }

    private void Update()
    {
        RaycastHit res;
        if (rayInteractor.TryGetCurrent3DRaycastHit(out res))
        {
            Vector3 groundPt = res.point; // the coordinate that the ray hits
            Quaternion groundRt = res.transform.rotation;
            selectPt = groundPt;
            IsInBoundary = CheckInBoundary(selectPt);
        }
    }

    bool CheckInBoundary(Vector3 pt)
    {
        Vector3 rayDirection = Vector3.down;
        RaycastHit hit;
        if (Physics.Raycast(pt, rayDirection, out hit, Mathf.Infinity, boundaryLayer))
        {
            return true;
        }
        return false;
    }

    public void GenerateUnitBoundary()
    {
        if (!isInBoundary && teleportationProvider.locomotionPhase == LocomotionPhase.Done)
        {
            //GameObject insUnitBoundary = Instantiate(unitBoundary, selectPt, selectRt);
            isInBoundary = true;
        }
    }
}
