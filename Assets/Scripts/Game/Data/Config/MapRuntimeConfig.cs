using System.Collections.Generic;
using Game.Data.Config;
using Game.Data.Map;
using UnityEngine;

namespace Game.Data.Comfig
{
    public class MapRuntimeConfig : ScriptableObject
    {
        public List<TileConfig> tiles;
        public List<BuildingConfig> buildings;
        public List<LandConfig> lands;
    }
}
