using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private BoundaryManager selectBoundary; //被Ray选中的Boundary
    public static TeleportationManager Instance { get; private set; }
    // 绑定脚本所关注的传送控制器
    public XRRayInteractor xRRayInteractor;
    public event EventHandler<BoundarySelectedEventArgs> BoundarySelected_EventHandler;
    public event EventHandler<BoundarySelectedEventArgs> SetBoundaryToPlayer_EventHandler;
    public class BoundarySelectedEventArgs : EventArgs
    {
        public BoundaryManager boundaryManager;
        public bool isSelectedBoundary;
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
        PlayerInputManager.Instance.TeleportDone_EventHandler += TeleportDone;
        PlayerManager.Instance.PlayerInBoundary_EventHandler += PlayerInBoundary;
    }
    
    private void Update()
    {
        if (PlayerInputManager.Instance.controllerInTeleSelection) UpdateSelection();
    }
    
    private void TeleportDone(object sender, EventArgs e) // 传送完成时，把传送前选中的目标Boundary传送给PlayerManager，并且在之前的Boundary中取消注册用户
    {
        SetBoundaryToPlayer_EventHandler?.Invoke(this,new BoundarySelectedEventArgs{boundaryManager = selectBoundary,isSelectedBoundary = false});
    }
    
    private void PlayerInBoundary(object sender, PlayerManager.PlayerBoundStateEventArgs e) // 接受玩家是否在区域中的广播，修改Ray Interactor的可交互层
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

    private void SetSelectBoundary(BoundaryManager boundary,bool isSelected) // 设置参数boundary为当前的selectBoundary，并且发送选中Boundary的委托
    {
        selectBoundary = boundary;
        BoundarySelected_EventHandler?.Invoke(this,new BoundarySelectedEventArgs{boundaryManager = boundary,isSelectedBoundary = isSelected});
    }

    private void UpdateSelection() // 进入传送点选择模式时，实时更新
    {
        if (xRRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit)) // 做XRRay的Raycast
        {
            if (hit.transform.TryGetComponent(out BoundaryManager boundaryManager)) // 如果Raycast到的点有BoundaryManager这个组件，把对应的boundaryManager设置为selectBoudnary；
            {
                if (boundaryManager != selectBoundary) // 选中新的时，更新
                {
                    SetSelectBoundary(boundaryManager,true);
                }
            }
            else // cast到的点没有对应组件，null
            {
                SetSelectBoundary(null,false);
            }
        }
        else
        {
            SetSelectBoundary(null,false);
        }
    }
}
