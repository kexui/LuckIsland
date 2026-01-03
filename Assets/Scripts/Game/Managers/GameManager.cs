using System.Collections;
using Core;
using Game.Data.Context;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        //[Header("游戏状态")]
        public bool isGameStarted { get; private set; } = false;
        public bool isGamePaused { get; private set; } = false;
        
        // ========== 字段 ==========
        public int LocalPlayerId { get; private set; } = -1;
        
        // ========== System引用 ==========
        public GameContext Context { get; private set; }
        
        private void Start()
        {
            Context = new GameContext();
            
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Game")
            {
                StartCoroutine(InitializeGame());
            }
        }

        //初始化服务
        private IEnumerator InitializeGame()
        {
            Context.Initialize();
            Context.Enable();
            
            LoadPlayers();
            StartGame();
            yield return null;
        }

        private void LoadPlayers()
        {
            if (Context.PlayerSystem != null)
            {
                Context.PlayerSystem.CreatePlayer(1,"Player1",500,0,"Character_A");
                Context.PlayerSystem.CreatePlayer(2,"Player2",500,0,"Character_B");
                LocalPlayerId = 1;
            }
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
            if (Context.TurnSystem != null)
            {
                Context.TurnSystem.StartTurnCycle();
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
            Context?.Cleanup();
        }
        
        private void OnDestroy()
        {
            // 清理System
            CleanupAllSystems();
            
            if (Core.Events.EventBus.Instance != null)
            {
                Destroy(Core.Events.EventBus.Instance.gameObject);
            }
        }
    }
}