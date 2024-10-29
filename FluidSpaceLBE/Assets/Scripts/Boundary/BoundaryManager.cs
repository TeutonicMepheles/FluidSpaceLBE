using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class BoundaryManager : MonoBehaviour
{
    public bool isCalibrated;
    public Material activatedMat; // 激活态材质
    public Material selectedMat; // 选中态材质
    
    public List<string> parameterNames; // 材质参数名列表
    public List<float> endValues; // 参数目标值列表
    public List<float> durations; // 各参数动画时长列表
    private List<float> startValues = new List<float>(); // 参数初始值列表
    
    private bool isSelected = false;
    private List<Tween> parameterTweens = new List<Tween>();
    [SerializeField] private MeshRenderer targetRenderer;
    
    public List<PlayerSO> playerList;
    public float destroyLimit = 5f;
    private float currentTimer = 0f;
    private bool isDestroying = false;
    public GameObject physicalSpace;
    
    private void Start()
    {
        isCalibrated = false;
        TeleportationManager.Instance.BoundarySelected_EventHandler += UpdateBoundaryMat;
        PlayerInputManager.Instance.EndSelection_EventHandler += EndSelection;
        
        // 从staticMat读取并初始化selectedMat的参数
        for (int i = 0; i < parameterNames.Count; i++)
        {
            float startValue = activatedMat.GetFloat(parameterNames[i]);
            startValues.Add(startValue);
            selectedMat.SetFloat(parameterNames[i], startValue);
        }
    }

    private void EndSelection(object sender, EventArgs e)
    {
        // 停止所有参数变化并重置
        ResetMaterialParameters();
        // 切换回静态材质
        targetRenderer.material = activatedMat;
    }

    private void UpdateBoundaryMat(object sender, TeleportationManager.BoundarySelectedEventArgs e)
    {
        // 检查事件传递的boundaryManager是否与当前对象一致
        if (e.boundaryManager == this)
        {
            ChangeMaterials(true); // 被选中
        }
        else
        {
            ChangeMaterials(false); // 未被选中
        }
    }
    
    private void ChangeMaterials(bool isSelected)
    {
        if (isSelected)
        {
            // 切换材质
            targetRenderer.material = selectedMat;
            
            // 修改所有参数
            for (int i = 0; i < parameterNames.Count; i++)
            {
                ChangeMaterialParameter(parameterNames[i], endValues[i], durations[i]);
            }
        }
        else
        {
            // 停止所有参数变化并重置
            ResetMaterialParameters();
            // 切换回静态材质
            targetRenderer.material = activatedMat;
        }
    }
    
    private void ChangeMaterialParameter(string parameterName, float endValue, float duration)
    {
        // 动态修改材质参数
        Tween tween = selectedMat.DOFloat(endValue, parameterName, duration);
        parameterTweens.Add(tween);
    }

    private void ResetMaterialParameters()
    {
        foreach (var tween in parameterTweens)
        {
            if (tween != null && tween.IsActive())
            {
                tween.Kill();
            }
        }
        parameterTweens.Clear();

        // 重置所有参数到初始值
        for (int i = 0; i < parameterNames.Count; i++)
        {
            selectedMat.SetFloat(parameterNames[i], startValues[i]);
        }
    }

    private void Update()
    {
        UpdateDestroy();
    }

    private void UpdateDestroy()
    {
        if (playerList.Count == 0)
        {
            // 如果列表为空且销毁尚未开始，初始化计时器并标记销毁开始
            if (!isDestroying)
            {
                currentTimer = destroyLimit;
                isDestroying = true;
            }

            // 每帧减少计时器
            currentTimer -= Time.deltaTime;

            // 如果计时器小于等于0，销毁游戏对象
            if (currentTimer <= 0f)
            {
                Debug.Log("Destroy!");
                gameObject.SetActive(false);
            }
        }
        else
        {
            // 如果列表中有新内容，重置标记和计时器
            isDestroying = false;
            currentTimer = 0f;
        }
    }

    public void RegisterPlayerToBoundary(PlayerSO player)
    {
        if (!playerList.Contains(player))
        {
            playerList.Add(player);
        }
    }

    public void DeregisterPlayerToBoundary(PlayerSO player)
    {
        if (playerList.Contains(player))
        {
            playerList.Remove(player);
        }
    }

    public void BoundaryCalibrated()
    {
        physicalSpace.SetActive(true);
        isCalibrated = true;
        Debug.Log("Calibrated"+gameObject.name);
    }
}
