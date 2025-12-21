using System;

namespace Game.Data.Map
{
    [Serializable]
    public class TileData
    {
        public int TileId;//Id
        
        public int FrontIndex;//前方Tile的ID
        public int BackIndex;//后方Tile的ID
        public int[] AdjacentLandIds;//相邻Land的ID

        public TileData()
        {
            TileId = -1;
            FrontIndex = -1;
            BackIndex = -1;
        }
        
        public TileData(TileData data)
        {
            TileId = data.TileId;
            FrontIndex = data.FrontIndex;
            BackIndex = data.BackIndex;
            AdjacentLandIds = data.AdjacentLandIds != null
                ? (int[])data.AdjacentLandIds.Clone()
                : null;
        }
    }
}