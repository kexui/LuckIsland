using Core.Enums;
using UnityEngine;

namespace Game.Data.Map
{
    public class BuildingData : ScriptableObject
    {
        public int BuildingId;
        public BuildingType Type;
        public string Name;
        public int MaxLevel;
        public LandData Land;
        public GameObject prefab;
    }
}