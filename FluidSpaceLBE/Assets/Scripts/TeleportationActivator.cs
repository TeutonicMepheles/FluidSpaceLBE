using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationActivator : MonoBehaviour
{
    public LayerMask teleAreaLayerMask; // TeleArea层的LayerMask
    public XRRayInteractor[] teleportation; // Teleportation GameObject

    void Update()
    {
        // 进行Raycast，包含Trigger Collider
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, teleAreaLayerMask, QueryTriggerInteraction.Collide))
        {
            for (int i = 0; i < teleportation.Length; i++)
            {
                // 如果Raycast命中TeleArea层，则启用Teleportation GameObject
                if (teleportation[i] != null && !teleportation[i].enabled)
                {
                    teleportation[i].enabled = true;
                    Debug.Log("Teleportation GameObject enabled");
                }
            }
        }
        else
        {
            for (int i = 0; i < teleportation.Length; i++)
            {
                // 如果Raycast没有命中TeleArea层，则禁用Teleportation GameObject
                if (teleportation[i] != null && teleportation[i].enabled)
                {
                    teleportation[i].enabled = false;
                    Debug.Log("Teleportation GameObject disabled");
                }
            }
        }
    }
}
