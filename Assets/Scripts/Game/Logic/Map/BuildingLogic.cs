using Core.Enums;
using Core.Logic;

namespace Game.Logic.Map
{
    public class BuildingLogic : LogicBase
    {
        public BuildingType BuildingType { get; set; }

        public BuildingLogic(int index,BuildingType buildingType)
        {
            id = index;
            this.BuildingType = buildingType;
        }
    }
}