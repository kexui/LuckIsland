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
        
        private TileLogic tileLogic;
        public TileLogic TileLogic => tileLogic;
        
        private void Start()
        {
            RegisterToMapSystem();
        }

        public int GetId()
        {
            return data.TileId;
        }

        private void RegisterToMapSystem()
        {
            var gameManager = GameManager.Instance;
            if (gameManager == null || gameManager.MapSystem == null)
            {
                Debug.LogWarning($"TileView {data.TileId}: GameManager或MapSystem未初始化");
                return;
            }
            
            var mapSystem = gameManager.MapSystem;
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
