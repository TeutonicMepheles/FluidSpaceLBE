using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Mathematics;

// 只需要抓到控制器激活和退出传送的事件即可，其他可以用TeleportationAnchor本身的事件来激活
public class BoundaryVisualization : MonoBehaviour
{
    [Header("BoundaryObjects")]
    [SerializeField] private BoundaryManager thisBoundary;
    [SerializeField] private GameObject[] selectingVisual;
    [SerializeField] private GameObject[] selectingUI;
    [SerializeField] private GameObject lineEnd;
    
    [Header("UI")]
    [SerializeField] private CanvasGroup tips_Canvas_Group;
    [SerializeField] private RectTransform tips_Bg;
    [SerializeField] private RectTransform tips_Default_Icon;
    [SerializeField] private Vector2 resetSizeDelta;
    [SerializeField] private Vector3 resetRotation;


    private void Start()
    {
        // 用来定义是否处于唤起态
        PlayerInputManager.Instance.StartSelection_EventHandler += PlayerStartSelection;

        // 用来定义是否处于默认态
        PlayerInputManager.Instance.EndSelection_EventHandler += PlayerEndSelection;
    }
    
    private void PlayerStartSelection(object sender, EventArgs e)
    {
        if (PlayerManager.Instance.selfBoundary != thisBoundary)
        {
            foreach (var visualObject in selectingUI)
            {
                visualObject.SetActive(true);
                visualObject.transform.DOScale(1, .5f).SetEase(Ease.OutQuad);
            }
        }
    }
    
    private void PlayerEndSelection(object sender, EventArgs e)
    {
        ResetTipsUI();
        foreach (var visualObject in selectingUI)
        {
            visualObject.transform
                .DOScale(0, .5f).SetEase(Ease.OutElastic)
                .OnComplete(() =>
                {
                    visualObject.SetActive(false); 
                });
        }
    }
    
    public void ShowSelectVisual() // Boundary进入选中态
    {
        foreach (var visualObject in selectingVisual)
        {
            visualObject.SetActive(true); 
            visualObject.transform.DOScale(1, .5f).SetEase(Ease.OutQuad);
        }
    }

    public void HideSelectVisual() // Boundary退出选中态
    {
        foreach (GameObject visualObject in selectingVisual)
        {
            visualObject.transform
                .DOScale(0, .5f).SetEase(Ease.OutElastic)
                .OnComplete(() =>
                {
                    visualObject.SetActive(false); 
                });
        }
    }

    public void ResetTipsUI()
    {
        tips_Bg.DOSizeDelta(resetSizeDelta, .3f).SetEase(Ease.OutElastic);
        tips_Bg.DOLocalRotate(resetRotation, .3f).SetDelay(.2f).SetEase(Ease.OutElastic);
        tips_Default_Icon.GetComponent<Image>().DOFade(1f, .3f).From(0f).SetDelay(.2f).SetEase(Ease.OutElastic);
        lineEnd.transform.DOLocalMoveY(.5f, .3f).SetEase(Ease.OutQuad);
    }
}
