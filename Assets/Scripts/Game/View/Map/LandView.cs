using Game.Logic.Map;
using Game.Managers;
using UnityEngine;

namespace Game.View.Map
{
    public class LandView : MonoBehaviour
    {
        public int landId = 0;
        private LandLogic landLogic;
        
        public LandLogic LandLogic => landLogic;
        public Vector3 Position => transform.position;
        
        
        private void Start()
        {
            RegisterToMapSystem();
        }
        
        /// <summary>
        /// 注册到MapSystem
        /// </summary>
        private void RegisterToMapSystem()
        {
            var gameManager = GameManager.Instance;
            if (gameManager == null || gameManager.MapSystem == null)
            {
                Debug.LogWarning($"LandView {landId}: GameManager或MapSystem未初始化");
                return;
            }
            
            var mapSystem = gameManager.MapSystem;
            
            // 获取或创建Logic
            landLogic = mapSystem.GetLand(landId);
            if (landLogic == null)
            {
                landLogic = new LandLogic(landId);
                mapSystem.AddLand(landLogic);
            }
            
            Debug.Log($"LandView {landId} 注册成功");
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