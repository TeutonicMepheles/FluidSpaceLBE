using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BoundaryTipsUI : MonoBehaviour
{
    [SerializeField] private Transform avatarContainer;
    [SerializeField] private Transform avatarTemplate;
    [SerializeField] private BoundaryManager boundaryManager;
    
    private void Awake()
    {
        avatarTemplate.gameObject.SetActive(false); // 模板不予显示
        TeleportationManager.Instance.BoundarySelected_EventHandler += UpdateBoundaryTipsUI;
    }

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateBoundaryTipsUI(object sender, TeleportationManager.BoundarySelectedEventArgs e)
    {
        if (e.isSelectedBoundary)
        {
            UpdateVisual();
        }
        else
        {
            UpdateVisual();
        }
    }
    
    private void UpdateVisual()
    {
        foreach (Transform child in avatarContainer) // 先刷新一下待显示的列表
        {
            if (child == avatarTemplate) continue;
            Destroy(child.gameObject);
        }
        
        foreach (PlayerSO coPlayer in boundaryManager.playerList)
        {
            Transform avatarTransform = Instantiate(avatarTemplate, avatarContainer);
            avatarTransform.gameObject.SetActive(true); // 显示生成的UI
            avatarTransform.GetComponent<BoundaryTipsSingleUI>().SetCoPlayerSO(coPlayer); // 设置为对应的Sprite资源
        }
    }
}
