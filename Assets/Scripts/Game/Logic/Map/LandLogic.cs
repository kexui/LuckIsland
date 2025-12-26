using Core.Enums;
using Core.Logic;
using Game.Data.Map;
using Game.Logic.Building;
using UnityEngine;

namespace Game.Logic.Map
{
    /// <summary>
    /// 地块逻辑
    /// - Land可以拥有Building
    /// - Land可以通过Building影响相邻的Tile
    /// </summary>
    public class LandLogic : LogicBase
    {
        private int tileId;
        private int buildingId;
        
        public int OwnerId { get; set; } = -1;
        public LandLogic(LandData data)
        {
            id = data.LandId;  // 设置基类的id
            tileId = data.TileId;
            buildingId = data.BuildingId;
            
            OwnerId = -1;
        }
        
        public bool HasBuilding => buildingId != -1;
        public bool IsOwned => OwnerId != -1;
        public bool CanBuild => !HasBuilding;
        
        // ========== 业务方法（自身逻辑）==========
        public bool CanBuildOn(int playerId)
        {
            // 如果已有建筑，不能建造
            if (HasBuilding)
                return false;
            
            // 如果无主，可以建造
            if (!IsOwned)
                return true;
            
            // 如果有主，只有所有者可以建造
            return OwnerId == playerId;
        }
        
        public bool IsOwnedBy(int playerId)
        {
            return IsOwned && OwnerId == playerId;
        }
        
        public void Own(int playerId)
        {
            if (playerId < 0)
            {
                Debug.LogWarning($"LandLogic {id} 占领失败: 无效的玩家ID {playerId}");
                return;
            }
            
            OwnerId = playerId;
        }
        
        // public BuildingType? GetBuildingType()
        // {
        //     
        // }
        
        // ========== 重写验证方法 ==========
        
        public override bool Validate()
        {
            if (!base.Validate()) return false;
            
            // 验证：如果有建筑，OwnerId应该有效
            if (HasBuilding && !IsOwned)
            {
                Debug.LogWarning($"LandLogic {id} 验证失败: 有建筑但无主");
                return false;
            }
            
            // 验证：OwnerId要么是-1（无主），要么>=0（有效玩家ID）
            if (OwnerId < -1)
            {
                Debug.LogWarning($"LandLogic {id} 验证失败: 无效的OwnerId {OwnerId}");
                return false;
            }
            
            return true;
        }
        
        // ========== 辅助方法 ==========
        
        /// <summary>
        /// 转换为字符串（用于调试）
        /// </summary>
        public override string ToString()
        {
            string ownerInfo = IsOwned ? $"Owner={OwnerId}" : "无主";
            return $"LandLogic(Id={id}, {ownerInfo})";
        }
    }
}