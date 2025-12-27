using Core;
using Core.Grid;
using UnityEngine;

namespace Game.Managers
{
    public class GridManager : Singleton<GridManager>
    {
        public float cellSize = 1f;

        public Vector3 GetWorldPos(Cell cell)
        {
            return new Vector3(cell.Pos.X * cellSize, cell.Pos.Y * cellSize, cell.Pos.Z * cellSize);
        }
        
        public Vector3 GetWorldPos(Vector3Int gridPos)
        {
            return new Vector3(gridPos.x * cellSize, gridPos.y * cellSize, gridPos.z * cellSize);
        }
    }
}