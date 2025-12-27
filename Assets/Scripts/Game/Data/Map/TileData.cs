using System;
using Core.Grid;

namespace Game.Data.Map
{
    [Serializable]
    public class TileData
    {
        public int TileId;//Id
        
        public int FrontIndex;//前方Tile的ID
        public int BackIndex;//后方Tile的ID
        public int[] AdjacentLandIds;//相邻Land的ID
        public GridPos Pos;//网格位置
        
        public TileData()
        {
            TileId = -1;
            FrontIndex = -1;
            BackIndex = -1;
            AdjacentLandIds = null;
            Pos = new GridPos(0, 0, 0); // 初始化默认位置
        }
        
        public TileData(TileData data)
        {
            TileId = data.TileId;
            FrontIndex = data.FrontIndex;
            BackIndex = data.BackIndex;
            Pos = data.Pos;
            AdjacentLandIds = data.AdjacentLandIds != null
                ? (int[])data.AdjacentLandIds.Clone()
                : null;
        }
    }
}