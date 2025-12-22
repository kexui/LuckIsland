using System.Collections.Generic;
using Core.Grid;
using Game.Data.Config;
using Game.Data.Map;
using Game.Logic.Map;
using Game.Managers;
using UnityEngine;

namespace Game.View.Map
{
    public class TileView : MonoBehaviour
    {
        public TileData data;
        
        public int GetId()
        {
            return data.TileId;
        }
        
        
        /// <summary>
        /// 在编辑器中可视化（可选）
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }
}
