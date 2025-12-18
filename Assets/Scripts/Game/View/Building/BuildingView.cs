using Game.Data.Config;
using UnityEngine;

namespace Game.View.Building
{
    public class BuildingView : MonoBehaviour
    {
        public BuildingConfig Config;

        public int GetId()
        {
            return Config.BuildingId;
        }
    }
}