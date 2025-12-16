using System.Collections.Generic;

namespace Game.Data.Map
{
    [System.Serializable]
    public class MapData
    {
        List<TileData> tiles;
        private Dictionary<int, LandData> lands;
    }
}
