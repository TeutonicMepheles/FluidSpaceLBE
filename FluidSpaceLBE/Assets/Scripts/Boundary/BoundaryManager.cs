using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    public List<PlayerSO> playerList;

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
}
