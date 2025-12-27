using Game.View.Player;

namespace Core.Systems
{
    public interface IPlayerViewSystem
    {
        void CreatePlayerView(PlayerLogic playerLogic);
        PlayerView GetPlayerView(int playerId);
    }
}