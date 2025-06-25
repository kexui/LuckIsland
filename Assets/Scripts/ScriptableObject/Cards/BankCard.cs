using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBankCard", menuName = "Card/BankCard")]
public class BankCard : CardDataBase
{
    [Header("���п����")]
    [Range(1,6)]
    private int moneyAmount; // ���п����
    private void OnValidate()
    {
        cardName = "��ҿ�";
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
