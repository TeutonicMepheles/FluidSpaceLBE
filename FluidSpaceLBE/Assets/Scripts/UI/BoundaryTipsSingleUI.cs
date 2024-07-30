using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoundaryTipsSingleUI : MonoBehaviour
{
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetCoPlayerSO(PlayerSO coPlayer)
    {
        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate)
            {
                iconTemplate.gameObject.SetActive(true);
                iconTemplate.GetComponent<Image>().enabled = true;
                iconTemplate.GetComponent<Image>().sprite = coPlayer.playerSprite;
            }
            else
            {
                continue;
            }
        }
    }
}
