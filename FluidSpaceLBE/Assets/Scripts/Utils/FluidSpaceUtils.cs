using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FluidSpaceUtils
{
    // ---- 碰撞体检测相关 ----
    
    /// <summary>
    /// 用来计算两个BoxCollider是否相交
    /// </summary>
    public static bool IsIntersecting(BoxCollider coll1, BoxCollider coll2)
    {
        // 计算两个触发器的边界框（AABB）
        Bounds bounds1 = coll1.bounds;
        Bounds bounds2 = coll2.bounds;

        // 检查是否重叠
        return bounds1.Intersects(bounds2);
    }
    
    /// <summary>
    /// 用来取出场景中指定层的所有BoxCollider Trigger
    /// </summary>
    public static BoxCollider[] GetTriggerInLayer(LayerMask layer)
    {
        // 获取场景中所有的BoxCollider
        BoxCollider[] allColliders = Object.FindObjectsOfType<BoxCollider>();
    
        // 过滤出属于指定Layer且isTrigger为true的BoxCollider
        return System.Array.FindAll(allColliders, collider => 
            collider.isTrigger && layer == (layer | (1 << collider.gameObject.layer)));
    }

    public static bool CheckSpawnNoIntersection(BoxCollider[] collList, BoxCollider coll)
    {
        foreach (var icoll in collList)
        {
            if (IsIntersecting(icoll, coll))
            {
                return false;
            }
        }
        return true;
    }
    
    // ---- 列表操作相关 ----
    public static void ShowListItem(IEnumerable<GameObject> objlist,bool isShow)
    {
        foreach (var obj in objlist)
        {
            obj.SetActive(isShow);
        }
    }
}
