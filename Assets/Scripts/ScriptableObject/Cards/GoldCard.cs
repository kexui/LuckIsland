using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGoldCard", menuName = "Card/GoldCard")]
public class GoldCard : CardDataBase
{
    [Header("�����")]
    [Range(1,6)]
    [SerializeField]private int moneyAmount; // ���п����
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
        Debug.Log("ʹ�ý�ҿ���" + moneyAmount);
    }
}
