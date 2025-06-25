using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

/// <summary>
/// 卡牌基类
/// </summary>
public abstract class CardDataBase : ScriptableObject
{
    // 卡牌唯一ID（如有需要可启用）
    // [SerializeField] private int id;

    [Header("卡牌基础信息")]

    [SerializeField] protected string cardName;
    [SerializeField] protected CardRarity rarity; // 稀有度
    [Range(-1, 10)]
    [SerializeField] protected int range = -1;
    [TextArea]
    [SerializeField] protected string effectText; // 描述

    [Header("卡牌图像")]
    [SerializeField] protected Sprite faceImage; // 卡牌正面图片
    [SerializeField] protected Sprite frameImage; // 卡牌边框图片

    [Header("权重")]
    [Range(0, 5)]
    [SerializeField] protected int weight;

    //属性访问器
    public string CardName=>cardName;
    public CardRarity Rarity => rarity;
    public int Range => range;
    public string EffectText => effectText;
    public Sprite FaceImage => faceImage;
    public Sprite FrameImage => frameImage;
    public int Weight => weight;

    /// <summary>
    /// 使用卡牌
    /// </summary>
    /// <param name="player">目标玩家</param>
    public abstract void UseCard(BasePlayerController player);

    public Sprite GetFrameByRarity(CardRarity rarity)
    {
        switch (rarity)
        {
            case CardRarity.Gold:
                return Resources.Load<Sprite>("FrameImages/Gold");
            case CardRarity.Silver:
                return Resources.Load<Sprite>("FrameImages/Silver");
            case CardRarity.Bronze:
                return Resources.Load<Sprite>("FrameImages/Bronze");
            default:
                Debug.LogWarning("CardDataBase");
                return null;
        }
    }
}
