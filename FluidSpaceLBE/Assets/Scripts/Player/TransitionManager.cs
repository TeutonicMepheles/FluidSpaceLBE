using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.XR.PXR;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class TransitionManager : MonoBehaviour
{
    public GameObject tempFunc_Tracker;
    public static TransitionManager Instance { get; private set; }

    [Header("转场材质")]
    [SerializeField] private Material transitionMaterial; // 需要控制的材质
    
    [Header("滤镜材质")]
    [SerializeField] private Material filterMaterial; // 需要控制的材质
    
    [Header("转场时间")]
    [SerializeField] private float transitionDuration = 1f; // 渐变持续时间

    [Header("触发器")]
    [SerializeField] private GameObject transistor;
    private GameObject currentTransistor = null;
    private TransistorAnimController currentTransistorAnim;

    [Header("偏移距离")] 
    [SerializeField] private float offsetDist = 0.8f;
    [SerializeField] private ScriptableRendererFeature renderFeature;
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private GameObject visual, physical;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }
    
    private void Start()
    {
        PlayerInputManager.Instance.TransitionStarted_EventHandler += OnTransitionStarted_EventHandler;
        // PlayerManager.Instance.PlayerInPhyBoundary_EventHandler += OnPlayerInPhyBoundary;
        visual.SetActive(false);
        physical.SetActive(true);
        gridGenerator.Show();
        filterMaterial.SetFloat("_Strengh", 0.75f);
        filterMaterial.SetFloat("_Blend", 1f);
    }

    private void OnTransitionStarted_EventHandler(object sender, EventArgs e) // 在虚空间中，可通过按键唤起
    {
        if (!PlayerManager.Instance.isInPhysical)
        {
            SpawnTransistor();
            PlayerManager.Instance.HideCanvas();
        }
    }
    
    public void SpawnTransistor()
    {
        // 获取Camera.main的前向视线方向
        Vector3 forwardDirection = Camera.main.transform.forward;

        // 计算XZ平面上的投影（将Y分量置为0）
        Vector3 forwardDirectionXZ = new Vector3(forwardDirection.x, 0, forwardDirection.z);

        // 归一化方向向量
        forwardDirectionXZ.Normalize();

        // 计算偏移后的目标位置
        Vector3 targetPosition = Camera.main.transform.position + forwardDirectionXZ * offsetDist+ new Vector3(0f,-0.35f,0f);

        if (currentTransistor != null)
        {
            currentTransistor.transform.position = targetPosition;
        }
        else
        {
            // 实例化Prefab
            currentTransistor = Instantiate(transistor, targetPosition, Quaternion.identity);
            currentTransistorAnim = currentTransistor.GetComponent<TransistorAnimController>();
        }
        currentTransistor.GetComponent<TransistorManager>().SetTransistorSprite(PlayerManager.Instance.isInPhysical);
    }

    public void VisualToPhysical()
    {
        float currentAmount = transitionMaterial.GetFloat("_TransitionAmount");
        float targetAmount = currentAmount == 0f ? 1f : 0f;
        
        // 使用DoTween来控制TransitionAmount的渐变
        DOTween.To(() => currentAmount,
                value => transitionMaterial.SetFloat("_TransitionAmount", value),
                targetAmount,
                1f).SetEase(Ease.InQuint)
            .OnComplete(() =>
            {
                float currentAmount = transitionMaterial.GetFloat("_TransitionAmount");
                float targetAmount = currentAmount == 0f ? 1f : 0f;
                UpdateSceneTransitionFilter(false);
                gridGenerator.Show();
                PlayerManager.Instance.isInPhysical = true;
                PositionManager.Instance.SetPivot(Camera.main.transform.gameObject,PositionManager.PivotType.Vis);
                PositionManager.Instance.ResetPositionFromPivot(PositionManager.PivotType.Phy);
                visual.SetActive(false);
                physical.SetActive(true);
                tempFunc_Tracker.SetActive(true);
                // 使用DoTween来控制TransitionAmount的渐变
                DOTween.To(() => currentAmount,
                    value => transitionMaterial.SetFloat("_TransitionAmount", value),
                    targetAmount,
                    1f).SetEase(Ease.OutQuint)
                    .SetDelay(0.5f);
            });
    }
    
    public void PhysicalToVisual()
    {
        float currentAmount = transitionMaterial.GetFloat("_TransitionAmount");
        float targetAmount = currentAmount == 0f ? 1f : 0f;
        
        // 使用DoTween来控制TransitionAmount的渐变
        DOTween.To(() => currentAmount,
                value => transitionMaterial.SetFloat("_TransitionAmount", value),
                targetAmount,
                1f).SetEase(Ease.InQuint)
            .OnComplete(() =>
            {
                float currentAmount = transitionMaterial.GetFloat("_TransitionAmount");
                float targetAmount = 0f;
                UpdateSceneTransitionFilter(true);
                gridGenerator.Hide();
                PlayerManager.Instance.isInPhysical = false;
                PositionManager.Instance.SetPivot(Camera.main.transform.gameObject,PositionManager.PivotType.Phy);
                PositionManager.Instance.ResetPositionFromPivot(PositionManager.PivotType.Vis);
                visual.SetActive(true);
                physical.SetActive(false);
                // 使用DoTween来控制TransitionAmount的渐变
                DOTween.To(() => currentAmount,
                        value => transitionMaterial.SetFloat("_TransitionAmount", value),
                        targetAmount,
                        1f).SetEase(Ease.OutQuint)
                    .SetDelay(0.5f);
            });
    }
    public void UpdateSceneTransitionFilter(bool isPToV)
    {
        if (isPToV) // 实转虚
        {
            // Blend 从 1 变为 0
            filterMaterial.DOFloat(0.75f, "_Strengh", 0.5f);
            filterMaterial.DOFloat(0f, "_Blend", 0.5f);
        }
        else
        {
            // Blend 从 0 变为 1
            filterMaterial.DOFloat(0.75f, "_Strengh", 0.5f);
            filterMaterial.DOFloat(1f, "_Blend", 0.5f);
        }
    }

    public void UpdateTeleStateFilter(bool isOutBoundary)
    {
        float targetBlend = isOutBoundary ? 1f : 0f;
        float targetStrength = 0f;
        
        // 使用 DOTween 动画设置 Blend
        DOTween.To(() => filterMaterial.GetFloat("_Blend"), x => filterMaterial.SetFloat("_Blend", x), targetBlend, 0.5f);
        
        // 使用 DOTween 动画设置 Strength
        DOTween.To(() => filterMaterial.GetFloat("_Strengh"), x => filterMaterial.SetFloat("_Strengh", x), targetStrength, 0.5f);

        
    }
}