using System.Collections.Generic;
using Core.Systems;
using Game.Data.System;

namespace Game.Systems.Turn
{
    public class TurnScheduler
    {
        private List<ITurnPhase> allPhases;
        private ITurnPhase currentPhase;
        public ITurnPhase CurrentPhase =>  currentPhase;
        
        public  TurnScheduler(TurnPhaseData data)
        {
            allPhases = new List<ITurnPhase>(data.AllTurnPhases);
        }
        
        public void ChangePhase(ITurnPhase next)
        {
            currentPhase?.Exit();
            currentPhase = next;
            currentPhase?.Enter();
        }
        
    }
}