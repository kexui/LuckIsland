using System.Collections.Generic;
using Core.Systems;
using Game.Logic.Map;
using Game.View.Map;
using UnityEngine;

namespace Game.Systems
{
    public class MapSystem : SystemBase ,IMapSystem
    {
        private MapLogic mapLogic;
        
        public MapSystem()
        {
            // 无依赖，直接创建
        }

        // ========== SystemBase 继承实现 ==========
        protected override void OnInitialize()
        {
            mapLogic = new MapLogic();
            mapLogic.Initialize();
            CollectViewFromScene();
            
            Debug.Log($"MapSystem 初始化完成: {mapLogic.GetTileCount()} Tiles, {mapLogic.GetLandCount()} Lands");
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

        public void AddTile(TileLogic tile)
        {
            mapLogic?.AddTile(tile);
        }

        public void AddLand(LandLogic land)
        {
            mapLogic?.AddLand(land);
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
        
        
        // ========== 初始化Demo地图（测试用）==========
        
        //收集场景中的View
        private void CollectViewFromScene()
        {
            // 收集所有LandView（先收集Land，因为Tile需要关联Land）
            var landViews = Object.FindObjectsOfType<LandView>();
            foreach (var view in landViews)
            {
                var land = new LandLogic(view.landId);
                mapLogic.AddLand(land);
                Debug.Log($"收集Land: {view.landId}");
            }
            
            var tileViews = Object.FindObjectsOfType<TileView>();
            foreach (var view in tileViews)
            {
                var tile = new TileLogic(view.tileIndex);
                mapLogic.AddTile(tile);
                Debug.Log($"收集Land: {view.tileIndex}");
            }
        }
        
        //确定连接
        private void LinkAllTileAndLand()
        {
            foreach (var VARIABLE in GetAllTiles())
            {
                
            }
        }
    }
}