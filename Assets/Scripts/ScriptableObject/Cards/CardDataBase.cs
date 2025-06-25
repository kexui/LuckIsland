using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

/// <summary>
/// ���ƻ���
/// </summary>
public abstract class CardDataBase : ScriptableObject
{
    // ����ΨһID��������Ҫ�����ã�
    // [SerializeField] private int id;

    [Header("���ƻ�����Ϣ")]

    [SerializeField] protected string cardName;
    [SerializeField] protected CardRarity rarity; // ϡ�ж�
    [Range(-1, 10)]
    [SerializeField] protected int range = -1;
    [TextArea]
    [SerializeField] protected string effectText; // ����

    [Header("����ͼ��")]
    [SerializeField] protected Sprite faceImage; // ��������ͼƬ
    [SerializeField] protected Sprite frameImage; // ���Ʊ߿�ͼƬ

    [Header("Ȩ��")]
    [Range(0, 5)]
    [SerializeField] protected int weight;

    //���Է�����
    public string CardName=>cardName;
    public CardRarity Rarity => rarity;
    public int Range => range;
    public string EffectText => effectText;
    public Sprite FaceImage => faceImage;
    public Sprite FrameImage => frameImage;
    public int Weight => weight;

    /// <summary>
    /// ʹ�ÿ���
    /// </summary>
    /// <param name="player">Ŀ�����</param>
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
