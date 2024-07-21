using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

// 这一脚本的主要作用：
// 1. 跟踪玩家摄像机位置，发送玩家当前位置是否在边界区域内的委托。
// 2. 记录玩家和边界区域的绑定关系，并且实时修改
public class PlayerManager : MonoBehaviour
{
    // 记录玩家当前绑定的Boundary
    [SerializeField] private BoundaryManager selfBoundary;
    public static PlayerManager Instance { get; private set; }
    
    public GameObject pivotObject;
    public LayerMask boundaryLayerMask;
    
    public event EventHandler<PlayerBoundStateEventArgs> PlayerInBoundary_EventHandler;
    public class PlayerBoundStateEventArgs : EventArgs
    {
        public bool isInBoundary;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        TeleportationManager.Instance.SetBoundaryToPlayerEventHandler += SetBoundaryToPlayer;
    }

    private void Update()
    {
        IsPlayerInBoundary(pivotObject,boundaryLayerMask);
    }
    
    private void SetBoundaryToPlayer(object sender, TeleportationManager.BoundarySelectedEventArgs e)
    {
        selfBoundary = e.boundaryManager;
    }
    
    // Raycast判断玩家当前是否能够传送
    public void IsPlayerInBoundary(GameObject pivot, LayerMask layer)
    {
        Ray ray = new Ray(pivot.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
        {
            PlayerInBoundary_EventHandler?.Invoke(this,new PlayerBoundStateEventArgs{isInBoundary = true});
            if (hit.transform.TryGetComponent(out BoundaryManager boundaryManager))
            {
                selfBoundary = boundaryManager;
            }
        }
        else
        {
            PlayerInBoundary_EventHandler?.Invoke(this,new PlayerBoundStateEventArgs{isInBoundary = false});
        }
    }
}
