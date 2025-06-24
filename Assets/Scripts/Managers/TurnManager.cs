using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TurnStage
{// 游戏回合阶段
    StartTurn,
    Wait,
    RollDice,
    Move,
    TriggerTileEvent,
    PlayerTurn,
    EndTurn
}

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    public TurnStage CurrentStage{ get; private set; }
    public int currentPlayerIndex { get; private set; }
    private BasePlayerController currentPlayerController;

    public bool OverTurn { get; private set; }

    public static event Action<TurnStage> OnTurnStageChanged;
    public static event Action<int> OnPlayerChanged;

    private Dictionary<TurnStage, float> Durations = new() {
        {TurnStage.StartTurn,1f},
        {TurnStage.Wait ,1f},
        {TurnStage.RollDice ,10f},
        {TurnStage.Move ,4f},
        {TurnStage.TriggerTileEvent ,3f},
        {TurnStage.PlayerTurn ,10f},
        {TurnStage.EndTurn ,1f}
    };

    private void Awake()
    {
        Instance = this;
        currentPlayerIndex = -1;
        OverTurn = false;
    }

    public void PlayerTurn()
    {

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
                    yield return new WaitForSeconds(1f);
                    OnRollDice();

                    yield return WaitTurnOver(8f);
                    SetTurn(TurnStage.Move);
                    break;
                case TurnStage.Move:
                    OnMove();
                    yield return WaitTurnOver(3f);

                    SetTurn(TurnStage.TriggerTileEvent);
                    break;
                case TurnStage.TriggerTileEvent:
                    OnTriggerTileEvent();

                    yield return WaitTurnOver(3f);
                    SetTurn(TurnStage.PlayerTurn);
                    break;
                case TurnStage.PlayerTurn:
                    yield return HandlePlayerTurns();

                    SetTurn(TurnStage.EndTurn);
                    break;
                case TurnStage.EndTurn:
                    OnEndTurn();

                    yield return WaitTurnOver(1f);
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
    {//触发地块事件不用经过玩家？
        BasePlayerController player;
        for (int i = 0; i < PlayerManager.Instance.playerCount; i++)
        {
            player = PlayerManager.Instance.GetPlayerData(i).playerController;
            StartCoroutine(player.TriggerTileEvent());
        }
    }
    IEnumerator HandlePlayerTurns()
    {//用方法可能更好
        currentPlayerIndex = 0;
        currentPlayerController = PlayerManager.Instance.GetPlayerData(currentPlayerIndex).playerController;

        while (currentPlayerIndex<PlayerManager.Instance.playerCount)
        {
            OnTurnStageChanged?.Invoke(CurrentStage);
            yield return currentPlayerController.DoTurn();
            yield return new WaitForSeconds(3f);
            currentPlayerIndex++;
        }
        currentPlayerIndex = -1;
    }
    void OnEndTurn()
    { 
    
    }
    IEnumerator WaitTurnOver(float turnTime)
    {
        float timer = 0;
        while (timer<turnTime)
        {
            if (OverTurn) break;
            timer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
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
        OverTurn = false;
        OnTurnStageChanged?.Invoke(CurrentStage);
    }
    public void SetOverTurn()
    {
        OverTurn = true;
    }
    public float GetTurnTime()
    {
        return Durations[CurrentStage];
    }
}
