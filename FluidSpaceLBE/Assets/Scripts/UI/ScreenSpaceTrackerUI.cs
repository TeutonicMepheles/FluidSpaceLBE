using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceTrackerUI : MonoBehaviour
{
    public static ScreenSpaceTrackerUI Instance { get; private set; }
    
    public event EventHandler<TargetVisibilityEventArgs> TargetVisibility_EventHandler;
    public class TargetVisibilityEventArgs : EventArgs
    {
        public bool visibility;
    }

    public Camera mainCamera;              // 主摄像机
    public Transform target;               // 被指引的目标物体
    public Transform trackerObj;           // 显示方向的3DUI
    public Transform trackerArrow;         // trackerObj中的子物体，用于指示方向
    public LineRenderer eline;             // 用于显示圆c边缘的LineRenderer
    public LineRenderer line1;             // 用于显示v1的LineRenderer
    public LineRenderer line11;            // 用于显示v11的LineRenderer
    public LineRenderer line2;             // 用于显示v2的LineRenderer
    public float offsetDistance = 1.0f;    // 平面距离摄像机的偏移量
    public Vector2 ellipseSize = new Vector2(0.5f, 0.3f); // 椭圆的大小（x和y方向的半径）
    public int ellipseSegments = 100;      // 圆的分段数
    public bool debugMode = true;          // 是否开启调试模式
    private Vector3 focusPoint;            // 圆和椭圆的中心点
    private Vector3 v1;                    // 从平面中心指向目标物体的向量
    private Vector3 v11BoundaryPoint;      // v11与圆c的边缘交点
    private Vector3 v2;                    // 从摄像机位置指向目标物体的向量
    private Vector3 intersectionPoint;     // v2与p平面的交点a
    private bool isVisible;                // 当前物体是否在视野内的状态
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }
    void Start()
    {
        if (eline != null)
        {
            eline.positionCount = ellipseSegments + 1;
        }
    }
    void Update()
    {
        if (target == null || trackerObj == null || trackerArrow == null || mainCamera == null)
            return;

        CalculateVectors();
        UpdateTrackerPosition();

        if (debugMode)
        {
            DebugLineVisual();
        }
    }

    // 计算相关向量
    private void CalculateVectors()
    {
        focusPoint = mainCamera.transform.position + mainCamera.transform.forward * offsetDistance;
        v1 = target.position - focusPoint;
        v11BoundaryPoint = CalculateV11BoundaryPoint();
        v2 = target.position - mainCamera.transform.position;
        intersectionPoint = CalculateIntersectionPoint();
    }

    // 计算v11与圆c边缘的交点
    private Vector3 CalculateV11BoundaryPoint()
    {
        Vector3 v1Projected = Vector3.ProjectOnPlane(v1, mainCamera.transform.forward);
        Vector3 v11Direction = v1Projected.normalized;
        float radiusC = ellipseSize.y;  // 圆c的半径
        return v11Direction * radiusC;
    }

    // 计算v2与p平面的交点a
    private Vector3 CalculateIntersectionPoint()
    {
        Plane plane = new Plane(mainCamera.transform.forward, focusPoint);
        if (plane.Raycast(new Ray(mainCamera.transform.position, v2), out float enter))
        {
            return mainCamera.transform.position + v2.normalized * enter;
        }
        return Vector3.zero;
    }

    // 更新3DUI的位置和方向
    private void UpdateTrackerPosition()
    {
        float radiusC = ellipseSize.y;  // 圆c的半径
        Vector3 aRelative = intersectionPoint - focusPoint;
        float aLength = aRelative.magnitude / radiusC;

        bool wasVisible = isVisible;
        isVisible = aLength <= 1.0f;

        if (isVisible)
        {
            trackerObj.gameObject.SetActive(false);
        }
        else
        {
            trackerObj.gameObject.SetActive(true);
            trackerObj.position = focusPoint + v11BoundaryPoint;
            UpdateTrackerArrowDirection();
        }

        // 当可见性状态改变时广播事件
        if (wasVisible != isVisible)
        {
            TargetVisibility_EventHandler?.Invoke(this,new TargetVisibilityEventArgs{visibility = isVisible});
        }
    }

    // 更新trackerArrow的方向，使其localY轴始终指向target
    private void UpdateTrackerArrowDirection()
    {
        Vector3 directionToTarget = (target.position - trackerArrow.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, trackerObj.up);
        trackerArrow.rotation = targetRotation;

        // 确保trackerArrow的localY轴指向target
        trackerArrow.localRotation = Quaternion.Euler(90, trackerArrow.localRotation.eulerAngles.y, trackerArrow.localRotation.eulerAngles.z);
    }

    // 调试线条显示方法
    private void DebugLineVisual()
    {
        // 使用line1显示v1
        if (line1 != null)
        {
            line1.SetPosition(0, focusPoint);
            line1.SetPosition(1, focusPoint + v1);
        }

        // 使用line11显示v11
        if (line11 != null)
        {
            line11.SetPosition(0, focusPoint);
            line11.SetPosition(1, focusPoint + v11BoundaryPoint);
        }

        // 使用line2显示v2
        if (line2 != null)
        {
            line2.SetPosition(0, mainCamera.transform.position);
            line2.SetPosition(1, target.position);
        }

        // 绘制圆c的边缘
        if (eline != null)
        {
            DrawCircle(focusPoint, mainCamera.transform.right, mainCamera.transform.up, ellipseSize.y);
        }
    }

    // 绘制圆c的边缘
    private void DrawCircle(Vector3 center, Vector3 right, Vector3 up, float radius)
    {
        for (int i = 0; i <= ellipseSegments; i++)
        {
            float angle = 2 * Mathf.PI * i / ellipseSegments;
            Vector3 circlePosition = radius * Mathf.Cos(angle) * right + radius * Mathf.Sin(angle) * up;
            eline.SetPosition(i, center + circlePosition);
        }
    }

    public void UpdateTrackTarget(PhysicalManager physicalManager)
    {
        target = physicalManager.trackerAnchor;
    }

}
