using System;
using System.Collections.Generic;

namespace Core.Grid
{
    /// <summary>
    /// 空间结构 + 查询
    /// </summary>
    public class Grid
    {
        private Dictionary<GridPos, Cell> cells = new();
        private Dictionary<int, Cell> cellsById = new();
        private int nextId = 0;

        public Cell CreateCell(int x, int y ,int z)
        {
            var pos = new GridPos(x, y, z);
            if (cells.ContainsKey(pos))
            {
                throw new Exception($"Cell already exists at {x},{y}");
            }

            var cell = new Cell(nextId++, pos);
            cells[cell.Pos] = cell;
            cellsById[cell.Id] = cell;
            return cell;
        }
        
        public Cell GetCell(GridPos pos)
        {
            cells.TryGetValue(pos, out var cell);
            return cell;
        }

        public Cell GetCellById(int id)
        {
            cellsById.TryGetValue(id, out var cell);
            return cell;
        }
        
        public List<Cell> GetNeighbors(Cell cell)
        {
            var result = new List<Cell>();
            foreach (var dir in GridDirs.Dir)
            {
                var pos = new GridPos(cell.Pos.X + dir.X, cell.Pos.Y + dir.Y ,cell.Pos.Z + dir.Z);
                var neighbor = GetCell(pos);
                if (neighbor != null)
                {
                    result.Add(neighbor);
                }
            }
            return result;
        }
    }
}