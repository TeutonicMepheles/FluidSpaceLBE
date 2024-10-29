using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using DG.Tweening;

public class TransistorAnimController : MonoBehaviour
{
    [SerializeField] private string paraName = "_time";
    
    [Header("序列帧材质")] 
    [SerializeField] private Material kerframeMat;
    [SerializeField] private float kerframeDuration;
    
    [Header("倒计时材质")] 
    [SerializeField] private Material loadMat;
    [SerializeField] private float loadDuration;
    private bool isLoading = false;
    private bool eventBroadcasted = false;
    private float timer = 0f;
    private float duration = 5f;
    
    [SerializeField] private GameObject fXPanel;
    [SerializeField] private GameObject ring1;
    [SerializeField] private GameObject ring2;
    [SerializeField] private GameObject loadBar;
    [SerializeField] private GameObject iconPanel;
    
    public event EventHandler TransistorLoadEnd;
    
    private void Start()
    {
        kerframeMat.SetFloat(paraName, 0f);
        loadMat.SetFloat(paraName, 0f);
        PlayKeyframeMatAnim();
    }

    private void Update()
    {
        if (isLoading && loadMat != null)
        {
            if (timer < loadDuration)
            {
                timer += Time.deltaTime;
                float value = Mathf.Clamp01(timer / loadDuration);
                loadMat.SetFloat(paraName, value);
                if (value >= 1f && !eventBroadcasted)
                {
                    TransistorLoadEnd?.Invoke(this, EventArgs.Empty); 
                    eventBroadcasted = true;  // 设置标志位，防止多次广播
                    Destroy(gameObject);
                }
            }
        }
    }

    public void StartLoadTransistor()
    {
        isLoading = true;
        eventBroadcasted = false;  // 重置标志位
        timer = 0f;
    }

    public void ExitLoadTransistor()
    {
        isLoading = false;
        timer = 0f;
        eventBroadcasted = false; 
    }

    public void ResetMatAnim()
    {
        loadMat.SetFloat(paraName, 0f);
        fXPanel.transform.DOScale(1.0354f, .5f).SetEase(Ease.OutQuad);
        ring2.transform.DOScale(.1f,.5f).SetDelay(.1f).SetEase(Ease.OutQuad);
        ring1.transform.DOScale(0.09f,.5f).SetDelay(.3f).SetEase(Ease.OutQuad);
        loadBar.transform.DOScale(.25f,.5f).SetDelay(.4f).SetEase(Ease.OutQuad);
        iconPanel.transform.DOScale(.3f,.5f).SetDelay(.5f).SetEase(Ease.OutQuad);
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
