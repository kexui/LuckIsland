using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField]private CardLibrary cardLibrary;//¿¨×é
    private List<CardDataBase> drawPool;//¿¨ÅÆ³éÈ¡³Ø

    private int[] rarityWeights;

    private void Start()
    {
        rarityWeights = new int[] { 1, 2, 5 };
    }

    public void GenerateDrawPool()
    {//Éú³É¿¨ÅÆ³éÈ¡³Ø
        drawPool = new List<CardDataBase>();
        foreach (CardDataBase card in cardLibrary.AllCards)
        {
            for (int i = 0; i < card.Weight; i++)
            {
                int index = (int)card.Rarity;
                for (int j = 0; j < rarityWeights[index]; j++)
                {
                    drawPool.Add(card);
                }
            }
        }
    }

    public CardDataBase DrawRandomCard()
    {
        if (drawPool == null||drawPool.Count==0)
            GenerateDrawPool();
        int index = Random.Range(0, drawPool.Count);
        return drawPool[index];
        //¿¨ÅÆÉ¾³ý£¿
    }

}
