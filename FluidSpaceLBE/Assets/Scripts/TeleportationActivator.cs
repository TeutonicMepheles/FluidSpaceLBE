using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationActivator : MonoBehaviour
{
    public LayerMask teleAreaLayerMask; // TeleArea层的LayerMask
    public GameObject teleportation; // Teleportation GameObject

    void Update()
    {
        // 进行Raycast，包含Trigger Collider
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, teleAreaLayerMask, QueryTriggerInteraction.Collide))
        {
            // 如果Raycast命中TeleArea层，则启用Teleportation GameObject
            if (teleportation != null && !teleportation.activeSelf)
            {
                teleportation.SetActive(true);
                Debug.Log("Teleportation GameObject enabled");
            }
        }
        else
        {
            // 如果Raycast没有命中TeleArea层，则禁用Teleportation GameObject
            if (teleportation != null && teleportation.activeSelf)
            {
                teleportation.SetActive(false);
                Debug.Log("Teleportation GameObject disabled");
            }
        }
    }
    
}
