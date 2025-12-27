using Core.Grid;
using UnityEngine;

namespace Game.Utils
{
    public static class GridHelper
    {
        // public static GridPos GetGridPosByPosition(this MonoBehaviour mono, float cellSize)
        // {
        //     Vector3 position = mono.transform.position;
        //     GridPos pos = new GridPos(Mathf.RoundToInt(position.x / cellSize), Mathf.RoundToInt(position.y / cellSize),
        //         Mathf.RoundToInt(position.z / cellSize));
        //     return pos;
        // }
        
        public static GridPos GetGridPosByPosition(Vector3 position, float cellSize)
        {
            GridPos pos = new GridPos(Mathf.RoundToInt(position.x / cellSize), Mathf.RoundToInt(position.y / cellSize),
                Mathf.RoundToInt(position.z / cellSize));
            return pos;
        }
    }
}