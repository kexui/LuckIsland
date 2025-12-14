using System.Collections.Generic;
using Game.Enums;

namespace Game.Systems
{
    public class TurnSystem
    {
        public TurnState CurrentState { get; private set; } = TurnState.Start;
        private List<PlayerLogic> Players;

        private bool isTurnCycleRunning = false;

    
        public void StartTurnCycle()
        {
            isTurnCycleRunning = true;
        }
    }
}