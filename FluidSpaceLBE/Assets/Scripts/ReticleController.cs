using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReticleController : MonoBehaviour
{
    private Animator animator;
    public bool switchState;

    void Start()
    {
        // 获取Animator组件
        animator = GetComponent<Animator>();
        TeleportationHandler.OnStateChange += HandleStateChange;
    }

    private void OnDestroy()
    {
        TeleportationHandler.OnStateChange -= HandleStateChange;
    }

    private void HandleStateChange(bool switchState)
    {
        animator.SetBool("inBound",switchState);
    }
}
