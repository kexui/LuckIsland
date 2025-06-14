using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardBase : ScriptableObject
{
    //public int id;
    public string cardName;
    public CardRarity rarity;//ϡ�ж�
    [Range(-1,10)]
    public int ramge;//��Χ
    //Ĭ��-1���޷�Χ����
    public string description;//����
    [Range(0,5)]
    protected int weight;
    public abstract void UseCard(BasePlayerController player);
    public int GetWeight()
    {
        return weight;
    }
}

