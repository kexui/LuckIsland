using Core.Systems;

namespace Game.Systems
{
    public interface ITurnSystem
    {
        void StartTurnCycle();
        ITurnPhase GetCurrentPhase();
    }
}