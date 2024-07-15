using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetManager : MonoBehaviour
{
    // 这个代码用来抓取传送目标点位置，并且执行相关的逻辑判断。
    public XRRayInteractor rayInteractor;
    public TeleportationProvider teleportationProvider;
    
    public Vector3 target;
    public LayerMask boundaryLayer;
    public static event Action OnTargetInBoundary;
    public static event Action OnTargetOutBoundary;
    
    void Update()
    {
        GetTargetPoint();
        InOutBoundaryTrigger(target);
    }

    public void GetTargetPoint()
    {
        RaycastHit res;
        if (rayInteractor.TryGetCurrent3DRaycastHit(out res))
        {
            target = res.point;
        }
    }

    public void InOutBoundaryTrigger(Vector3 pt)
    {
        Ray ray = new Ray(pt, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, boundaryLayer))
        {
            OnTargetInBoundary?.Invoke();
        }
        else
        {
            OnTargetOutBoundary?.Invoke();
        }
    }
}
