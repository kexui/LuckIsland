using System.Collections.Generic;
using Game.Data.Config;
using Game.Data.Map;
using Game.Logic.Map.Building;

namespace Game.Logic.Map
{
    public class MapLogic
    {
        private List<TileLogic> tiles = new();
        private Dictionary<int, LandLogic> landDic = new();
        private List<LandLogic> lands= new ();
        private List<BuildingLogic> buildings = new();

        public MapLogic(MapData mapData)
        {
            foreach (var tile in mapData.tiles)
            {
                tiles.Add(new TileLogic(tile));
            }
            
            foreach (var land in mapData.lands)
            {
                LandLogic newLand = new LandLogic(land);
                lands.Add(newLand);
                landDic.Add(newLand.GetId(), newLand);
            }

            foreach (var building in mapData.buildings)
            {
                buildings.Add(new BuildingLogic(building));
            }
        }

        public MapLogic(MapRuntimeConfig mapConfig)
        {
            foreach (var tile in mapConfig.tiles)
            {
                tiles.Add(new TileLogic(tile));
            }
            
            foreach (var land in mapConfig.lands)
            {
                LandLogic newLand = new LandLogic(land);
                lands.Add(newLand);
                landDic.Add(newLand.GetId(), newLand);
            }

            foreach (var building in mapConfig.buildings)
            {
                buildings.Add(new BuildingLogic(building));
            }
        }

        public void Initialize()
        {
            
        }
        
        public void Cleanup()
        {
            tiles?.Clear();
            lands?.Clear();
            buildings?.Clear();
        }

        // ========== Tile管理 ==========

        //按index获取Tile
        public TileLogic GetTile(int id)
        {
            return tiles[id];
        }

        public int GetNextTile(int id)
        {
            //暂时
            return (id + 1) % tiles.Count;
        }

        //获取所有Tile
        public List<TileLogic> GetAllTiles()
        {
            return tiles;
        }
        
        //获取Tile数量
        public int GetTileCount()
        {
            return tiles.Count;
        }
        
        // ========== Land管理 ==========
        
        public LandLogic GetLand(int Id)
        {
            return landDic[Id];
        }

        public List<LandLogic> GetAllLands()
        {
            return lands;
        }

        public int GetLandCount()
        {
            return lands.Count;
        }
        
        // ========== Building管理 ==========
        public BuildingLogic GetBuilding(int id)
        {
            return buildings[id];
        }

        public List<BuildingLogic> GetAllBuildings()
        {
            return buildings;
        }

        public int GetBuildingCount()
        {
            return buildings.Count;
        }
    }
}