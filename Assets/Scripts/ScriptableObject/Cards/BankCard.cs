using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBankCard", menuName = "Card/BankCard")]
public class BankCard : CardDataBase
{
    [Range(0,6)]
    public int moneyAmount; // ���п����
    private void OnEnable()
    {
        cardName = "BankCard";
        weight = 1;
    }
    public override void UseCard(BasePlayerController player)
    {
        //����
        player.playerData.AddCopper(moneyAmount);
        Debug.Log("ʹ�����п���" + moneyAmount);
    }
}
