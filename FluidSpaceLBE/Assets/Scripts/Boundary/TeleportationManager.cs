using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 这一脚本的主要作用：
// 1. 接收到手柄进入选择状态的委托时，实时判断选中点是否在边界区域内，并且发送对应的委托
// 2. 记录玩家和边界区域的绑定关系，并且实时修改

public class TeleportationManager : MonoBehaviour
{
    // 被Ray选中的Boundary
    [SerializeField] private BoundaryManager selectBoundary;
    public static TeleportationManager Instance { get; private set; }
    
    private bool isInTeleportSelection = false;
    // 绑定脚本所关注的传送控制器
    public XRRayInteractor xRRayInteractor;
    public event EventHandler<BoundarySelectedEventArgs> BoundarySelectedEventHandler;
    public event EventHandler<BoundarySelectedEventArgs> SetBoundaryToPlayerEventHandler;
    public class BoundarySelectedEventArgs : EventArgs
    {
        public BoundaryManager boundaryManager;
        public bool isSelectInBoundary;
    }


    // 开闭传送功能需要控制的InteractionLayerMask，以及Boundary所在的Layer
    public InteractionLayerMask validLayer;
    public InteractionLayerMask unvalidLayer;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Teleportation Controller instance");
        }
        Instance = this;
    }
    
    private void Start()
    {
        PlayerInputManager.Instance.SelectingMode_EventHandler += TeleportSelection;
        PlayerInputManager.Instance.TeleportDone_EventHandler += TeleportDone;
        PlayerManager.Instance.PlayerInBoundary_EventHandler += PlayerInBoundary;
    }
    
    private void Update()
    {
        if (isInTeleportSelection) UpdateSelection();
    }

    private void PlayerInBoundary(object sender, PlayerManager.PlayerBoundStateEventArgs e)
    {
        if (e.isInBoundary) // 改为可射线交互Boundary层和Anchor层
        {
            xRRayInteractor.interactionLayers = validLayer;
        }
        else // 改为不可射线交互
        {
            xRRayInteractor.interactionLayers = unvalidLayer;
        }
    }
    
    private void TeleportSelection(object sender, PlayerInputManager.SelectingModeEventArgs e)
    {
        // 记录是否在传送中的状态
        isInTeleportSelection = e.isInSelection;
        if (!e.isInSelection)
        {
            // 退出传送选择模式时，清空selectBoundary，并且发送未选择的委托
            selectBoundary = null;
            BoundarySelectedEventHandler?.Invoke(this,new BoundarySelectedEventArgs{boundaryManager = null,isSelectInBoundary = false});
        }
    }
     private void SetSelectBoundary(BoundaryManager boundary)
    {
        // 设置参数boundary为当前的selectBoundary，并且发送选中Boundary的委托
        selectBoundary = boundary;
        BoundarySelectedEventHandler?.Invoke(this,new BoundarySelectedEventArgs{boundaryManager = boundary,isSelectInBoundary = true});
    }
     
    private void TeleportDone(object sender, EventArgs e)
    {
        // 传送完成时，把传送前选中的目标Boundary传送给PlayerManager；
        SetBoundaryToPlayerEventHandler?.Invoke(this,new BoundarySelectedEventArgs{boundaryManager = selectBoundary,isSelectInBoundary = true});
    }

    private void UpdateSelection()
    {
        if (xRRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            // 如果Raycast到的点有BoundaryManager这个组件
            if (hit.transform.TryGetComponent(out BoundaryManager boundaryManager))
            {
                if (boundaryManager != selectBoundary)
                {
                    SetSelectBoundary(boundaryManager);
                }
            }
            else
            {
                SetSelectBoundary(null);
            }
        }
        else
        {
            SetSelectBoundary(null);
        }
    }
}
