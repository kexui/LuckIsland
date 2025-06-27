using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [SerializeField]private CardLibrary cardLibrary;//卡组
    private List<CardDataBase> drawPool;//卡牌抽取池

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
            Debug.LogWarning("卡牌抽取池为空，无法发牌");
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
            Debug.Log($"发牌给玩家 {playerData.PlayerName}：{newCard.CardName}");
        }
        else
        {
            Debug.LogWarning("无法发牌，抽取池为空或未正确生成。");
        }
    }

    public void GenerateDrawPool()
    {//生成卡牌抽取池
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
            Debug.Log("卡牌池为空");
        }
        int index = Random.Range(0, drawPool.Count);
        CardDataBase newCard = drawPool[index];
        drawPool.RemoveAt(index); // 从抽取池中移除已抽取的卡牌
        return newCard;
    }
}
