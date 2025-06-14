using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardBase : ScriptableObject
{
    //public int id;
    public string cardName;
    public CardRarity rarity;//Ï¡ÓÐ¶È
    [Range(-1,10)]
    public int ramge;//·¶Î§
    //Ä¬ÈÏ-1¼´ÎÞ·¶Î§¿¨ÅÆ
    public string description;//ÃèÊö
    [Range(0,5)]
    protected int weight;
    public abstract void UseCard(BasePlayerController player);
    public int GetWeight()
    {
        return weight;
    }
}

