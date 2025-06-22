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
        currentPlayerIndex = -1;
    }

    public void PlayerTurn()
    {
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

                    yield return new WaitForSeconds(1f);
                    SetTurn(TurnStage.RollDice);
                    break;
                case TurnStage.RollDice:
                    OnRollDice();

                    yield return new WaitForSeconds(5f);
                    SetTurn(TurnStage.Move);
                    break;
                case TurnStage.Move:
                    OnMove();

                    yield return new WaitForSeconds(3f);
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
                    OnEndTurn();

                    yield return WaitTurn(1);
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
        DiceManager.Instance.StartAllDiceRolling();
        BasePlayerController player;
        for (int i = 0; i < PlayerManager.Instance.playerCount; i++)
        {
            Debug.Log("RollDice：i=" + i);
            player = PlayerManager.Instance.GetPlayerData(i).playerController;
            StartCoroutine(player.RollDice());
        }
    }
    void OnMove()
    {
        BasePlayerController player;
        for (int i = 0; i < PlayerManager.Instance.playerCount; i++)
        {
            Debug.Log("Move：i=" + i);
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
            OnTurnStageChanged?.Invoke(CurrentStage);
            yield return currentPlayerController.DoTurn();
            yield return new WaitForSeconds(1f);
            currentPlayerIndex++;
        }
        currentPlayerIndex = -1;
    }
    void OnEndTurn()
    { 
    
    }
    IEnumerator WaitTurn(int time)
    { 
        yield return new WaitForSeconds(time);
    }
    void NextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % PlayerManager.Instance.playerCount;
        currentPlayerController = PlayerManager.Instance.allPlayerDatas[currentPlayerIndex].playerController;
        OnTurnStageChanged?.Invoke(CurrentStage);
        OnPlayerChanged?.Invoke(currentPlayerIndex);
    }
    void SetTurn(TurnStage nextTurn)
    { 
        CurrentStage = nextTurn;
        OnTurnStageChanged?.Invoke(CurrentStage);
    }
}
