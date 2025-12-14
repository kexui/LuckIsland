using Core.Enums;
using Core.Logic;

namespace Game.Logic.Building
{
    public class Buildinglogic : LogicBase
    {
        public BuildingType BuildingType { get; }
        public int OwnerId { get; private set; }
        public int Level { get; private set; }

        public Buildinglogic(int id,BuildingType buildingType,int ownerId)
        {
            this.id = id;
            this.BuildingType = buildingType;
            this.OwnerId = ownerId;
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