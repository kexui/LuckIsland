using Core.Systems;

namespace Game.Events
{
    public class TurnTimerEvent
    {
        public float RemainingTime { get; set; }
        public float TotalTime { get; set; }
        public ITurnPhase CurrentPhase { get; set; }

        public TurnTimerEvent(float remainingTime, float totalTime, ITurnPhase currentPhase)
        {
            RemainingTime = remainingTime;
            TotalTime = totalTime;
            CurrentPhase = currentPhase;
        }
    }

    public class TurnPhaseChangedEvent
    {
        public ITurnPhase CurrentPhase { get; set; }
        public ITurnPhase PreviousPhase { get; set; }

        public TurnPhaseChangedEvent(ITurnPhase currentPhase, ITurnPhase previousPhase)
        {
            CurrentPhase = currentPhase;
            PreviousPhase = previousPhase;
        }
    }

    public class TurnPhaseTimeUpEvent
    {
        public ITurnPhase Phase { get; set; }

        public TurnPhaseTimeUpEvent(ITurnPhase phase)
        {
            Phase = phase;
        }
    }
}