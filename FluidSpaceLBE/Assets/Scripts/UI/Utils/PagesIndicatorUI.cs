using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PagesIndicatorUI : MonoBehaviour
{
    public GameObject next;
    public GameObject close;
    public GameObject[] pages;
    private int pageNow;
    private void Start()
    {
        PlayerInputManager.Instance.SecondaryPressed_EventHandler += OnSecondaryPressed;
        pageNow = 0;
    }

    private void OnSecondaryPressed(object sender, EventArgs e)
    {
        if (pageNow + 1 != pages.Length)
        {
            pageNow++;
            ShowPage();
            next.SetActive(true);
            close.SetActive(false);
        }

        if (pageNow + 1 == pages.Length)
        {
            next.SetActive(false);
            close.SetActive(true);
        }
    }

    public void ShowPage()
    {
        FluidSpaceUtils.ShowListItem(pages,false);
        pages[pageNow].SetActive(true);
    }
}
