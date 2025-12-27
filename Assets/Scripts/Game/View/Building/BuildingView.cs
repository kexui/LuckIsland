using Game.Data.Config;
using Game.Data.Map;
using UnityEngine;

namespace Game.View.Building
{
    public class BuildingView : ViewBase
    {
        public BuildingData data;

        public override int GetId()
        {
            return data.Id;
        }
    }
}