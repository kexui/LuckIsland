using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardLibrary", menuName = "Card/CardLibrary")]
public class CardLibrary : ScriptableObject
{
    [SerializeField]private List<CardDataBase> allCards;
    public List<CardDataBase> AllCards => allCards;
    public CardDataBase GetCardByName(string cardName)
    {
        foreach (var card in allCards)
        {
            if (card.CardName == cardName)
            {
                return card;
            }
        }
        Debug.LogWarning($"Card with name {cardName} not found in the library.");
        return null;
    }
}
