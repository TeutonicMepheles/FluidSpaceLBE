using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    
    public enum ShowListItemMode
    {
        Default,    // 当前默认行为，设置物体的活跃状态
        ScaleOut,     // 反转物体的活跃状态
        ScaleIn,    // 禁用物体
        Enable      // 启用物体
    }
    
    public static void ShowListItem(IEnumerable<GameObject> objlist,bool isShow,ShowListItemMode mode = ShowListItemMode.Default)
    {
        foreach (var obj in objlist)
        {
            switch (mode)
            {
                case ShowListItemMode.Default:
                    obj.SetActive(isShow);
                    break;
                
                case ShowListItemMode.ScaleOut:
                    obj.transform
                        .DOScale(0, .5f).SetEase(Ease.OutElastic)
                        .OnComplete(() =>
                        {
                            obj.SetActive(isShow); 
                        });
                    break;
                
                case ShowListItemMode.ScaleIn:
                    obj.SetActive(isShow);
                    obj.transform.DOScale(1, .5f).SetEase(Ease.OutQuad);
                    break;
                
                case ShowListItemMode.Enable:
                    obj.SetActive(true);
                    break;
            }
        }
    }

    public static void SetupListItemMaterial(IEnumerable<GameObject> objlist, bool isValid, Material validMat, Material invalidMat)
    {
        // 选择材质
        Material targetMaterial = isValid ? validMat : invalidMat;
        // 设置objlist中所有物体的材质
        foreach (var obj in objlist)
        {
            if (obj != null)
            {
                MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = targetMaterial;
                }
            }
        }
    }
}
