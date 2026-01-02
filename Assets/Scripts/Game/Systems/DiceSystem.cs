using System.Collections.Generic;
using Core.Events;
using Core.Systems;
using Game.Events;
using Game.Managers;
using Game.Systems.Turn;
using UnityEngine;

public class DiceSystem : SystemBase,IDiceSystem
{
    private System.Random random;
    private IPlayerSystem playerSystem;
    
    private Dictionary<int, bool> playerRolledState = new Dictionary<int, bool>();
    private Dictionary<int, int> playerRollResults = new();
    
    
    // ========== SystemBase ==========
    protected override void OnInitialize()
    {
        random = new System.Random();
    }

    protected override void OnEnable()
    {
        SubscribeEvent<UIRollDiceRequest>(OnRequestRollDice);
    }

    protected override void OnCleanup()
    {
        random = null;
        playerRolledState.Clear();
    }

    // ========== Events ==========
    
    public void OnRequestRollDice(UIRollDiceRequest evt)
    {
        RequestRollDice(evt.PlayerId);
    }

    // ========== IDiceSystem ==========

    public void RequestRollDice(int playerId)
    {
        if (!CanRollDice())
        {
            Debug.LogWarning($"玩家 {playerId} 当前无法投骰子");
            return;
        }

        if (HasPlayerRolled(playerId))
        {
            Debug.Log($"DiceSystem:玩家{playerId}已经投过了");
            return;
        }
        
        int result = random.Next(1, 7);
        playerRolledState[playerId] = true;
        playerRollResults.Add(playerId, result);
        Debug.Log($"RequestRollDice, ID:{playerId}, Result:{result}");
        
        if (EventBus.Instance != null)
        {
            var diceEvent = new DiceRolledEvent()
            {
                Result = result,
                PlayerId = playerId
            };
            EventBus.Instance.Publish(diceEvent);
        }
    }
    
    private bool CanRollDice()
    {
        if (GameManager.Instance?.TurnSystem != null)
        {
            var currentPhase = GameManager.Instance.TurnSystem.GetCurrentPhase();
            if (currentPhase is RollDicePhase)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasPlayerRolled(int playerId)
    {
        if (!playerRolledState.TryGetValue(playerId, out bool result))
        {
            Debug.LogError($"DiceSystem: 玩家 {playerId} 不存在");
        }
        return result;
    }
}
