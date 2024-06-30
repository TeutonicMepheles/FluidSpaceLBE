using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(BoundaryGenerator))]
public class TeleAreaGenerator : MonoBehaviour
{
    [Header("传送区材质")] 
    public Material teleAreaMaterial; // 传送区的材质

    [Header("玩家传送")] 
    public TeleportationProvider teleProvider;
    
    private BoundaryGenerator boundaryGenerator;
    private GameObject teleAreaObject;
    
    void Start()
    {
        // 获取 BoundaryGenerator 组件
        boundaryGenerator = GetComponent<BoundaryGenerator>();
        
        // 检查是否分配了顶点
        if (boundaryGenerator.vertex == null || boundaryGenerator.vertex.Length < 3)
        {
            Debug.LogError("BoundaryGenerator 中的顶点数量不足");
            return;
        }
        
        GenerateTeleArea();
    }

    void GenerateTeleArea()
    {
        // 创建一个新的游戏对象用于传送区
        teleAreaObject = new GameObject("TeleArea");
        teleAreaObject.transform.SetParent(transform);
        MeshFilter meshFilter = teleAreaObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = teleAreaObject.AddComponent<MeshRenderer>();

        // 创建一个新的网格
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        // 设置传送区的顶点
        Vector3[] vertices = new Vector3[boundaryGenerator.vertex.Length * 2];
        for (int i = 0; i < boundaryGenerator.vertex.Length; i++)
        {
            vertices[i] = boundaryGenerator.vertex[i].position;
            vertices[i + boundaryGenerator.vertex.Length] = boundaryGenerator.vertex[i].position;
        }

        // 创建双面的三角形索引数组
        int[] triangles = new int[(boundaryGenerator.vertex.Length - 2) * 6];
        for (int i = 0; i < boundaryGenerator.vertex.Length - 2; i++)
        {
            // 正面
            triangles[i * 6] = 0;
            triangles[i * 6 + 1] = i + 1;
            triangles[i * 6 + 2] = i + 2;

            // 背面
            triangles[i * 6 + 3] = boundaryGenerator.vertex.Length;
            triangles[i * 6 + 4] = i + 2 + boundaryGenerator.vertex.Length;
            triangles[i * 6 + 5] = i + 1 + boundaryGenerator.vertex.Length;
        }

        // 将顶点和三角形分配给网格
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // 重新计算法线以确保正确的光照效果
        mesh.RecalculateNormals();

        // 将材质分配给网格渲染器
        meshRenderer.material = teleAreaMaterial;

        // 将游戏对象分配到 TeleArea 层
        teleAreaObject.layer = LayerMask.NameToLayer("TeleArea");

        // 计算多边形的包围盒
        Bounds bounds = new Bounds(vertices[0], Vector3.zero);
        foreach (Vector3 vertex in vertices)
        {
            bounds.Encapsulate(vertex);
        }

        // 为传送锚点生成BoxCollider并设置为isTrigger
        BoxCollider boxCollider = teleAreaObject.AddComponent<BoxCollider>();
        boxCollider.center = bounds.center - teleAreaObject.transform.position;
        boxCollider.size = bounds.size;
        boxCollider.isTrigger = false;
        
        /*// 为传送区生成MeshCollider并设置为isTrigger
        MeshCollider meshCollider = teleAreaObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        meshCollider.isTrigger = false;
        */

        /*// 添加Teleportation Area组件
        TeleportationArea teleportationArea = teleAreaObject.AddComponent<TeleportationArea>();
        teleportationArea.teleportationProvider = teleProvider.GetComponent<TeleportationProvider>();
        teleportationArea.matchOrientation = MatchOrientation.TargetUp;
        teleportationArea.matchDirectionalInput = true;
        teleportationArea.selectMode = InteractableSelectMode.Multiple;
        teleportationArea.interactionLayers = InteractionLayerMask.GetMask("Teleportation");*/
        
        // 添加并配置 Teleportation Anchor
        TeleportationAnchor teleportationAnchor = teleAreaObject.AddComponent<TeleportationAnchor>();
        teleportationAnchor.teleportationProvider = teleProvider.GetComponent<TeleportationProvider>();
        teleportationAnchor.matchOrientation = MatchOrientation.TargetUp;
        teleportationAnchor.matchDirectionalInput = true;
        teleportationAnchor.selectMode = InteractableSelectMode.Multiple;
        teleportationAnchor.interactionLayers = InteractionLayerMask.GetMask("Teleportation");
    }
}
