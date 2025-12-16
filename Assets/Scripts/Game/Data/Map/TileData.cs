using System.Collections.Generic;
using Core.Grid;
using Game.View.Map;
using UnityEngine;

namespace Game.Data.Map
{
    [System.Serializable]
    public class TileData
    {
        public int TileIndex;//Id
        public int FrontIndex;//前方Tile的ID
        public int BackIndex;//后方Tile的ID
        public int[] AdjacentLandIds;//相邻Land的ID
        public GridPos Pos;//位置

        public TileData()
        {
            
        }
    }
}
