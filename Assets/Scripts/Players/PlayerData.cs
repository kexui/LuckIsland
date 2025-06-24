using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerData//�������
{
    public event Action OnDataChanged;//���ݸı��¼�

    public int ID { get; private set; }//���ID
    public string PlayerName { get; private set; }

    private int currentTileIndex;//��ǰ�������
    public int CurrentTileIndex
    {
        get => currentTileIndex;
        set
        {
            currentTileIndex = Mathf.Max(0, value);
            OnDataChanged?.Invoke();//���ݸı��¼�
        }
    }

    private int totalSteps;//����
    public int TotalSteps
    { 
        get => totalSteps;
        set 
        {//�÷�����װ����
            totalSteps = value;
            remainingSteps = value;
            OnDataChanged?.Invoke();//���ݸı��¼�
        }
    }
    private int remainingSteps;//ʣ�ಽ��
    public int RemainingSteps
    {
        get { return remainingSteps; }
        set 
        {
            remainingSteps = Mathf.Max(0, value);
            OnDataChanged?.Invoke();//���ݸı��¼�
        }
    }
    public DiceController dice{ get; private set; }


    private int copper;
    public int Copper=>copper;//ֻ����

    private int luck;
    public int Luck
    {
        get => luck;
        set
        {
            luck = Mathf.Max(0, value);
            OnDataChanged?.Invoke();//���ݸı��¼�
        }
    }
    private List<CardDataBase> handCards;


    //����������ñ���
    public bool autoRollDIce = false;//�Զ�ҡ����

    public CharacterData characterData { get; private set; }//��ɫ
    public BasePlayerController playerController { get; private set; }//��ҿ�����

    public List<TileBase> ownedTiles;



    public PlayerData(int id,CharacterData characterData,BasePlayerController playerController)
    {
        //��ʼ��
        ID = id;
        currentTileIndex = 0;
        totalSteps = 0;
        remainingSteps = 0;
        PlayerName = "Player " + id; //Ĭ�������

        copper = 15;
        ownedTiles = new List<TileBase>();

        dice = DiceManager.Instance.diceArray[id];//��ȡ����

        this.characterData = characterData;
        this.playerController = playerController;
        playerController.Init(this);
    }

    public void AddCopper(int amount)
    { 
        copper += amount;
        OnDataChanged?.Invoke();//���ݸı��¼�
    }
    public bool SubtractCopper(int amount)
    {
        if (copper>=amount)
        {//Ǯ��
            copper -= amount;
            OnDataChanged?.Invoke();//���ݸı��¼�
            return true;
        }
        return false;
    }
    public bool HasEnoughCopper(int amount)
    {//Ǯ������
        return copper >= amount;
    }
    public int GetCurrentStep()
    {//��ǰ����
        return totalSteps-remainingSteps;
    }
    public void AddHandCard(CardDataBase card)
    { 
        handCards.Add(card);
        OnDataChanged?.Invoke();//���ݸı��¼�
    }
    public void RemoveHandCard(CardDataBase card)
    {
        if (handCards.Contains(card))
        {
            handCards.Remove(card);
            OnDataChanged?.Invoke();//���ݸı��¼�
        }
    }

}
