using Game.Data.Map;
using Game.Managers;
using Game.Utils;
using UnityEngine;

namespace Game.View.Map
{
    public class TileView : ViewBase
    {
        public TileData data;
        
        public override int GetId()
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
