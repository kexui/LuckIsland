using Core.Grid;
using UnityEngine;

namespace Game.Managers
{
    public class GridManager : MonoBehaviour
    {
        public float sellSize = 1f;

        public Vector3 GetWorldPos(Cell cell)
        {
            return new Vector3(cell.Pos.X * sellSize, 0, cell.Pos.Y * sellSize);
        }
        public Vector3 GetWorldPos(Vector2Int gridPos)
        {
            return new Vector3(gridPos.x * sellSize, 0, gridPos.y * sellSize);
        }
    }
}