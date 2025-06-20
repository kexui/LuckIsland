using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField]private CardLibrary cardLibrary;//����
    private List<CardDataBase> drawPool;//���Ƴ�ȡ��

    private int[] rarityWeights;

    private void Start()
    {
        rarityWeights = new int[] { 1, 2, 5 };
    }

    public void GenerateDrawPool()
    {//���ɿ��Ƴ�ȡ��
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
        //����ɾ����
    }

}
