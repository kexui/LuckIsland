using System.Collections;
using Core;
using Core.Systems;
using Game.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        //[Header("游戏状态")]
        public bool isGameStarted { get; private set; } = false;
        public bool isGamePaused { get; private set; } = false;
        
        // ========== System引用 ==========
        private MapSystem mapSystem;
        public IMapSystem MapSystem => mapSystem;
        public PlayerSystem PlayerSystem { get; private set; }
        public TurnSystem TurnSystem { get; private set; }
        
        //public BuildingSystem BuildingSystem { get; private set; }
        //public CardSystem CardSystem { get; private set; }
        //public EventSystem EventSystem { get; private set; }
        //public AISystem AISystem { get; private set; }
        //public UISystem UISystem { get; private set; }
        
        private void Start()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Game")
            {
                StartCoroutine(InitializeGame());
            }
        }

        //初始化服务
        private IEnumerator InitializeGame()
        {
            mapSystem = new MapSystem();
            mapSystem.Initialize();
            mapSystem.Enable();
            yield return null;

            //PlayerSystem = new PlayerSystem(MapSystem);
            //PlayerSystem.Initialize();
            //PlayerSystem.Enable();
            
            TurnSystem = new TurnSystem();
            TurnSystem.Initialize();
            TurnSystem.Enable();
            
            yield return null;
            
            StartGame();
        }

        //开始游戏
        public void StartGame()
        {
            if (isGameStarted)
            {
                return;
            }
            isGameStarted = true;
            isGamePaused = false;
            
            // 订阅游戏结束事件
            //EventBus.Instance.Subscribe<Events.GameOverEvent>(OnGameOver);
            
            //事件
            if (TurnSystem != null)
            {
                TurnSystem.StartTurnCycle();
            }
            Debug.Log("Game started");
        }

        public void PauseGame()
        {
            if (!isGamePaused)
            {
                Debug.LogWarning("游戏未开始，无法暂停");
                return;
            }
            isGamePaused = true;
            Time.timeScale = 0;
            
            //暂停所有服务
            //TurnSystem?.Disable();
            //UISystem?.Disable();
        }

        public void ResumeGame()
        {
            if (!isGameStarted)
            {
                Debug.LogWarning("游戏未开始，无法恢复");
                return;
            }

            if (!isGamePaused)
            {
                return;
            }

            isGamePaused = false;
            Time.timeScale = 1f;
            
            // 恢复所有System
            //TurnSystem?.Enable();
            
            Debug.Log("游戏恢复");
        }
        
        //private void OnGameOver(Events.GameOverEvent evt)

        public void ReturnToMenu()
        {
        }

        public void RestartGame()
        {
        }

        /// <summary>
        /// 清理所有System
        /// </summary>
        private void CleanupAllSystems()
        {
            mapSystem?.Cleanup();
            PlayerSystem?.Cleanup();
            //TurnSystem?.Cleanup();
            //UISystem?.Cleanup();
            //CardSystem?.Cleanup();
            //BuildingSystem?.Cleanup();
            //EventSystem?.Cleanup();
            //AISystem?.Cleanup();
        }
        
        private void OnDestroy()
        {
            // 清理订阅
            if (Core.Events.EventBus.Instance != null)
            {
                //EventBus.Instance.Unsubscribe<Events.GameOverEvent>(OnGameOver);
            }
            
            // 清理System
            CleanupAllSystems();
        }
    }
}