using System;
using Core.Enums;
using Core.Grid;

namespace Game.Data.Map
{
    [Serializable]
    public class BuildingData
    {
        public int Id;
        
        public int LandId;
        public BuildingType Type;
        public GridPos Pos;

        public BuildingData(BuildingData data)
        {
            Id = data.Id;
            LandId = data.LandId;
            Type = data.Type;
            Pos = data.Pos;
        }
    }
}