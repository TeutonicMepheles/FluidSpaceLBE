using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetManager : MonoBehaviour
{
    // 这个代码用来抓取传送目标点位置，并判断传送的目标点是否在Boundary内。
    public XRRayInteractor rayInteractor;

    public Vector3 target;
    public LayerMask boundaryLayer;
    public static event Action OnTargetInBoundary;
    public static event Action OnTargetOutBoundary;
    
    void Update()
    {
        UpdateTargetPoint();
        InOutBoundaryTrigger(target);
    }

    public void UpdateTargetPoint()
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