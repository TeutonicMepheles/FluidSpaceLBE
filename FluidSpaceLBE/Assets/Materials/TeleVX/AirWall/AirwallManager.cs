using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirwallManager : MonoBehaviour
{
    [SerializeField] private Material inAirWallMat;
    [SerializeField] private MeshRenderer selfMeshRenderer;
    
    [Header("Shader Reference")]
    [SerializeField] private string playerPosition = "_PlayerPosition";

    private void Awake()
    {
        inAirWallMat = new Material(inAirWallMat);
        selfMeshRenderer.material = inAirWallMat;
    }

    private void Update()
    {
        if (Camera.main != null)
        {
            var head = Camera.main.transform.position;
            inAirWallMat.SetVector(playerPosition, head);
        }
    }
}
