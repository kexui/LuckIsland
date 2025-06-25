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

    private Dictionary<TurnStage, float> TurnDurations = new() {
        {TurnStage.StartTurn,1f},
        {TurnStage.Wait ,1f},
        {TurnStage.RollDice ,10f},
        {TurnStage.Move ,4f},
        {TurnStage.TriggerTileEvent ,3f},
        {TurnStage.PlayerTurn ,2f},
        {TurnStage.EndTurn ,1f}
    };

    private void Awake()
    {
        Instance = this;
        currentPlayerIndex = -1;
        OverTurn = false;
    }

    public void StartGame()
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

                    yield return WaitTurnOver();
                    SetTurn(TurnStage.Wait);
                    break;
                case TurnStage.Wait:
                    OnWait();

                    yield return WaitTurnOver();
                    SetTurn(TurnStage.RollDice);
                    break;
                case TurnStage.RollDice:
                    OnRollDice();

                    yield return WaitTurnOver();
                    SetTurn(TurnStage.Move);
                    break;
                case TurnStage.Move:
                    OnMove();

                    yield return WaitTurnOver();
                    SetTurn(TurnStage.TriggerTileEvent);
                    break;
                case TurnStage.TriggerTileEvent:
                    OnTriggerTileEvent();

                    yield return WaitTurnOver();
                    SetTurn(TurnStage.PlayerTurn);
                    break;
                case TurnStage.PlayerTurn:
                    yield return HandlePlayerTurns();

                    SetTurn(TurnStage.EndTurn);
                    break;
                case TurnStage.EndTurn:
                    OnEndTurn();

                    yield return WaitTurnOver();
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
        CardManager.Instance.DealCardsToAllPlayers(PlayerManager.Instance.allPlayerDatas);
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
        {//重复玩家回合
            OnTurnStageChanged?.Invoke(CurrentStage);//玩家回合跟新
            //yield return currentPlayerController.DoTurn();
            TimerUtility.Instance.StartTimer(GetTurnTime(), () => { StartCoroutine(currentPlayerController.DoTurn()); },()=>OverTurn);
            currentPlayerIndex++;
            yield return WaitTurnOver();
        }
        currentPlayerIndex = -1;
    }
    void OnEndTurn()
    { 
    
    }
    IEnumerator WaitTurnOver()
    {//等待当前回合结束
        float timer = 0f;
        while (timer < GetTurnTime() - 1)
        {
            if (OverTurn) break;
            timer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }
    IEnumerator WaitTurnOver(float Timer)
    {
        float timer = 0;
        while (timer<Timer)
        {
            if (OverTurn) break;
            timer += Time.deltaTime;
            yield return null;
        }
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
        return TurnDurations[CurrentStage];
    }
}
