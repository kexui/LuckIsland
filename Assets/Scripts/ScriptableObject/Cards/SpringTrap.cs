using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpringTrap", menuName = "Card/SpringTrap")]
public class SpringTrap : CardDataBase
{//���ɿ�
    [Header("���˲���")]
    [Range(0,3)]
    private int backwardSteps;

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
        //���õ���
        Debug.Log("ʹ�õ��ɿ�������" + backwardSteps + "��");
    }
}
