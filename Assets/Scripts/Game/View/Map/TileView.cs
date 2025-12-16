using System.Collections.Generic;
using Core.Grid;
using Game.Data.Config;
using Game.Logic.Map;
using Game.Managers;
using UnityEngine;

namespace Game.View.Map
{
    public class TileView : MonoBehaviour
    {
        public TileConfig Config;
        
        private TileLogic tileLogic;
        public TileLogic TileLogic => tileLogic;
        
        private void Start()
        {
            RegisterToMapSystem();
        }

        public int GetId()
        {
            return Config.TileId;
        }

        private void RegisterToMapSystem()
        {
            var gameManager = GameManager.Instance;
            if (gameManager == null || gameManager.MapSystem == null)
            {
                Debug.LogWarning($"TileView {Config.TileId}: GameManager或MapSystem未初始化");
                return;
            }
            
            var mapSystem = gameManager.MapSystem;
            
            // 获取或创建Logic
            tileLogic = mapSystem.GetTile(Config.TileId);
            if (tileLogic == null)
            {
                // 创建新的Logic
                tileLogic = new TileLogic(Config.TileId);
                mapSystem.AddTile(tileLogic);
            }
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
