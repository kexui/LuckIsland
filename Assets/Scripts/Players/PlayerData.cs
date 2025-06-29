using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerData//�������
{
    public event Action OnDataChanged;//���ݸı��¼�
    public event Action OnCardChanged;
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

    private int lopCount;
    public int LopCount
    {
        get=> lopCount;
        set
        {
            lopCount = (int)Mathf.Clamp(value,0,4);
        }
    }

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
    

    //����������ñ���
    public bool autoRollDIce = false;//�Զ�ҡ����

    public CharacterData characterData { get; private set; }//��ɫ
    public BasePlayerController playerController { get; private set; }//��ҿ�����

    public List<TileBase> ownedTiles;

    private List<CardDataBase> handCards;
    public List<CardDataBase> HandCards => handCards;//����



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
        handCards = new List<CardDataBase>();

        dice = DiceManager.Instance.diceArray[id];//��ȡ����
        LopCount = 0;

        this.characterData = characterData;
        this.playerController = playerController;
        playerController.Init(this);
    }

    public void AddCopper(int amount)
    { 
        copper += amount;
        playerController.playerFloatingUI.ShowMessage(amount);
        OnDataChanged?.Invoke();//���ݸı��¼�
    }
    public bool SubtractCopper(int amount)
    {
        if (copper>=amount)
        {//Ǯ��
            copper -= amount;
            playerController.playerFloatingUI.ShowMessage(-amount);
            OnDataChanged?.Invoke();//���ݸı��¼�
            return true;
        }
        return false;
    }
    public bool HasEnoughCopper(int amount)
    {//Ǯ������
        return copper >= amount;
    }
    public void SetTotalSteps(int step)
    {
        int tileCount = TileManager.Instance.Tiles.Count;
        int maxSteps = tileCount - currentTileIndex;
        if (step> maxSteps)
        {
            totalSteps = maxSteps ;
            remainingSteps = maxSteps;
            lopCount++;
            Debug.Log("LopCount:" + lopCount);
        }
        else
        {
            totalSteps = step;
            remainingSteps = step;
        }
    }
    public int GetCurrentStep()
    {//��ǰ����
        return totalSteps-remainingSteps;
    }
    public void AddHandCard(CardDataBase card)
    { 
        handCards.Add(card);
        OnCardChanged?.Invoke();
    }

    public void RemoveHandCard(int index)
    {
        handCards.RemoveAt(index);
        OnCardChanged?.Invoke();
    }
}
