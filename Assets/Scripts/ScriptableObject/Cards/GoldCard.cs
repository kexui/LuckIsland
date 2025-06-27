using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGoldCard", menuName = "Card/GoldCard")]
public class GoldCard : CardDataBase
{
    [Header("金币数")]
    [Range(1,6)]
    [SerializeField]private int moneyAmount; // 银行卡金额
    private void OnValidate()
    {
        cardName = "金币卡";
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
        //方法
        player.playerData.AddCopper(moneyAmount);
        Debug.Log("使用金币卡加" + moneyAmount);
    }
}
