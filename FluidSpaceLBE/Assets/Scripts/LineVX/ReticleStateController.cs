using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// 控制与传送中的准星有关的所有逻辑
/// 先判断是否进入传送激活态，再判断是否指向边界；
/// 如果未指向边界，开始监听扳机键，调节准星，刷新缓存碰撞体，并每帧进行动态检测
/// </summary>

public class ReticleStateController : MonoBehaviour
{
    [Header("选中态参数")]
    [SerializeField] private GameObject[] outBoundaryVis;
    [SerializeField] private GameObject[] inBoundaryVis;
    [SerializeField] private GameObject[] validVis;

    [Header("传送准星参数")] 
    [SerializeField] private List<GameObject> indicators;
    [SerializeField] private BoxCollider[] reticleColl;
    [SerializeField] private LayerMask boundaryLayer;

    private GameObject activeIndicator;
    private BoxCollider activeColl;
    private BoxCollider[] cachedColl;
    public int currentIndex  = 0;
    public bool isIntersect = false;
    private void OnEnable()
    {
        TeleportationManager.Instance.BoundarySelected_EventHandler += BoundarySelected;
        PlayerInputManager.Instance.TeleportModify_Action += ScaleModify;
    }

    private void Start()
    {
        activeColl = reticleColl[currentIndex]; // 默认是最开始被激活那个，记得对好列表状态
        activeIndicator = indicators[currentIndex];
    }
    
    private void Update()
    {
        if (!TeleportationManager.Instance.isTargetInBoundary) // 指向了边界外，开始做逐帧判断
        {
            UpdateBoundarySpawnTransform(); // 每帧更新落点位置
            isIntersect = !FluidSpaceUtils.CheckSpawnNoIntersection(cachedColl, activeColl); // 交错检测
            activeIndicator.GetComponent<IndicatorStateController>().SetupObjectValidation(isIntersect);
            FluidSpaceUtils.ShowListItem(validVis,!isIntersect);
        }
    }

    private void ScaleModify(object sender, PlayerInputManager.ModifyCountEventArgs e) // 用来切换被激活的indicator以及对应的coll
    {
        currentIndex = e.count;
        currentIndex = (currentIndex + 1) % indicators.Count;
        UpdateIndicator(indicators,currentIndex);
    }
    
    private void UpdateIndicator(List<GameObject> indicatorList,int index)
    {
        // 先把所有的都禁用掉，然后刷新显示指定的指示器，并且更新判定用的碰撞体
        FluidSpaceUtils.ShowListItem(indicatorList,false);
        // 在指示器列表中，筛选
        indicatorList[index].SetActive(true);
        activeIndicator = indicatorList[index];
        activeColl = reticleColl[index];
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
        FluidSpaceUtils.ShowListItem(validVis,true);
        currentIndex = 0;
    }
    
    private void ReticleOutBoundary() // 开始缓存Trigger
    {
        FluidSpaceUtils.ShowListItem(inBoundaryVis,false);
        FluidSpaceUtils.ShowListItem(outBoundaryVis,true);
        cachedColl = FluidSpaceUtils.GetTriggerInLayer(boundaryLayer);
    }

    private void UpdateBoundarySpawnTransform()
    {
        TeleportationManager.Instance.targetPos = transform.position;
        TeleportationManager.Instance.targetRot = transform.rotation;
    }
}