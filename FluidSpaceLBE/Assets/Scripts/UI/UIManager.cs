using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public UnityEvent OnPressedB;
    private void Start()
    {
        PlayerInputManager.Instance.SecondaryPressed_EventHandler += SecondaryPressed;
    }

    private void SecondaryPressed(object sender, EventArgs e)
    {
        OnPressedB?.Invoke();
    }
}
