using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnStage
{// 游戏回合阶段
    StartTurn,
    Wait,
    RollDice,
    Move,
    TriggerTileEvent,
    PlayerTurns,
    EndTurn
}

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    public TurnStage CurrentStage{ get; private set; }
    public int currentPlayerIndex { get; private set; }
    private BasePlayerController currentPlayerController;

    public static event Action<TurnStage> OnTurnStageChanged;
    public static event Action<int> OnPlayerChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerTurn()
    {
        currentPlayerIndex = 0; //从第一个玩家开始
        //currentPlayerController = PlayerManager.Instance.allPlayerDatas[currentPlayerIndex].playerController;
        //OnPlayerChanged?.Invoke(currentPlayerIndex);

        SetTurn(TurnStage.StartTurn);
        StartCoroutine(RunGameLoop());
    }

    private IEnumerator RunGameLoop()
    {
        while (true)
        {
            switch (CurrentStage)
            {
                case TurnStage.StartTurn:
                    OnStartTurn();

                    yield return new WaitForSeconds(1f);
                    SetTurn(TurnStage.Wait);
                    break;
                case TurnStage.Wait:
                    OnWait();

                    SetTurn(TurnStage.RollDice);
                    break;
                case TurnStage.RollDice:
                    OnRollDice();

                    SetTurn(TurnStage.Move);
                    break;
                case TurnStage.Move:
                    OnMove();

                    SetTurn(TurnStage.TriggerTileEvent);
                    break;
                case TurnStage.TriggerTileEvent:
                    OnTriggerTileEvent();

                    SetTurn(TurnStage.PlayerTurns);
                    break;
                case TurnStage.PlayerTurns:
                    yield return HandlePlayerTurns();

                    SetTurn(TurnStage.EndTurn);
                    break;
                case TurnStage.EndTurn:

                    SetTurn(TurnStage.StartTurn);
                    break;
                default:
                    Debug.LogError("Turn轮换出错");
                    break;
            }
        }
    }
    void OnStartTurn()
    { 
        
    }
    void OnWait()
    { }
    void OnRollDice()
    {
        BasePlayerController player;
        for (int i = 0; i < PlayerManager.Instance.playerCount; i++)
        {
            player = PlayerManager.Instance.GetPlayerData(i).playerController;
            StartCoroutine(player.RollDice());
        }
    }
    void OnMove()
    {
        BasePlayerController player;
        for (int i = 0; i < PlayerManager.Instance.playerCount; i++)
        {
            player = PlayerManager.Instance.GetPlayerData(i).playerController;
            StartCoroutine(player.MoveCoroutine());
        }
    }
    void OnTriggerTileEvent()
    { 
        
    }
    IEnumerator HandlePlayerTurns()
    {//用方法可能更好
        currentPlayerIndex = 0;
        currentPlayerController = PlayerManager.Instance.GetPlayerData(currentPlayerIndex).playerController;

        while (currentPlayerIndex<PlayerManager.Instance.playerCount)
        {
            yield return currentPlayerController.DoTurn();
            NextPlayer();
        }
    }
    void NextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % PlayerManager.Instance.playerCount;
        currentPlayerController = PlayerManager.Instance.allPlayerDatas[currentPlayerIndex].playerController;
        OnTurnStageChanged?.Invoke(CurrentStage);
    }
    void SetTurn(TurnStage nextTurn)
    { 
        CurrentStage = nextTurn;
        OnTurnStageChanged?.Invoke(CurrentStage);
    }
}
