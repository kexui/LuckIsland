using System;

namespace Game.Data.Config
{
    [Serializable]
    public class TileConfig
    {
        public int TileId;//Id
        
        public int FrontIndex;//前方Tile的ID
        public int BackIndex;//后方Tile的ID
        public int[] AdjacentLandIds;//相邻Land的ID

        public TileConfig()
        {
            TileId = -1;
            FrontIndex = -1;
            BackIndex = -1;
        }
        
        public TileConfig(TileConfig config)
        {
            TileId = config.TileId;
            FrontIndex = config.FrontIndex;
            BackIndex = config.BackIndex;
            AdjacentLandIds = config.AdjacentLandIds != null
                ? (int[])config.AdjacentLandIds.Clone()
                : null;
        }
    }
}