using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TransistorManager : MonoBehaviour
{
    [SerializeField] private Image desUI;
    public Sprite phySprite;
    public Sprite visSprite;
    
    [SerializeField] private TransistorAnimController transistorCtrl;
    public UnityEvent visToPhy;
    public UnityEvent phyToVis;

    private void Start()
    {
        transistorCtrl.TransistorLoadEnd += StartTransition;
    }

    private void StartTransition(object sender, EventArgs e)
    {
        if (PlayerManager.Instance.isInPhysical)
        {
            phyToVis?.Invoke();
            TransitionManager.Instance.PhysicalToVisual();
            
        }
        else
        {
            visToPhy?.Invoke();
            TransitionManager.Instance.VisualToPhysical();
        }
    }

    public void SetTransistorSprite(bool isInPhysical)
    {
        if(isInPhysical)
        {
            desUI.sprite = phySprite;
        }
        else
        {
            desUI.sprite = visSprite;
        }
    }
}
