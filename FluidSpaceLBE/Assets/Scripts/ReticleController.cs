using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReticleController : MonoBehaviour
{
    private Animator animator;
    void OnEnable()
    {
        TeleportationHandler.ReticleInBoundary += OnReticleInBoundary;
        TeleportationHandler.ReticleOutBoundary += OnReticleOutBoundary;
    }

    void OnDisable()
    {
        TeleportationHandler.ReticleInBoundary -= OnReticleInBoundary;
        TeleportationHandler.ReticleOutBoundary -= OnReticleOutBoundary;
    }
    
    void Start()
    {
        // 获取Animator组件
        animator = GetComponent<Animator>();
    }
    void OnReticleInBoundary()
    {
        animator.SetBool("inBound", true);
    }

    void OnReticleOutBoundary()
    {
        animator.SetBool("inBound", false);
    }
}