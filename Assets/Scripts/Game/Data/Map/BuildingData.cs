using Core.Enums;
using UnityEngine;

namespace Game.Data.Map
{
    public class BuildingData : ScriptableObject
    {
        public int BuildingId;
        public int LandId;
        public BuildingType Type;
    }
}