using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectBoundaryVisual : MonoBehaviour
{
    [SerializeField] private BoundaryManager thisBoundary;
    [SerializeField] private GameObject[] selectingVisual;

    private void Start()
    {
        TeleportationManager.Instance.BoundarySelected_EventHandler += BoundarySelected;
        PlayerInputManager.Instance.TeleportDone_EventHandler += PlayerTeleportDone;
        PlayerInputManager.Instance.EndSelection_EventHandler += PlayerEndSelection;
    }

    private void PlayerEndSelection(object sender, EventArgs e)
    {
        HideSelectVisual();
    }

    private void PlayerTeleportDone(object sender, EventArgs e)
    {
        HideSelectVisual();
    }

    private void BoundarySelected(object sender, TeleportationManager.BoundarySelectedEventArgs e)
    {
        if (e.boundaryManager == thisBoundary)
        {
            ShowSelectVisual();
        }
        else
        {
            HideSelectVisual();
        }
    }

    private void ShowSelectVisual()
    {
        foreach (var visualObject in selectingVisual)
        {
            visualObject.SetActive(true); 
        }
    }

    private void HideSelectVisual()
    {
        foreach (var visualObject in selectingVisual)
        {
            visualObject.SetActive(false); 
        }
    }
}
