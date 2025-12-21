using Game.Data.Config;
using Game.Data.Map;
using UnityEngine;

namespace Game.View.Building
{
    public class BuildingView : MonoBehaviour
    {
        public BuildingData data;

        public int GetId()
        {
            return data.Id;
        }
    }
}