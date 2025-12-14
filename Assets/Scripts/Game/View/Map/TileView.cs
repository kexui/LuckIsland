using System.Collections.Generic;
using Game.Logic.Map;
using Game.Managers;
using UnityEngine;

namespace Game.View.Map
{
    public class TileView : MonoBehaviour
    {
        public int tileIndex = 0;
        
        [Header("邻居关系（Editor工具自动生成）")]
        public int FrontIndex = -1;
        public int BackIndex = -1;
        public int[] adjacentLandIds = new int[2];  // 相邻Land的ID
        
        private TileLogic tileLogic;
        public TileLogic TileLogic => tileLogic;
        public Vector3 Position => transform.position;

        private void Start()
        {
            RegisterToMapSystem();
        }

        private void RegisterToMapSystem()
        {
            var gameManager = GameManager.Instance;
            if (gameManager == null || gameManager.MapSystem == null)
            {
                Debug.LogWarning($"TileView {tileIndex}: GameManager或MapSystem未初始化");
                return;
            }
            
            var mapSystem = gameManager.MapSystem;
            
            // 获取或创建Logic
            tileLogic = mapSystem.GetTile(tileIndex);
            if (tileLogic == null)
            {
                // 创建新的Logic
                tileLogic = new TileLogic(tileIndex);
                mapSystem.AddTile(tileLogic);
            }
        }
        
        
        /// <summary>
        /// 在编辑器中可视化（可选）
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(Position, Vector3.one);
        }
    }
}
