using Core.Events;
using Core.Systems;
using Game.Events;
using Game.Managers;
using Game.Systems.Turn;
using UnityEngine;

public class DiceSystem : SystemBase,IDiceSystem
{
    private System.Random random;
    
    // ========== SystemBase ==========
    protected override void OnInitialize()
    {
        random = new System.Random();
    }

    protected override void OnCleanup()
    {
        random = null;
    }

    public int RequestRollDice(int playerId)
    {
        
        if (!CanRollDice())
        {
            Debug.LogWarning($"玩家 {playerId} 当前无法投骰子");
            return 0;
        }
        
        int result = random.Next(1, 7);
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
        return result;
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
}
