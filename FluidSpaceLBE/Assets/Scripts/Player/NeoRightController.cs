using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class NeoRightController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer; // 当前手柄的Skinned Mesh Renderer
    public Material outlineMaterial; // 描边材质
    private Material[] originalMaterials;
    
    [SerializeField] private GameObject tutorUIteleport;
    [SerializeField] private GameObject tutorUIescape;
    [SerializeField] private GameObject tutorUImodify;
    private bool isInTransition = false;
    private bool isPlayerLearnedTeleport = false;
    public float startTutorUITime = 7f;
    private float timer;
    private bool isTutorUIshown;
    void Start()
    {
        // 获取并保存当前手柄的初始材质数组
        originalMaterials = skinnedMeshRenderer.materials;
        PlayerInputManager.Instance.StartSelection_EventHandler += StartSelection;
        PlayerInputManager.Instance.EndSelection_EventHandler += EndSelection;
        PlayerManager.Instance.PlayerInVisBoundary_EventHandler += ShowEscapeUI;
        PlayerInputManager.Instance.TransitionStarted_EventHandler += TransitionStart;
        
        timer = startTutorUITime;
        isTutorUIshown = false;
    }

    private void Update()
    {
        if (!isPlayerLearnedTeleport && !PlayerManager.Instance.isInPhysical)
        {
            if (PlayerManager.Instance.isMoveLearned)
            {
                timer -= Time.deltaTime;
                if (timer <= 0 && !isTutorUIshown)
                {
                    SetOutline(true);
                    tutorUIteleport.SetActive(true);
                    isTutorUIshown = true;
                    PlayerManager.Instance.isTeleportLearned = true;
                }
            }
            else
            {
                if (tutorUIteleport.activeSelf)
                {
                    tutorUIteleport.SetActive(false);
                    SetOutline(false);
                }
            }
        }
        else
        {
            // SetOutline(false);
            tutorUIteleport.SetActive(false);
        }
    }
    

    private void TransitionStart(object sender, EventArgs e)
    {
        isInTransition = true;
    }

    private void ShowEscapeUI(object sender, PlayerManager.PlayerBoundStateEventArgs e)
    {
        if (!isInTransition)
        {
            if (!e.isInBoundary)
            {
                tutorUIescape.SetActive(true);
                SetOutline(true);
            }
            else
            {
                tutorUIescape.SetActive(false);
                SetOutline(false);
            }
        }
        else
        {
            tutorUIescape.SetActive(false);
            SetOutline(false);
        }
    }
    
    private void EndSelection(object sender, EventArgs e)
    {
        tutorUImodify.SetActive(false);
        SetOutline(false);
    }

    private void StartSelection(object sender, EventArgs e)
    {
        isPlayerLearnedTeleport = true;
        tutorUImodify.SetActive(true);
        SetOutline(true);
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
