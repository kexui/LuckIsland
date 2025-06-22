using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ӹ���
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

    float rollDuration = 5f; // ��������ʱ��
    float flipTime = 0.07f; // ÿ��ת����ʱ����

    public DiceController[] diceArray; //��������
    private int currentPlayerCount;

    private int riceResult;

    public event Action<int> OnDiceRolled;

    public void PerGame(int PlayerCount)
    {
        currentPlayerCount = PlayerCount;
        for (int i = 0; i < diceArray.Length; i++)
        {
            diceArray[i].gameObject.SetActive(i < PlayerCount);
        }
    }
    public void StartAllDiceRolling()
    {
        for (int i = 0; i < currentPlayerCount; i++)
        {
            diceArray[i].StartRollingDice();
        }
    }




    public void RollDice()
    {
        riceResult = UnityEngine.Random.Range(1, 7);//����ҿ�  ��1��7��
        Debug.Log("Dice Rolled: " + riceResult);
    }
    public int GetDiceResult()
    {
        return riceResult;
    }
}
