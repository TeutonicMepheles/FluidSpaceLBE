using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    public List<PlayerSO> playerList;
    public float destroyLimit = 5f;
    private float currentTimer = 0f;
    private bool isDestroying = false;

    public void RegisterPlayerToBoundary(PlayerSO player)
    {
        if (!playerList.Contains(player))
        {
            playerList.Add(player);
        }
    }

    public void DeregisterPlayerToBoundary(PlayerSO player)
    {
        if (playerList.Contains(player))
        {
            playerList.Remove(player);
        }
    }

    private void Update()
    {
        UpdateDestroy();
    }

    private void UpdateDestroy()
    {
        if (playerList.Count == 0)
        {
            // 如果列表为空且销毁尚未开始，初始化计时器并标记销毁开始
            if (!isDestroying)
            {
                currentTimer = destroyLimit;
                isDestroying = true;
            }

            // 每帧减少计时器
            currentTimer -= Time.deltaTime;

            // 如果计时器小于等于0，销毁游戏对象
            if (currentTimer <= 0f)
            {
                Debug.Log("Destroy!");
                gameObject.SetActive(false);
            }
        }
        else
        {
            // 如果列表中有新内容，重置标记和计时器
            isDestroying = false;
            currentTimer = 0f;
        }
    }

    
}
