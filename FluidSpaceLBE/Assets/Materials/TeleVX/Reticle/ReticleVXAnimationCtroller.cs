using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleVXAnimationCtroller : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void SetState(string stateName)
    {
        ResetTriggers();
        animator.SetTrigger(stateName);
    }

    private void ResetTriggers()
    {
        animator.ResetTrigger("Teleport");
        animator.ResetTrigger("Disconnect");
        animator.ResetTrigger("Joint");
        animator.ResetTrigger("Overlap");
    }
}
