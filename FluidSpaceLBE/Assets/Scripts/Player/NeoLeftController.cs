using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class NeoLeftController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer; // 当前手柄的Skinned Mesh Renderer
    public Material outlineMaterial; // 描边材质
    private Material[] originalMaterials;
    [SerializeField] private GameObject tutorUI;
    
    private bool isPlayerLearnedMove = false;
    public float startTutorUITime = 7f;
    private float timer;
    private bool isTutorUIshown = false;
    
    public 
    
    void Start()
    {
        // 获取并保存当前手柄的初始材质数组
        originalMaterials = skinnedMeshRenderer.materials;
        PlayerInputManager.Instance.StartMove += PlayerStartMove;
        timer = startTutorUITime;
        isTutorUIshown = false;
    }

    private void Update()
    {
        if (!isPlayerLearnedMove && !PlayerManager.Instance.isInPhysical)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && !isTutorUIshown)
            {
                SetOutline(true);
                tutorUI.SetActive(true);
                isTutorUIshown = true;
                PlayerManager.Instance.isMoveLearned = true;
            }
        }
        else
        {
            if (tutorUI.activeSelf)
            {
                tutorUI.SetActive(false);
                SetOutline(false);
                isTutorUIshown = false;
            }
        }
    }

    private void PlayerStartMove(object sender, EventArgs e)
    {
        isPlayerLearnedMove = true;
    }

    private void SetOutline(bool enable)
    {
        Material[] materials = skinnedMeshRenderer.materials;

        if (enable)
        {
            // 显示描边材质
            materials[materials.Length - 1] = outlineMaterial;
        }
        else
        {
            // 隐藏描边材质，恢复初始材质
            materials[materials.Length - 1] = originalMaterials[originalMaterials.Length - 1];
        }
        
        skinnedMeshRenderer.materials = materials;
    }
}
