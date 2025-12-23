namespace Core.Systems
{
    public interface ITurnPhase
    {
        int time { get; }
        void Enter();
        void Exit();
    }
}