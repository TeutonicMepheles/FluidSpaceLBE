using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationActivator : MonoBehaviour
{
    // 定义事件
    public static event Action TeleportEnalbed;
    public static event Action TeleportDisalbed;
    
    public LayerMask anchorLayerMask; // Anchor层的LayerMask

    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, anchorLayerMask))
        {
            TeleportEnalbed?.Invoke();
        }
        else
        {
            TeleportEnalbed?.Invoke();
        }
    }
}
