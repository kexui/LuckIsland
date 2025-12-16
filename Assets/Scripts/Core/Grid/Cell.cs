using System.Collections.Generic;

namespace Core.Grid
{
    /// <summary>
    /// 暂时不使用
    /// 纯数据
    /// </summary>
    public class Cell
    {
        public int Id { get; }
        public GridPos Pos { get; }
        public Cell(int id, GridPos pos)
        {
            Id = id;
            Pos = pos;
        }
    }
}