using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryUnitManager : MonoBehaviour
{
   public GameObject playerTransform;
   public GameObject unitBoundaryPrefab;
   public LayerMask boundaryLayer;
   private void OnEnable()
   {
      TeleportationObserver.OnTeleportationDone += HandleTeleportationDone;
   }
   
   private void OnDisable()
   {
      TeleportationObserver.OnTeleportationDone -= HandleTeleportationDone;
   }

   void HandleTeleportationDone()
   {
      Vector3 ypos = new Vector3(playerTransform.transform.position.x, 0f, playerTransform.transform.position.z);
      if (!HaveBoundary(playerTransform))
      {
         Instantiate(unitBoundaryPrefab, ypos, Quaternion.identity);
      }
   }

   private bool HaveBoundary(GameObject obj)
   {
      RaycastHit hit;
      if (Physics.Raycast(playerTransform.transform.position, Vector3.down, out hit, Mathf.Infinity, boundaryLayer))
      {
         return true;
      }

      return false;
   }
}
