using System.Collections.Generic;
using System.Linq;
using Game.Data.Config;
using Game.Data.Map;
using Game.Logic.Building;
using UnityEngine;

namespace Game.Logic.Map
{
    public class MapLogic
    {
        private List<TileData> tiles;
        private Dictionary<int, LandData> landDic;
        private List<LandData> lands;
        private Dictionary<int, BuildingData> buildingDic;
        private List<BuildingData> buildings;

        public MapLogic(MapData mapData)
        {
            tiles = new();
            landDic = new();
            lands = new();
            buildingDic = new();
            buildings = new();
            
            tiles = mapData.tiles;
            foreach (var land in mapData.lands)
            {
                lands.Add(land);
                landDic.Add(land.LandId, land);
            }

            foreach (var building in mapData.buildings)
            {
                buildings.Add(building);
                buildingDic.Add(building.BuildingId, building);
            }
        }

        public MapData(MapRuntimeConfig mapConfig)
        {
            tiles = new();
            landDic = new();
            lands = new();
            buildingDic = new();
            buildings = new();

            tiles = mapConfig.tiles;
            
        }

        public void Initialize()
        {
            
        }
        
        public void Cleanup()
        {
            tiles?.Clear();
            lands?.Clear();
        }

        // ========== Tile管理 ==========
        
        //添加Tile
        public void AddTile(TileLogic tile)
        {
            if (tile == null)
            {
                Debug.LogWarning("MapLogic: 尝试添加null的Tile");
                return;
            }
            
            // 检查是否已存在
            if (GetTile(tile.Index) != null)
            {
                Debug.LogWarning($"MapLogic: Tile {tile.Index} 已存在");
                return;
            }
            
            tiles.Add(tile);
        }

        //按index获取Tile
        public TileLogic GetTile(int index)
        {
            if (index < 0 || index >= tiles.Count)
            {
                Debug.Log("超出查找范围");
                return null;
            }

            return tiles.FirstOrDefault(t => t.GetId() == index);
        }
        
        //获取所有Tile
        public List<TileLogic> GetAllTiles()
        {
            return new List<TileLogic>(tiles);
        }
        
        //获取Tile数量
        public int GetTileCount()
        {
            return tiles.Count;
        }
        
        // ========== Land管理 ==========

        public void AddLand(LandLogic land)
        {
            if (land == null)
            {
                Debug.LogWarning("MapLogic: 尝试添加null的land");
            }

            lands.Add(land.GetId(), land);
        }

        public LandLogic GetLand(int landId)
        {
            if (lands.TryGetValue(landId, out LandLogic land))
            {
                return land;
            }
            return null;
        }

        public List<LandLogic> GetAllLands()
        {
            return new List<LandLogic>(lands.Values);
        }

        public int GetLandCount()
        {
            return lands.Count;
        }
        
        // ========== 关联关系管理 ==========
        
        //建立Tile和Land的关联关系
        public void LinkTileToLand(int tileIndex, int landId)
        {
            var tile = GetTile(tileIndex);
            if (tile == null)
            {
                Debug.LogWarning($"MapLogic: 无法关联，Tile {tileIndex} 不存在");
                return;
            }
            
            var land = GetLand(landId);
            if (land == null)
            {
                Debug.LogWarning($"MapLogic: 无法关联，Land {landId} 不存在");
                return;
            }
            
            tile.AddAdjacentLand(landId);
        }
    }
}