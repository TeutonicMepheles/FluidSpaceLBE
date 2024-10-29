using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FrameAnimController : MonoBehaviour
{
    [Header("序列帧材质")] 
    [SerializeField] private Material kerframeMat;
    [SerializeField] private float kerframeDuration;
    [SerializeField] private string paraName = "_time";
    
    private void OnEnable()
    {
        kerframeMat.SetFloat(paraName, 0f);
        PlayKeyframeMatAnim();
    }
    
    private void PlayKeyframeMatAnim()
    {
        // 创建一个从0到1的Tween，并循环播放
        DOTween.To(() => kerframeMat.GetFloat(paraName), 
                x => kerframeMat.SetFloat(paraName, x), 
                0.9f, kerframeDuration)
            .SetEase(Ease.Linear) // 线性增长
            .SetLoops(-1, LoopType.Restart); // 无限循环，每次结束时重新从0开始
    }
}
