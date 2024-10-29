using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 专门管放大效果的
public class TargetUIAnim : MonoBehaviour
{
    public Transform targetUI;   // 要放大的UI元素
    private Tween scaleTween;    // 用于控制缩放动画的Tween
    
    void Start()
    {
        ScreenSpaceTrackerUI.Instance.TargetVisibility_EventHandler += TargetVisibility;
    }

    private void TargetVisibility(object sender, ScreenSpaceTrackerUI.TargetVisibilityEventArgs e)
    {
        if (e.visibility)
        {
            // 如果物体从视野外进入视野内，触发UI放大动画
            PlayScaleAnimation();
        }
        else
        {
            // 如果物体从视野内进入视野外，中断动画并复原UI大小
            ResetScaleAnimation();
        }
    }

    // 播放缩放动画
    private void PlayScaleAnimation()
    {
        // 如果有正在进行的动画，先中断
        if (scaleTween != null && scaleTween.IsActive())
        {
            scaleTween.Kill();
        }

        // 播放缩放动画
        scaleTween = targetUI.DOScale(3f, 0.2f)  // 0.2秒内放大1.5倍
            .OnComplete(() => targetUI.DOScale(1f, 0.2f))  // 然后0.2秒内恢复原大小
            .SetEase(Ease.OutElastic);  // 使用弹性缓动效果
    }

    // 复原UI大小
    private void ResetScaleAnimation()
    {
        if (scaleTween != null && scaleTween.IsActive())
        {
            scaleTween.Kill();
        }

        // 立即复原UI的缩放
        targetUI.localScale = Vector3.one;
    }
}
