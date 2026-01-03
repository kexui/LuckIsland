using System.Collections.Generic;
using Game.Data.Player;

namespace Game.Systems
{
    public interface IPlayerSystem
    {
        void CreatePlayer(PlayerData data);
        void CreatePlayer(int playerId, string playerName, int startGold, int startTileIndex,
            string characterName);

        void LoadPlayers(List<PlayerData> playerDataList);
        PlayerLogic GetPlayer(int playerId);
        List<PlayerLogic> GetAllPlayers();
    }
}