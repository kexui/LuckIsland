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

    [SerializeField] private Dice dice;
    private int riceResult;

    public event Action<int> OnDiceRolled;

    public void RollDice()
    {
        riceResult = UnityEngine.Random.Range(1, 7);//左闭右开  【1，7）
        OnDiceRolled?.Invoke(riceResult);
        UIManager.Instance.UpdatePlayerDataUI();//更新UI
    }
    public int GetDiceResult()
    {
        return riceResult;
    }
}
