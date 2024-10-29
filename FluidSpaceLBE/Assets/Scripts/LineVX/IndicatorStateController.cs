using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorStateController : MonoBehaviour
{
    [SerializeField] private ReticleStateController reticle;
    
    [Header("合法指示器")]
    [SerializeField] private GameObject[] valid;

    [Header("非法指示器")]
    [SerializeField] private GameObject[] invalid;
    
    [Header("指针")]
    [SerializeField] private List<GameObject> cursor;
    
    [SerializeField] private Material validMat, invalidMat;

    private void Update()
    {
        if (!TeleportationManager.Instance.isTargetInBoundary) // 指向了边界外，开始做逐帧判断
        {
            SetupObjectValidation(!reticle.isIntersect);
        }
    }

    public void SetupObjectValidation(bool validState)
    {
        FluidSpaceUtils.ShowListItem(valid,validState);
        FluidSpaceUtils.ShowListItem(invalid,!validState);
        FluidSpaceUtils.SetupListItemMaterial(cursor,validState,validMat,invalidMat);
    }
}
