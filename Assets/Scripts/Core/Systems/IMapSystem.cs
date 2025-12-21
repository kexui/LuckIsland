using System.Collections.Generic;
using Game.Logic.Map;

namespace Core.Systems
{
    /// <summary>
    /// 地图系统公共接口，供其他System使用
    /// </summary>
    public interface IMapSystem
    {
        TileLogic GetTile(int index);
        LandLogic GetLand(int landId);
        
        //List<LandLogic> GetAdjacentLands(int tileIndex);
        //List<TileLogic> GetTilesAdjacentToLand(int landId);
    }
}