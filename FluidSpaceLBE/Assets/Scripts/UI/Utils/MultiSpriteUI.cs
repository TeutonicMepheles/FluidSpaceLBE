using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class MultiSpriteUI : MonoBehaviour
{
    [SerializeField] private Image imageComponent;
    [SerializeField] private Sprite[] spriteList;

    public void SetupSprite(bool isDefault)
    {
        if (spriteList != null)
        {
            if (isDefault)
            {
                imageComponent.sprite = spriteList[0];
            }
            else
            {
                imageComponent.sprite = spriteList[1];
            }
        }
    }
}
