using Game.Logic.Map;

namespace Game.Logic.Building
{
    public interface IBuildingEffect
    {
        void OnPlayerArrived( int playerId);
    }
}