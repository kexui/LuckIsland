using System.Collections.Generic;
using Game.Data.Map;
using UnityEngine;

namespace Game.Data.Config
{
    public class MapRuntimeConfig : ScriptableObject
    {
        public List<TileData> tiles;
        public List<LandData> lands;
        public List<BuildingData> buildings;
    }
}
