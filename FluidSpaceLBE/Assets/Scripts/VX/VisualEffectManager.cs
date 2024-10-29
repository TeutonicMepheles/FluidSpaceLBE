using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectManager : MonoBehaviour
{
    [Header("传送激活态特效")] public TeleVX_ScannerController teleVx;
    
    public bool isPlayerCanTeleport = false;
    
    void Start()
    {
         PlayerManager.Instance.PlayerInVisBoundary_EventHandler += StartTeleModeVX;
         // 用来定义是否处于唤起态
         PlayerInputManager.Instance.StartSelection_EventHandler += PlayerStartSelection;

         // 用来定义是否处于默认态
         PlayerInputManager.Instance.EndSelection_EventHandler += PlayerEndSelection;
    }

    private void PlayerEndSelection(object sender, EventArgs e)
    {
        teleVx.ScanBack();
    }

    private void PlayerStartSelection(object sender, EventArgs e)
    {
        if (isPlayerCanTeleport)
        {
            if(teleVx.scanDistance <= 0)
            {
                teleVx.Scan();
            }
        }
    }

    private void StartTeleModeVX(object sender, PlayerManager.PlayerBoundStateEventArgs e)
    {
        isPlayerCanTeleport = e.isInBoundary;
    }

}
