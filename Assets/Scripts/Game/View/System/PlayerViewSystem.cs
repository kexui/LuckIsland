using System;
using System.Collections.Generic;
using Core.Systems;
using Game.Events;
using Game.View.Player;
using Unity.VisualScripting;
using UnityEngine;
using EventBus = Core.Events.EventBus;

namespace Game.Systems
{
    public class PlayerViewSystem : MonoBehaviour
    {
        private Dictionary<int, PlayerView> playerViewMap = new();
        
        private System.Collections.Generic.List<IDisposable> _subscriptions = new();
        
        private Transform playerRoot;
        private const string CHARACTERS_PATH = "Prefabs/Characters/";
        
        // ========== SystemBase ==========
        private void Awake()
        {
            GameObject container = new GameObject("Players");
            playerRoot = container.transform;
        }

        private void OnEnable()
        {
            if (EventBus.Instance == null)
            {
                
            }
            SubscribeEvent<PlayerCreatedEvent>(OnPlayerCreaed);
            SubscribeEvent<PlayerMoveStepRequestEvent>(OnPlayerMoveStep);
        }

        protected IDisposable SubscribeEvent<T>(Action<T> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
    
            if (EventBus.Instance == null)
            {
                Debug.LogWarning($"EventBus未初始化，无法订阅事件 {typeof(T).Name}");
                return null;
            }
    
            var subscription = EventBus.Instance.Subscribe(handler);
            if (subscription != null)
            {
                _subscriptions.Add(subscription);
            }
            return subscription;
        }

        private void OnDisable()
        {
            ClearSubscriptions();
        }

        private void OnDestroy()
        {
            foreach (var view in playerViewMap.Values)
            {
                if (view != null)
                {
                    //
                }
            }
            playerViewMap.Clear();
            if (playerRoot != null)
            {
                //Destroy(playerContainer.gameObject);
            }
        }
        
        protected virtual void ClearSubscriptions()
        {
            if (_subscriptions == null || _subscriptions.Count == 0)
            {
                return;
            }
            
            int count = _subscriptions.Count;
            foreach (var subscription in _subscriptions)
            {
                try
                {
                    subscription?.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{GetType().Name} 清理订阅时出错: {ex.Message}");
                }
            }
            
            _subscriptions.Clear();
            Debug.Log($"{GetType().Name} 清理了 {count} 个事件订阅");
        }
        
        // ========== Events ==========
        
        private void OnPlayerCreaed(Events.PlayerCreatedEvent evt)
        {
            CreatePlayerView(evt.PlayerLogic);
        }

        private void OnPlayerMoveStep(PlayerMoveStepRequestEvent evt)
        {
            //识别是谁
            
            //动

            var finishedEvent = new PlayerStepAnimationFinishedEvent();
            
            EventBus.Instance.Publish<PlayerStepAnimationFinishedEvent>(finishedEvent);
        }
        // ========== PlayerViewSystem ==========
        
        /// <summary>
        /// 创建玩家显示层
        /// </summary>
        private void CreatePlayerView(PlayerLogic playerLogic)
        {
            if (playerLogic == null)
            {
                Debug.LogWarning("PlayerLogic为空，无法创建PlayerView");
                return;
            }
            
            GameObject prefab = Resources.Load<GameObject>(CHARACTERS_PATH + playerLogic.GetCharacterPrefabName());
            if (prefab == null)
            {
                Debug.LogError($"Factory: 未能找到玩家角色，Name：{playerLogic.GetCharacterPrefabName()}，Path：{CHARACTERS_PATH}");
                return;
            }
            
            GameObject go = GameObject.Instantiate(prefab, playerRoot);
            var view = go.GetComponent<PlayerView>();
            view.Initialize(playerLogic.GetId(),playerLogic);
            
            playerViewMap.Add(playerLogic.GetId(), view);
            Debug.Log($"创建PlayerView: PlayerId={playerLogic.GetId()}");
        }
        
        public PlayerView GetPlayerView(int playerId)
        {
            playerViewMap.TryGetValue(playerId, out PlayerView view);
            return view;
        }
    }
}