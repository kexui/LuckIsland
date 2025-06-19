using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBankCard", menuName = "Card/BankCard")]
public class BankCard : CardDataBase
{
    [Range(0,6)]
    public int moneyAmount; // 银行卡金额
    private void OnEnable()
    {
        cardName = "BankCard";
        weight = 1;
    }
    public override void UseCard(BasePlayerController player)
    {
        //方法
        player.playerData.AddCopper(moneyAmount);
        Debug.Log("使用银行卡加" + moneyAmount);
    }
}
