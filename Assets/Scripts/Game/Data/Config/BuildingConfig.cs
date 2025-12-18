using Core.Enums;
using UnityEngine;

namespace Game.Data.Config
{
    [System.Serializable]
    public class BuildingConfig
    {
        public int BuildingId;
        public BuildingType type;

        public BuildingConfig(BuildingConfig config)
        {
            BuildingId = config.BuildingId;
            type = config.type;
        }
    }
}