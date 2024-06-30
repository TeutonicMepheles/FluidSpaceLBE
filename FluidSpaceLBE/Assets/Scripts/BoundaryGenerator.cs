using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryGenerator : MonoBehaviour
{
    [Header("边界顶点")] public Transform[] vertex; // 用于存储多边形顶点的数组
    [Header("空气墙高度")] public float wallHeight = 3.0f;
    [Header("空气墙材质")] public Material wallMaterial; // 空气墙的材质
    [Header("Gizmo颜色")] public Color gizmoColor = Color.white;
    
    private LineRenderer lineRenderer;
    private GameObject wallObject;
    
    void Start()
    {
        // 获取当前游戏对象的LineRenderer组件
        lineRenderer = GetComponent<LineRenderer>();

        // 检查是否分配了顶点
        if (vertex == null || vertex.Length < 3)
        {
            Debug.LogError("该边界至少包括三个顶点");
            return;
        }
        
        DrawPolygon();
        DrawAirWall();
    }

    void DrawPolygon()
    {
        // 设置LineRenderer的顶点数
        lineRenderer.positionCount = vertex.Length + 1; // +1以闭合多边形

        // 为LineRenderer设置顶点位置
        Vector3[] positions = new Vector3[vertex.Length + 1];
        for (int i = 0; i < vertex.Length; i++)
        {
            positions[i] = vertex[i].position;
        }
        positions[vertex.Length] = vertex[0].position; // 闭合多边形

        lineRenderer.SetPositions(positions);
    }

    void DrawAirWall()
    {
        // 创建一个新的空气墙游戏对象
        wallObject = new GameObject("AirWall");
        wallObject.transform.SetParent(transform);
        MeshFilter meshFilter = wallObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = wallObject.AddComponent<MeshRenderer>();

        // 创建一个新的网格用于空气墙
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        // 设置空气墙的顶点（从地面向上拉伸）
        Vector3[] wallVertices = new Vector3[vertex.Length * 4];
        for (int i = 0; i < vertex.Length; i++)
        {
            wallVertices[i] = vertex[i].position;
            wallVertices[i + vertex.Length] = vertex[i].position + Vector3.up * wallHeight;
        }

        // 为后侧面复制顶点
        for (int i = 0; i < vertex.Length * 2; i++)
        {
            wallVertices[i + vertex.Length * 2] = wallVertices[i];
        }

        // 创建三角形
        int[] triangles = new int[vertex.Length * 12];
        for (int i = 0; i < vertex.Length; i++)
        {
            int nextIndex = (i + 1) % vertex.Length;

            // 前侧面
            triangles[i * 6 + 0] = i;
            triangles[i * 6 + 1] = nextIndex;
            triangles[i * 6 + 2] = i + vertex.Length;

            triangles[i * 6 + 3] = nextIndex;
            triangles[i * 6 + 4] = nextIndex + vertex.Length;
            triangles[i * 6 + 5] = i + vertex.Length;

            // 后侧面
            triangles[vertex.Length * 6 + i * 6 + 0] = i + vertex.Length * 2;
            triangles[vertex.Length * 6 + i * 6 + 1] = i + vertex.Length * 2 + vertex.Length;
            triangles[vertex.Length * 6 + i * 6 + 2] = nextIndex + vertex.Length * 2;

            triangles[vertex.Length * 6 + i * 6 + 3] = nextIndex + vertex.Length * 2;
            triangles[vertex.Length * 6 + i * 6 + 4] = i + vertex.Length * 2 + vertex.Length;
            triangles[vertex.Length * 6 + i * 6 + 5] = nextIndex + vertex.Length * 2 + vertex.Length;
        }

        // 将顶点和三角形分配给网格
        mesh.vertices = wallVertices;
        mesh.triangles = triangles;

        // 重新计算法线以确保正确的光照效果
        mesh.RecalculateNormals();

        // 将材质分配给网格渲染器
        meshRenderer.material = wallMaterial;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        if (vertex == null || vertex.Length < 3)
            return;

        // 绘制边界顶点
        for (int i = 0; i < vertex.Length; i++)
        {
            Gizmos.DrawSphere(vertex[i].position, 0.1f);
            Gizmos.DrawLine(vertex[i].position, vertex[(i + 1) % vertex.Length].position);
        }
    }
}
