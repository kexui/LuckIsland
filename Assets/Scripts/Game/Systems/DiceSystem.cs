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
        RequestRollDice(evt.PlayerID);
    }

    // ========== IDiceSystem ==========

    public void RequestRollDice(int playerID)
    {
        if (!CanRollDice())
        {
            Debug.LogWarning($"玩家 {playerID} 当前无法投骰子");
            return;
        }

        if (HasPlayerRolled(playerID))
        {
            Debug.Log($"DiceSystem:玩家{playerID}已经投过了");
            return;
        }
        
        int result = random.Next(1, 7);
        playerRolledState[playerID] = true;
        Debug.Log($"RequestRollDice, ID:{playerID}, Result:{result}");
        
        if (EventBus.Instance != null)
        {
            var diceEvent = new DiceRolledEvent()
            {
                Result = result,
                PlayerId = playerID
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
