using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//骰子管理
public class DiceManager : MonoBehaviour
{
    public static DiceManager instance;
    public static DiceManager Instance
    {
        get
        {
            if (instance==null)
            {
                instance = FindAnyObjectByType(typeof(DiceManager)) as DiceManager;
            }
            return instance;
        }
    }


    public DiceController[] diceArray; //骰子数组
    private int currentPlayerCount;

    private int rolledPlayerCount = 0; //已摇骰子的玩家数量

    //public event Action<int> OnDiceRolled;

    public void PerGame(int PlayerCount)
    {
        currentPlayerCount = PlayerCount;
        for (int i = 0; i < diceArray.Length; i++)
        {
            diceArray[i].gameObject.SetActive(i < PlayerCount);
            diceArray[i].SetId(i);
        }
    }
    public void StartAllDiceRolling()
    {
        rolledPlayerCount = 0;
        for (int i = 0; i < currentPlayerCount; i++)
        {
            diceArray[i].StartRollingDice();
        }
    }

    public void DiceResult(int id,int result)
    { 
        PlayerManager.Instance.GetPlayerData(id).TotalSteps = result; //设置玩家的骰子结果

        rolledPlayerCount++;
        if (rolledPlayerCount==PlayerManager.Instance.playerCount)
        {
            TurnManager.Instance.OverTurn();
        }
    }
}
