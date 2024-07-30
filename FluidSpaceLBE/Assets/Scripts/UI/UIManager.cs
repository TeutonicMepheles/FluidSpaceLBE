using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private RectTransform bgRect;

    private void Awake()
    {
        bgRect = GetComponent<RectTransform>();
    }

    public void BoundaryUITipsStrenchExpand()
    {
        Debug.Log("1");
        bgRect.DOShapeCircle(bgRect.pivot, -45f, 0.5f, true);
    }
}
