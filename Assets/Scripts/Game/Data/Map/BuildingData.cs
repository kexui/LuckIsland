using Core.Enums;

namespace Game.Data.Map
{
    [System.Serializable]
    public class BuildingData
    {
        public int Id;
        public int LandId;
        public BuildingType Type;

        public BuildingData(BuildingData data)
        {
            Id = data.Id;
            LandId = data.LandId;
            Type = data.Type;
        }
    }
}