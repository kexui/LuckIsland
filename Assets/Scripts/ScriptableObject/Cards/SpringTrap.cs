using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBankCard", menuName = "Card/SpringTrap")]
public class SpringTrap : CardDataBase
{
    [Header("���˲���")]
    [Range(0,3)]
    public int backwardSteps;


    private void OnValidate()
    {
        cardName = "���˿�";
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
        //�˺󷽷�
        Debug.Log("����");
    }
}
