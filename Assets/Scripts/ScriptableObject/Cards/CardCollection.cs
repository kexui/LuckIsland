using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBankCard", menuName = "Card/CardCollection")]
public class CardCollection : ScriptableObject
{
    public List<CardDataBase> cardCollection; // ¿¨×é
}
