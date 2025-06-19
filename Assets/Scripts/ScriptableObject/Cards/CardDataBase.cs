using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌基类
/// </summary>
public abstract class CardDataBase : ScriptableObject
{
    // 卡牌唯一ID（如有需要可启用）
    // [SerializeField] private int id;

    [Header("基本信息")]
    public string cardName;
    public CardRarity rarity; // 稀有度

    [Range(-1, 10)]
    public int range = -1; // 范围，-1为无范围卡牌

    [TextArea]
    public string effectText; // 描述

    [Header("美术资源")]
    public string faceImage; // 卡牌正面图片
    public string frameImage; // 卡牌边框图片

    [Header("权重")]
    [Range(0, 5)]
    [SerializeField] protected int weight;

    /// <summary>
    /// 使用卡牌
    /// </summary>
    /// <param name="player">目标玩家</param>
    public abstract void UseCard(BasePlayerController player);

    /// <summary>
    /// 获取卡牌权重
    /// </summary>
    public int GetWeight() => weight;
}
