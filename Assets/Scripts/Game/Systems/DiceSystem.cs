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

    protected override void OnEnable()
    {
        SubscribeEvent<UIRollDiceRequest>(OnRequestRollDice);
    }

    protected override void OnCleanup()
    {
        random = null;
    }

    // ========== Events ==========
    
    public void OnRequestRollDice(UIRollDiceRequest evt)
    {
        RequestRollDice(evt.PlayerID);
    }

    // ========== IDiceSystem ==========
    
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

    public void RequestRollDice(int playerID)
    {
        if (!CanRollDice())
        {
            Debug.LogWarning($"玩家 {playerID} 当前无法投骰子");
            return;
        }
        
        int result = random.Next(1, 7);
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
}
