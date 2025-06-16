using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {

    }
    public void PlayerTurn()
    {
        currentPlayerIndex = 0; //从第一个玩家开始
        currentPlayerController = PlayerManager.Instance.allPlayerDatas[currentPlayerIndex].playerController;

        CurrentStage = TurnStage.StartTurn;
        OnTurnStageChanged?.Invoke(CurrentStage);
        OnPlayerChanged?.Invoke(currentPlayerIndex);
        StartCoroutine(HandlePlayerTurn());
    }

    private IEnumerator HandlePlayerTurn()
    {
        while (true)
        {
            switch (CurrentStage)
            {
                case TurnStage.StartTurn:
                    currentPlayerController.StartTurn();
                    CurrentStage = TurnStage.Wait;
                    OnTurnStageChanged?.Invoke(CurrentStage);
                    break;
                case TurnStage.Wait:
                    yield return currentPlayerController.Wait();
                    CurrentStage = TurnStage.RollDice;
                    OnTurnStageChanged?.Invoke(CurrentStage);
                    break;
                case TurnStage.RollDice:
                    yield return currentPlayerController.RollDice();
                    CurrentStage = TurnStage.Move;
                    OnTurnStageChanged?.Invoke(CurrentStage);
                    break;
                case TurnStage.Move:
                    yield return currentPlayerController.MoveCoroutine();
                    CurrentStage = TurnStage.TriggerTileEvent;
                    OnTurnStageChanged?.Invoke(CurrentStage);
                    break;
                case TurnStage.TriggerTileEvent:
                    yield return currentPlayerController.TriggerTileEvent();
                    CurrentStage = TurnStage.EndTurn;
                    OnTurnStageChanged?.Invoke(CurrentStage);
                    break;
                case TurnStage.EndTurn:
                    currentPlayerController.EndTurn();
                    NextPlayer();
                    break;
                default:
                    Debug.LogError("Turn轮换出错");
                    break;
            }
        }
    }
    void NextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % PlayerManager.Instance.GetPlayersCount();
        currentPlayerController = PlayerManager.Instance.allPlayerDatas[currentPlayerIndex].playerController;

        OnPlayerChanged?.Invoke(currentPlayerIndex);
        CurrentStage = TurnStage.StartTurn;
        OnTurnStageChanged?.Invoke(CurrentStage);
    }

}
