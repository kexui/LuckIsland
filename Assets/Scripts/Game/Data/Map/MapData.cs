using System.Collections.Generic;
using Game.Data.Config;

namespace Game.Data.Map
{
    [System.Serializable]
    public class MapData
    {
        public List<TileData> tiles;
        public List<LandData> lands;
        public List<BuildingData> buildings;
        
        // JSON 序列化方法
        public string ToJson()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
        
        // JSON 反序列化方法
        public static MapData FromJson(string json)
        {
            return UnityEngine.JsonUtility.FromJson<MapData>(json);
        }
    }
}
