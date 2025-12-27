using System.Collections.Generic;

namespace Core.Systems
{
    public interface IPlayerSystem
    {
        PlayerLogic CreatePlayer(int playerId, string playerName, int startGold, int startTileIndex,
            string characterName);

        void LoadPlayer(int playerId, string playerName, int startGold, int startTileIndex, string characterName);
    }
}