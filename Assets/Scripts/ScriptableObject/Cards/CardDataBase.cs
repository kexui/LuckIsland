using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ƻ���
/// </summary>
public abstract class CardDataBase : ScriptableObject
{
    // ����ΨһID��������Ҫ�����ã�
    // [SerializeField] private int id;

    [Header("������Ϣ")]
    public string cardName;
    public CardRarity rarity; // ϡ�ж�

    [Range(-1, 10)]
    public int range = -1; // ��Χ��-1Ϊ�޷�Χ����

    [TextArea]
    public string effectText; // ����

    [Header("������Դ")]
    public string faceImage; // ��������ͼƬ
    public string frameImage; // ���Ʊ߿�ͼƬ

    [Header("Ȩ��")]
    [Range(0, 5)]
    [SerializeField] protected int weight;

    /// <summary>
    /// ʹ�ÿ���
    /// </summary>
    /// <param name="player">Ŀ�����</param>
    public abstract void UseCard(BasePlayerController player);

    /// <summary>
    /// ��ȡ����Ȩ��
    /// </summary>
    public int GetWeight() => weight;
}
