using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [SerializeField]private CardLibrary cardLibrary;//����
    private List<CardDataBase> drawPool;//���Ƴ�ȡ��

    private int[] rarityWeights;

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        GameManager.OnLoadResources += GameManager_OnLoadResources;
    }

    private void GameManager_OnLoadResources()
    {
        rarityWeights = new int[] { 1, 2, 3 };
        GenerateDrawPool();
    }

    public void DealCardsToAllPlayers(List<PlayerData> players)
    {
        if (drawPool == null || drawPool.Count == 0)
        {
            Debug.LogWarning("���Ƴ�ȡ��Ϊ�գ��޷�����");
            return;
        }
        foreach (PlayerData player in players)
        {
            DealCardsToplayer(player);
        }
    }
    public void DealCardsToplayer(PlayerData playerData)
    {
        CardDataBase newCard = GetRandomCard();
        if (newCard != null)
        {
            playerData.AddHandCard(newCard);
            Debug.Log($"���Ƹ���� {playerData.PlayerName}��{newCard.CardName}");
        }
        else
        {
            Debug.LogWarning("�޷����ƣ���ȡ��Ϊ�ջ�δ��ȷ���ɡ�");
        }
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

    public CardDataBase GetRandomCard()
    {
        if (drawPool == null || drawPool.Count == 0)
        {
            Debug.Log("���Ƴ�Ϊ��");
        }
        int index = Random.Range(0, drawPool.Count);
        CardDataBase newCard = drawPool[index];
        drawPool.RemoveAt(index); // �ӳ�ȡ�����Ƴ��ѳ�ȡ�Ŀ���
        return newCard;
    }
}
