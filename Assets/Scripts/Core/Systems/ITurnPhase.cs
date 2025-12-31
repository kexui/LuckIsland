using Game.Enums;

namespace Core.Systems
{
    public interface ITurnPhase
    {
        TurnPhase Phase { get; }
        int Time { get; }
        void Enter();
        void Exit();
    }
}