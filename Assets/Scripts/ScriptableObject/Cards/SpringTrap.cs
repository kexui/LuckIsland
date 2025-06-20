using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBankCard", menuName = "Card/SpringTrap")]
public class SpringTrap : CardDataBase
{
    [Header("后退步数")]
    [Range(0,3)]
    public int backwardSteps;


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
        //退后方法
        Debug.Log("后退");
    }
}
