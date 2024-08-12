using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReticleStateController : MonoBehaviour
{
    [Header("选中态参数")]
    [SerializeField] private GameObject[] outBoundaryVis;
    [SerializeField] private GameObject[] inBoundaryVis;
    [SerializeField] private GameObject[] invalidBoundaryVis;
    
    [Header("传送准星生成")] 
    [SerializeField] private List<GameObject> indicators;
    [SerializeField] private BoxCollider[] reticleColl;
    [SerializeField] private LayerMask boundaryLayer;
    
    private BoxCollider activeColl;
    private BoxCollider[] cachedColl;
    public int currentIndex  = 0;
    
    private void Start()
    {
        TeleportationManager.Instance.BoundarySelected_EventHandler += BoundarySelected;
        PlayerInputManager.Instance.TeleportModify_Action += ScaleModify;
        activeColl = reticleColl[currentIndex];
    }
    
    private void Update()
    {
        SetBoundarySpawnTransform();
    }

    private void ScaleModify(object sender, PlayerInputManager.ModifyCountEventArgs e)
    {
        currentIndex = e.count;
        currentIndex = (currentIndex + 1) % indicators.Count;
        UpdateIndicator();
    }

    private void SetBoundarySpawnTransform() // 如果进入了Reticle在边界外的情况，则每帧更新Reticle所在的位置，设置为TempBoundary生成的位置
    {
        if (!TeleportationManager.Instance.isTargetInBoundary) 
        {
            TeleportationManager.Instance.targetPos = transform.position;
            TeleportationManager.Instance.targetRot = transform.rotation;
            if (!FluidSpaceUtils.CheckSpawnNoIntersection(cachedColl, activeColl))
            {
                Debug.Log("有交叉！！！");
                FluidSpaceUtils.ShowListItem(invalidBoundaryVis,true);
            }
            else
            {
                FluidSpaceUtils.ShowListItem(invalidBoundaryVis,false);
            }
        }
        else
        {
            FluidSpaceUtils.ShowListItem(invalidBoundaryVis,false);
        }
    }
    
    private void UpdateIndicator()
    {
        foreach (GameObject indicator in indicators)
        {
            indicator.SetActive(false);
        }
        FluidSpaceUtils.ShowListItem(indicators,false);
        // 显示当前索引对应的子物体
        indicators[currentIndex].SetActive(true);
        activeColl = reticleColl[currentIndex];
    }

    private void BoundarySelected(object sender, TeleportationManager.BoundarySelectedEventArgs e)
    {
        if (e.isSelectedBoundary) // 在选择态中，取消Reticle相关的检测，重置Reticle
        {
            ReticleInBoundary();
            
        }
        else // 在选择态中，开启Reticle相关的检测，可调节Reticle
        {
            ReticleOutBoundary();
        }
    }
    
    private void ReticleInBoundary()
    {
        FluidSpaceUtils.ShowListItem(inBoundaryVis,true);
        FluidSpaceUtils.ShowListItem(outBoundaryVis,false);
        currentIndex = 0;
    }
    
    private void ReticleOutBoundary()
    {
        FluidSpaceUtils.ShowListItem(inBoundaryVis,false);
        FluidSpaceUtils.ShowListItem(outBoundaryVis,true);
        cachedColl = FluidSpaceUtils.GetTriggerInLayer(boundaryLayer);
    }
}