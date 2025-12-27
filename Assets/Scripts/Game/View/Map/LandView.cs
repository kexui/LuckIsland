using Core.Enums;
using Core.Grid;
using Game.Data.Config;
using Game.Data.Map;
using Game.Logic.Map;
using Game.Managers;
using UnityEngine;

namespace Game.View.Map
{
    public class LandView : ViewBase
    {
        public LandData data;//Runtimedata
        
        public override int GetId()
        {
            return data.LandId;
        }

        /// <summary>
        /// 在编辑器中可视化（可选）
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, Vector3.one);
        }
    }
}