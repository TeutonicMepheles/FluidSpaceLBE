using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationActivator : MonoBehaviour
{
    public GameObject pivotObject;
    public static event Action TeleportEnalbed;
    public static event Action TeleportDisalbed;
    
    public LayerMask anchorLayerMask; // Anchor层的LayerMask

    void Update()
    {
        Ray ray = new Ray(pivotObject.transform.localToWorldMatrix.GetPosition(), Vector3.down);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, anchorLayerMask))
        {
            Debug.Log("in anchor");
            TeleportEnalbed?.Invoke();
        }
        else
        {
            Debug.Log("out anchor");
            TeleportEnalbed?.Invoke();
        }
    }
}