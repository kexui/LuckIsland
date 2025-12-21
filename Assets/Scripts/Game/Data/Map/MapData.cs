using System.Collections.Generic;

namespace Game.Data.Map
{
    [System.Serializable]
    public class MapData
    {
        List<TileData> tiles;
        private Dictionary<int, LandData> landDic;
        private List<LandData> lands;
        private Dictionary<int, BuildingData> buildingDic;
    }
}
