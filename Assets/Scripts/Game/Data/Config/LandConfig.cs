using System;
using Core.Grid;

namespace Game.Data.Config
{
    [Serializable]
    public class LandConfig
    {
        public int LandId;
        
        public int TileId;
        public int BuildingId;

        public LandConfig()
        {
            LandId = -1;
            TileId = -1;
            BuildingId = -1;
        }

        public LandConfig(LandConfig config)
        {
            LandId =  config.LandId;
            TileId = config.TileId;
            BuildingId = config.BuildingId;
        }
    }
}