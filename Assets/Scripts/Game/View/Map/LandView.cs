using Core.Enums;
using Core.Grid;
using Game.Data.Config;
using Game.Logic.Map;
using Game.Managers;
using UnityEngine;

namespace Game.View.Map
{
    public class LandView : MonoBehaviour
    {
        public LandConfig Config;
        
        private LandLogic landLogic;
        public LandLogic LandLogic => landLogic;
        
        public int GetID()
        {
            return Config.LandId;
        }
        
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
                Debug.LogWarning($"LandView {GetID()}: GameManager或MapSystem未初始化");
                return;
            }
            
            var mapSystem = gameManager.MapSystem;
            
            // 获取或创建Logic
            landLogic = mapSystem.GetLand(GetID());
            if (landLogic == null)
            {
                landLogic = new LandLogic(GetID());
                mapSystem.AddLand(landLogic);
            }
            
            Debug.Log($"LandView {GetID()} 注册成功");
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