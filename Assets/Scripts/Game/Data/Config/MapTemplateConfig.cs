using System.Collections.Generic;
using Core.Grid;
using UnityEngine;

namespace Game.Data.Config
{
    //模板地图，用于生成简易版地图
    [CreateAssetMenu(menuName = "Data/MapTemplate")]
    public class MapTemplateConfig : ScriptableObject
    {
        public int width;
        public int height;
        public List<Cell> TileCells = new();
        public List<Cell> LandCells = new();
    }
}
