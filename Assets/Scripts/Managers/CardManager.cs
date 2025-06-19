using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public CardDataBase[] allCards;//¿¨×é
    private List<CardDataBase> drawPool;
    
    private int[] rarityWeights;

    private void Start()
    {
        allCards = Resources.LoadAll<CardDataBase>("Cards");
        rarityWeights = new int[] { 1, 2, 5 };
    }

    public void GenerateDrawPool()
    {
        drawPool = new List<CardDataBase>();
        foreach (CardDataBase card in allCards)
        {
            for (int i = 0; i < card.GetWeight(); i++)
            {
                int index = (int)card.rarity;
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
