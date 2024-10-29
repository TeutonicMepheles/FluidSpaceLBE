using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoundaryMatController : MonoBehaviour
{
    public Material activatedMat;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInputManager.Instance.StartSelection_EventHandler += StartSelection;
        PlayerInputManager.Instance.EndSelection_EventHandler += EndSelection;
    }

    private void EndSelection(object sender, EventArgs e)
    {
        activatedMat.DOFloat(0.05f, "_ShowArea", 0.5f);
    }

    private void StartSelection(object sender, EventArgs e)
    {
        activatedMat.DOFloat(1f, "_ShowArea", 0.5f);
    }

}
