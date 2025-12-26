using System.Collections.Generic;
using Core.Systems;
using Game.Data.Config;
using Game.Logic.Map;
using UnityEngine;

namespace Game.Systems
{
    public class MapSystem : SystemBase ,IMapSystem
    {
        private const string configPath = "Configs/MapRuntime/MapConfig";
        private MapLogic mapLogic;

        // ========== SystemBase 继承实现 ==========
        protected override void OnInitialize()
        {
            //加载地图数据
            MapRuntimeConfig mapConfig = Resources.Load<MapRuntimeConfig>(configPath);
            if (mapConfig == null)
            {
                Debug.LogError($"MapRuntimeConfig 加载失败，路径: {configPath}");
                throw new System.Exception($"MapRuntimeConfig not found at path: {configPath}");
            }
            if (mapConfig.tiles == null || mapConfig.lands == null || mapConfig.buildings == null)
            {
                Debug.LogWarning("MapRuntimeConfig 包含空数据，使用空列表初始化");
            }
            mapLogic = new MapLogic(mapConfig);
            
            Debug.Log($"MapSystem 初始化完成: {mapLogic.GetTileCount()} Tiles, {mapLogic.GetLandCount()} Lands,{mapLogic.GetBuildingCount()} Buildings");
        }

        protected override void OnCleanup()
        {
            mapLogic?.Cleanup();
            mapLogic = null;
            Debug.Log("MapSystem 清理完成");
        }

        // ========== IMapSystem 接口实现 ==========

        public TileLogic GetTile(int index)
        {
            return mapLogic?.GetTile(index);
        }

        public LandLogic GetLand(int landId)
        {
            return mapLogic?.GetLand(landId);
        }
        
        public List<TileLogic> GetAllTiles()
        {
            return mapLogic?.GetAllTiles();
        }

        public List<LandLogic> GetAllLands()
        {
            return mapLogic?.GetAllLands();
        }
        //public List<LandLogic> GetAdjacentLands(int tileIndex)
        //public List<TileLogic> GetTilesAdjacentToLand(int landId)
        //public void LinkTileToLand(int tileIndex, int landId)
    }
}