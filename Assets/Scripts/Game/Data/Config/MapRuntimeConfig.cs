using System.Collections.Generic;
using UnityEngine;

namespace Game.Data.Config
{
    public class MapRuntimeConfig : ScriptableObject
    {
        public List<TileConfig> tiles;
        public List<LandConfig> lands;
        public List<BuildingConfig> buildings;
    }
}
