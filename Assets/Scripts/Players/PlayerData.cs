using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public event Action OnDataChanged;//数据改变事件

    private CharacterData characterData;//角色
    public int playerName { get; private set; }//玩家ID

    private int currentTileIndex;//当前棋子序号
    public int CurrentTileIndex
    {
        get => currentTileIndex;
        set => currentTileIndex = Mathf.Max(0,value);
    }

    private int totalSteps;//步数
    public int TotalSteps
    { 
        get => totalSteps;
        set 
        {//用方法封装更好
            totalSteps = value;
            remainingSteps = value;
            //OnDataChanged?.Invoke();//数据改变事件
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

    public BasePlayerController playerController;
    public List<TileBase> ownedTiles;



    public PlayerData(int id)
    {
        //初始化
        playerName = id;
        currentTileIndex = 0;
        totalSteps = 0;
        remainingSteps = 0;
        copper = 5;
        ownedTiles = new List<TileBase>();

    }

    public void AddCopper(int amount)
    { 
        copper += amount;
        OnDataChanged?.Invoke();//数据改变事件
    }
    public bool SubtractCopper(int amount)
    {
        if (copper>=amount)
        {//钱够
            copper -= amount;
            OnDataChanged?.Invoke();//数据改变事件
            return true;
        }
        return false;
    }
    public bool HasEnoughCopper(int amount)
    {//钱够不够
        return copper >= amount;
    }
    public int GetCurrentStep()
    {//当前步数
        return totalSteps-remainingSteps;
    }
    public void SetCharacter(CharacterData data)
    {
        characterData = data;
    }
}
