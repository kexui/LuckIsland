using Core.Enums;
using Core.Logic;
using Game.Data.Map;

namespace Game.Logic.Map.Building
{
    public class BuildingLogic : LogicBase
    {
        private readonly int landId;
        public BuildingType BuildingType { get; }
        public int OwnerId { get; private set; }
        public int Level { get; private set; }

        public BuildingLogic(BuildingData data)
        {
            id = data.Id;
            landId = data.LandId;
            BuildingType = data.Type;
            OwnerId = -1;
            Level = 1;
        }
        
        // ========== 业务方法（自身逻辑）==========
        public void Upgrade()
        {
        }
        
        // ========== 效果方法（需要System层调用）==========
        // 注意：实际效果逻辑在System层通过IBuildingEffect实现
    }
}