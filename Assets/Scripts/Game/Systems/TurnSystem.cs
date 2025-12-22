using System.Collections.Generic;
using Core.Systems;
using Game.Enums;
using Core.Systems;

namespace Game.Systems
{
    public class TurnSystem:SystemBase,ITurnSystem
    {
        public TurnState CurrentState { get; private set; } = TurnState.Start;
        private bool isTurnCycleRunning = false;
        private int currentPlayerIndex;
        private List<int> playerOrder = new();

        protected override void OnInitialize()
        {
            currentPlayerIndex = -1;
        }

        protected override void OnCleanup()
        {
            currentPlayerIndex = -1;
            playerOrder.Clear();
        }

        public void StartTurnCycle()
        {
            isTurnCycleRunning = true;
        }
    }
}