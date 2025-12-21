using System;

namespace Game.Data.Map
{
    [Serializable]
    public class LandData
    {
        public int LandId;
        
        public int TileId;
        public int BuildingId;

        public LandData()
        {
            LandId = -1;
            TileId = -1;
            BuildingId = -1;
        }

        public LandData(LandData data)
        {
            LandId =  data.LandId;
            TileId = data.TileId;
            BuildingId = data.BuildingId;
        }
    }
}