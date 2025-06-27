using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpringTrap", menuName = "Card/SpringTrap")]
public class SpringTrap : CardDataBase
{//弹簧卡
    [Header("后退步数")]
    [Range(0,3)]
    private int backwardSteps;

    private void OnValidate()
    {
        cardName = "后退卡";
        var sprite = GetFrameByRarity(rarity);
        if (sprite == null)
        {
            Debug.LogWarning("找不到frameImage");
            return;
        }
        frameImage = sprite;
    }

    public override void UseCard(BasePlayerController player)
    {
        //放置道具
        Debug.Log("使用弹簧卡，后退" + backwardSteps + "步");
    }
}
