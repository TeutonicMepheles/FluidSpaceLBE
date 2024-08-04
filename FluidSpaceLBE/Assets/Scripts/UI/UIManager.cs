using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public float fadeTime = 0.5f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    public void PanelFadeIn(Vector3 startPos)
    {
        // 从起始点到Anchor位置浮现
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = startPos;
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, fadeTime);
    }

    public void PanelFadeOut(Vector3 endPos)
    {
        // Anchor位置到终点消失
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = endPos;
        rectTransform.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.InOutQuint);
        canvasGroup.DOFade(0, fadeTime);
    }
}
