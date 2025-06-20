using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBankCard", menuName = "Card/BankCard")]
public class BankCard : CardDataBase
{
    [Header("���п����")]
    [Range(0,6)]
    public int moneyAmount; // ���п����
    private void OnValidate()
    {
        cardName = "���п�";
        var sprite = GetFrameByRarity(rarity);
        if (sprite == null)
        {
            Debug.LogWarning("�Ҳ���frameImage");
            return;
        }
        frameImage = sprite;
    }
    public override void UseCard(BasePlayerController player)
    {
        //����
        player.playerData.AddCopper(moneyAmount);
        Debug.Log("ʹ�����п���" + moneyAmount);
    }
}
