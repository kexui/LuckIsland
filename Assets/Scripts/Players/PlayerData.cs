using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public event Action OnDataChanged;
    public string characterName { get; private set; }//��ɫ��
    public int playerName { get; private set; }//���ID

    private int currentTileIndex;//��ǰ�������
    public int CurrentTileIndex
    {
        get => currentTileIndex;
        set => currentTileIndex = Mathf.Max(0,value);
    }

    private int totalSteps;//����
    public int TotalSteps
    { 
        get => totalSteps;
        set 
        {//�÷�����װ����
            totalSteps = value;
            remainingSteps = value;
            //OnDataChanged?.Invoke();//���ݸı��¼�
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
    private int copper;
    public int Copper=>copper;//ֻ����

    private int luck;

    //����������ñ���
    public bool autoRollDIce = false;//�Զ�ҡ����

    public GameObject character;
    public BasePlayerController playerController;
    public List<TileBase> ownedTiles;



    public PlayerData(string name,int id,GameObject characterPrefab)
    {
        //��ʼ��
        characterName = name;
        playerName = id;
        character = GameObject.Instantiate(characterPrefab);
        currentTileIndex = 0;
        totalSteps = 0;
        remainingSteps = 0;
        copper = 5;
        ownedTiles = new List<TileBase>();

        character.GetComponent<BasePlayerController>().playerData = this;
        playerController = character.GetComponent<BasePlayerController>();
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
    {//Ǯ��
        return copper >= amount;
    }
    public int GetCurrentStep()
    {//��ǰ����
        return totalSteps-remainingSteps;
    }
}
