using System.Collections.Generic;
using Core.Systems;
using Game.Systems.Turn;

namespace Game.Data.System
{
    public class TurnPhaseData
    {
        public List<ITurnPhase> AllTurnPhases;

        public TurnPhaseData()
        {
            AllTurnPhases = new List<ITurnPhase>();
            AllTurnPhases.Add(new StartPhase());
            AllTurnPhases.Add(new WaitPhase());
            AllTurnPhases.Add(new RollDicePhase());
            AllTurnPhases.Add(new MovePhase());
            AllTurnPhases.Add(new PlayerPhase());
            AllTurnPhases.Add(new PlayerPhase());
        }
    }
}