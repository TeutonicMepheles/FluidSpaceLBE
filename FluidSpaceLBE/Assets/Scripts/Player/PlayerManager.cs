using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;

// 这一脚本的主要作用：
// 1. 跟踪玩家摄像机位置，发送玩家当前位置是否在边界区域内的委托。
// 2. 记录玩家和边界区域的绑定关系，并且实时修改
public class PlayerManager : MonoBehaviour
{
    // 玩家状态相关
    public bool isInPhysical;
    
    // 教学UI相关
    public bool isMoveLearned = false;
    public bool isTeleportLearned = false;
    public GameObject outCanvas;
    
    // 记录玩家当前绑定的Boundary
    public BoundaryManager selfBoundary;
    [SerializeField] private PlayerSO selfPlayerSO;
    public static PlayerManager Instance { get; private set; }
    
    public GameObject pivotObject;
    public LayerMask visualBoundaryLayerMask;
    public LayerMask physicalBoundaryLayerMask;
    
    public float wallDist = 0.4f;

    public event EventHandler<PlayerBoundStateEventArgs> PlayerInPhyBoundary_EventHandler;
    public event EventHandler<PlayerBoundStateEventArgs> PlayerInVisBoundary_EventHandler;
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
        TeleportationManager.Instance.SetBoundaryToPlayer_EventHandler += SetBoundaryToPlayer;
        isInPhysical = true; // 初始在实世界出生
    }
        
    private void Update()
    {
        if (isInPhysical)
        {
            IsPlayerInPhyBoundary(pivotObject,physicalBoundaryLayerMask);
        }

        if (!isInPhysical)
        {
            IsPlayerInVisBoundary(pivotObject,visualBoundaryLayerMask);
            DetectAirWall(wallDist);
        }
    }
        
    // 把玩家从传送前所在的Boundary取消注册，切换新的Boundary之后，再注册到新的Boundary中
    private void SetBoundaryToPlayer(object sender, TeleportationManager.BoundarySelectedEventArgs e)
    { 
        selfBoundary.GetComponent<BoundaryManager>().DeregisterPlayerToBoundary(selfPlayerSO);
        selfBoundary = e.boundaryManager; 
        selfBoundary.GetComponent<BoundaryManager>().RegisterPlayerToBoundary(selfPlayerSO);
    }
    
         
    private void DetectAirWall(float dist)
    {
        // 获取主相机视角的方向并投影到XZ平面
        Vector3 viewDirXZ = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        Ray ray = new Ray(Camera.main.transform.position, viewDirXZ);
        RaycastHit hit;
    }

    void IsPlayerInVisBoundary(GameObject pivot, LayerMask layer) // Raycast判断玩家当前是否在VisBoundary内
    {
        Ray ray = new Ray(pivot.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer)) // Raycast到了Boundary层
        {
            PlayerInVisBoundary_EventHandler?.Invoke(this,new PlayerBoundStateEventArgs{isInBoundary = true});
            if (hit.transform.TryGetComponent(out BoundaryManager boundaryManager)) // 拿到所在的boundaryManager组件，来进行内部人员的管理
            {
                selfBoundary = boundaryManager;
                boundaryManager.RegisterPlayerToBoundary(selfPlayerSO); // 在Boundary内，注册玩家的SO
                CalibrationCheck(boundaryManager);
            }
        }
        else
        {
            if (selfBoundary != null) // 从哪里出来就从哪里取消登记
            {
                selfBoundary.DeregisterPlayerToBoundary(selfPlayerSO);
                selfBoundary = null;
            }
            PlayerInVisBoundary_EventHandler?.Invoke(this,new PlayerBoundStateEventArgs{isInBoundary = false});
        }
    }

    void IsPlayerInPhyBoundary(GameObject pivot, LayerMask layer) // Raycast判断玩家当前是否在PhyBoundary内
    {
        Ray ray = new Ray(pivot.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer)) // Raycast到了Physical层
        {
            PlayerInPhyBoundary_EventHandler?.Invoke(this,new PlayerBoundStateEventArgs{isInBoundary = true});
        }
        else
        {
            PlayerInPhyBoundary_EventHandler?.Invoke(this,new PlayerBoundStateEventArgs{isInBoundary = false});
        }
    }
    
    private void CalibrationCheck(BoundaryManager boundaryManager)
    {
        if(boundaryManager.isCalibrated) return;
        foreach (PlayerSO player in boundaryManager.playerList)
        {
            if (player != selfPlayerSO)
            {
                boundaryManager.BoundaryCalibrated();
                ScreenSpaceTrackerUI.Instance.UpdateTrackTarget(boundaryManager.physicalSpace.GetComponent<PhysicalManager>());
                TransitionManager.Instance.UpdateTeleStateFilter(true);
                outCanvas.SetActive(true);
            }
        }
    }

    public void HideCanvas()
    {
        outCanvas.SetActive(false);
    }

}
