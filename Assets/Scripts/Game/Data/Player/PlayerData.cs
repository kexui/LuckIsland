namespace Game.Data.Player
{
    //用于初始化玩家数据
    public class PlayerData
    {
        public int playerId;
        public string playerName;
        public int startGold = 500;
        public int startTileIndex = 0;
        public string characterPrefabName; // 角色预制体名称
    }
}