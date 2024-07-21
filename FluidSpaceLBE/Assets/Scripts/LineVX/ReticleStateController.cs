using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReticleStateController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        // 获取Animator组件
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        TeleportationManager.Instance.BoundarySelectedEventHandler += SelectBoundary;
    }

    private void SelectBoundary(object sender, TeleportationManager.BoundarySelectedEventArgs e)
    {
        if (e.isSelectInBoundary)
        {
            Debug.Log("SelectInBoundary");
            animator.SetBool("inBound", true);
        }
        if (!e.isSelectInBoundary)
        {
            Debug.Log("SelectOutBoundary");
            animator.SetBool("inBound", false);
        }
    }
}