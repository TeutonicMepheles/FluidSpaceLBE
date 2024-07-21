using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryUnitChecker : MonoBehaviour
{
    public Transform player; // 玩家对象
    public BoxCollider boxCollider;
    private bool playerInBoundary;
    private float checkInterval = 1f;
    private float checkTimer;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        playerInBoundary = false;
        checkTimer = checkInterval;
    }

    void Update()
    {
        CheckPlayerInBoundary();

        if (playerInBoundary)
        {
            checkTimer = checkInterval; // Reset timer if player is within boundary
        }
        else
        {
            checkTimer -= Time.deltaTime;
            if (checkTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void CheckPlayerInBoundary()
    {
        Vector3 playerPosition = player.position;
        Vector3 boxCenter = boxCollider.bounds.center;
        Vector3 boxExtents = boxCollider.bounds.extents;

        // 通过射线检测玩家是否在边界范围内
        playerInBoundary = Physics.Raycast(playerPosition, boxCenter - playerPosition, out RaycastHit hit, Mathf.Infinity)
                           && hit.collider == boxCollider;
    }
}


