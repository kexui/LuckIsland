using Core.Systems;
using Game.Enums;
using UnityEngine;

namespace Game.Systems.Turn
{
    public class RollDicePhase : ITurnPhase
    {
        public TurnPhase Phase { get; } = TurnPhase.RollDice;
        public int Time { get; } = 10;
        private bool hasRolled = false;
        public void Enter()
        {
            Debug.Log("Entering StartPhase");
            hasRolled = false;
            
            //显示骰子和按钮
        }

        public void Exit()
        {
            hasRolled = false;
        }
    }
}