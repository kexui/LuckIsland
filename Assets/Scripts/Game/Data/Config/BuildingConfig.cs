using Core.Enums;
using UnityEngine;

namespace Game.Data.Config
{
    [System.Serializable]
    public class BuildingConfig
    {
        public int BuildingId;
        public int LandId;
        public BuildingType Type;

        public BuildingConfig(BuildingConfig config)
        {
            BuildingId = config.BuildingId;
            LandId = config.LandId;
            Type = config.Type;
        }
    }
}