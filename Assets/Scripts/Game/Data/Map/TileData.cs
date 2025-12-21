using Core.Grid;

namespace Game.Data.Map
{
    [System.Serializable]
    public class TileData
    {
        public int TileId;//Id
        public int FrontIndex;//前方Tile的ID
        public int BackIndex;//后方Tile的ID
        public int[] AdjacentLandIds;//相邻Land的ID
        public GridPos Pos;//位置
    }
}
