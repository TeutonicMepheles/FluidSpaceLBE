using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReticleStateController : MonoBehaviour
{
    [SerializeField] private GameObject[] outBoundaryVis;
    [SerializeField] private GameObject[] inBoundaryVis;

    private void Start()
    {
        TeleportationManager.Instance.BoundarySelectedEventHandler += BoundarySelected;
    }

    private void BoundarySelected(object sender, TeleportationManager.BoundarySelectedEventArgs e)
    {
        if (e.isSelectedBoundary) // 在选择态中
        {
            ReticleInBoundary();
        }
        else
        {
            ReticleOutBoundary();
        }
    }
    
    private void ReticleInBoundary()
    {
        foreach (var visualObject in inBoundaryVis)
        {
            visualObject.SetActive(true); 
        }
        foreach (var visualObject in outBoundaryVis)
        {
            visualObject.SetActive(false); 
        }
    }
    private void ReticleOutBoundary()
    {
        foreach (var visualObject in inBoundaryVis)
        {
            visualObject.SetActive(false); 
        }
        foreach (var visualObject in outBoundaryVis)
        {
            visualObject.SetActive(true); 
        }
    }
}