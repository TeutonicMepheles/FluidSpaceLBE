using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBetweenPoints : MonoBehaviour
{
    public Transform start;
    public Transform end;

    public LineRenderer lineRenderer;

    void Update()
    {
        // 设置线条的起点和终点
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPosition(1, end.position);
    }
    
}
