using System.Collections.Generic;
using Core.Logic;

namespace Game.Logic.Map
{
    public class TileLogic : LogicBase
    {
        // ========== 数据字段 ==========
        private int index;
        public int Index 
        { 
            get => index;
            set
            {
                index = value;
                id = value;
            }
        }
        public List<int> adjacentLandIds;  // 相邻Land的ID列表
        
        public TileLogic(int index) 
        {
            id = index;
            adjacentLandIds = new List<int>();
        }
        
        // ========== 基本方法 ==========
        public void AddAdjacentLand(int landId)
        {
            adjacentLandIds.Add(landId);
        }

        public bool HasAdjacentLand()
        {
            return adjacentLandIds.Count > 0;
        }

        public int GetAdjacentLandCount()
        {
            return adjacentLandIds.Count;
        }
    }
}