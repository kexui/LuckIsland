using System.Collections.Generic;
using Core.Logic;
using Game.Data.Map;
using UnityEngine;

namespace Game.Logic.Map
{
    public class TileLogic : LogicBase
    {
        // ========== 数据字段 ==========
        public int FrontIndex { get; } //前方Tile的ID
        public int BackIndex { get; }//后方Tile的ID
        private readonly int[] adjacentLandIds;//相邻Land的ID
        
        public TileLogic(TileData data)
        {
            if (data == null)
            {
                Debug.LogError("TileLogic: TileData 不能为 null");
                id = -1;
                FrontIndex = -1;
                BackIndex = -1;
                adjacentLandIds = new int[2];
                return;
            }

            id = data.TileId;
            FrontIndex = data.FrontIndex;
            BackIndex = data.BackIndex;
            if (data.AdjacentLandIds != null && data.AdjacentLandIds.Length > 0)
            {
                adjacentLandIds = new int[data.AdjacentLandIds.Length];
                System.Array.Copy(data.AdjacentLandIds, adjacentLandIds, data.AdjacentLandIds.Length);
            }
            else
            {
                adjacentLandIds = new int[0];
            }
        }
        
        // ========== 基本方法 ==========
        public bool HasAdjacentLand()
        {
            return adjacentLandIds.Length > 0;
        }

        public int GetAdjacentLandCount()
        {
            return adjacentLandIds.Length;
        }
        public IReadOnlyList<int> GetAdjacentLandIds()
        {
            return adjacentLandIds;
        }
        public bool IsAdjacentToLand(int landId)
        {
            return System.Array.IndexOf(adjacentLandIds, landId) >= 0;
        }
    }
}