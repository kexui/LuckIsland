using Core.Systems;
using Game.Enums;
using UnityEngine;

namespace Game.Systems.Turn
{
    public class RollDicePhase : ITurnPhase
    {
        public TurnPhase Phase { get; } = TurnPhase.RollDice;
        public int Time { get; } = 6;
        private bool hasRolled = false;
        public void Enter()
        {
            Debug.Log("Entering RollDicePhase");
            hasRolled = false;
            
            //显示骰子和按钮
        }

        public void Exit()
        {
            hasRolled = false;
        }
    }
}