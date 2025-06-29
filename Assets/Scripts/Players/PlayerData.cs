using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerData//玩家数据
{
    public event Action OnDataChanged;//数据改变事件
    public event Action OnCardChanged;
    public int ID { get; private set; }//玩家ID
    public string PlayerName { get; private set; }

    private int currentTileIndex;//当前棋子序号
    public int CurrentTileIndex
    {
        get => currentTileIndex;
        set
        {
            currentTileIndex = Mathf.Max(0, value);
            OnDataChanged?.Invoke();//数据改变事件
        }
    }

    private int totalSteps;//步数
    public int TotalSteps
    { 
        get => totalSteps;
        set 
        {//用方法封装更好
            totalSteps = value;
            remainingSteps = value;
            OnDataChanged?.Invoke();//数据改变事件
        }
    }
    private int remainingSteps;//剩余步数
    public int RemainingSteps
    {
        get { return remainingSteps; }
        set 
        {
            remainingSteps = Mathf.Max(0, value);
            OnDataChanged?.Invoke();//数据改变事件
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
    public int Copper=>copper;//只访问

    private int luck;
    public int Luck
    {
        get => luck;
        set
        {
            luck = Mathf.Max(0, value);
            OnDataChanged?.Invoke();//数据改变事件
        }
    }
    

    //这个属于设置变量
    public bool autoRollDIce = false;//自动摇骰子

    public CharacterData characterData { get; private set; }//角色
    public BasePlayerController playerController { get; private set; }//玩家控制器

    public List<TileBase> ownedTiles;

    private List<CardDataBase> handCards;
    public List<CardDataBase> HandCards => handCards;//手牌



    public PlayerData(int id,CharacterData characterData,BasePlayerController playerController)
    {
        //初始化
        ID = id;
        currentTileIndex = 0;
        totalSteps = 0;
        remainingSteps = 0;
        PlayerName = "Player " + id; //默认玩家名

        copper = 15;
        ownedTiles = new List<TileBase>();
        handCards = new List<CardDataBase>();

        dice = DiceManager.Instance.diceArray[id];//获取骰子
        LopCount = 0;

        this.characterData = characterData;
        this.playerController = playerController;
        playerController.Init(this);
    }

    public void AddCopper(int amount)
    { 
        copper += amount;
        playerController.playerFloatingUI.ShowMessage(amount);
        OnDataChanged?.Invoke();//数据改变事件
    }
    public bool SubtractCopper(int amount)
    {
        if (copper>=amount)
        {//钱够
            copper -= amount;
            playerController.playerFloatingUI.ShowMessage(-amount);
            OnDataChanged?.Invoke();//数据改变事件
            return true;
        }
        return false;
    }
    public bool HasEnoughCopper(int amount)
    {//钱够不够
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
    {//当前步数
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
