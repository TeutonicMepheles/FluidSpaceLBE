using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public static PositionManager Instance { get; private set; }
    public enum PivotType
    {
        Vis, // 表示可视化的pivot
        Phy  // 表示物理的pivot
    }
    [SerializeField] private GameObject XROrigin;
    [SerializeField] private GameObject visPivot;
    [SerializeField] private GameObject phyPivot;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }
    
    // 新的 SetPivot 方法
    public void SetPivot(GameObject targetPos, PivotType pivotType)
    {
        GameObject pivot = GetPivotByType(pivotType);

        // 获取target的x和z值
        float targetX = targetPos.transform.position.x;
        float targetZ = targetPos.transform.position.z;
        
        // 获取当前pivot的y值
        float pivotY = pivot.transform.position.y;

        // 设置pivot的position
        pivot.transform.position = new Vector3(targetX, pivotY, targetZ);
    }

    // 新的 ResetPositionFromPivot 方法
    public void ResetPositionFromPivot(PivotType pivotType)
    {
        GameObject pivot = GetPivotByType(pivotType);

        // 获取当前物体的当前位置
        Vector3 currentPosition = XROrigin.transform.position;

        // 获取pivot的x和z值
        float pivotX = pivot.transform.position.x;
        float pivotZ = pivot.transform.position.z;

        // 保持y值不变，设置x和z值为pivot的x和z值
        XROrigin.transform.position = new Vector3(pivotX, currentPosition.y, pivotZ);
    }

    // Helper 方法，根据 PivotType 返回对应的 pivot GameObject
    private GameObject GetPivotByType(PivotType pivotType)
    {
        switch (pivotType)
        {
            case PivotType.Vis:
                return visPivot;
            case PivotType.Phy:
                return phyPivot;
            default:
                Debug.LogError("Invalid PivotType");
                return null;
        }
    }
}