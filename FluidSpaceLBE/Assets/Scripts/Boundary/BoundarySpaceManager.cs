using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine;

public class BoundarySpaceManager : MonoBehaviour
{
    public GameObject boundarySpacePrefab;

    // 建立玩家和边界的对应关系
    private Dictionary<GameObject, GameObject> playerBoundaryPairs = new Dictionary<GameObject, GameObject>();

    public void AssignBoundarySpace(GameObject player, Vector3 teleportPosition)
    {
        
    }
}
