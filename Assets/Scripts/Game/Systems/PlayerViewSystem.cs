using System.Collections.Generic;
using System.ComponentModel;
using Core.Systems;
using Game.Events;
using Game.View.Player;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerViewSystem : SystemBase
    {
        private Dictionary<int, PlayerView> playerViewMap = new();
        private Transform playerContainer; // 玩家容器
        
        protected override void OnInitialize()
        {
            GameObject container = new GameObject("Players");
            playerContainer = container.transform;

            SubscribeEvent<PlayerCreatedEvent>(OnPlayerCreaed);
        }

        protected override void OnCleanup()
        {
            ClearSubscriptions();
            
            foreach (var view in playerViewMap.Values)
            {
                if (view != null)
                {
                    //
                }
            }
            playerViewMap.Clear();
            if (playerContainer != null)
            {
                //Destroy(playerContainer.gameObject);
            }
        }

        private void OnPlayerCreaed(Events.PlayerCreatedEvent evt)
        {
            CreatePlayerView(evt.PlayerLogic);
        }

        private void CreatePlayerView(PlayerLogic playerLogic)
        {
            if (playerLogic == null)
            {
                Debug.LogWarning("PlayerLogic为空，无法创建PlayerView");
                return;
            }
            
            GameObject playerObj = new GameObject($"Player_{playerLogic.GetId()}");
            playerObj.transform.SetParent(playerContainer);

            PlayerView playerView = playerObj.AddComponent<PlayerView>();
            playerView.Initialize(playerLogic.GetId(), playerLogic);
            
            playerViewMap.Add(playerLogic.GetId(), playerView);
            Debug.Log($"创建PlayerView: PlayerId={playerLogic.GetId()}");
        }
        
        public PlayerView GetPlayerView(int playerId)
        {
            playerViewMap.TryGetValue(playerId, out PlayerView view);
            return view;
        }
    }
}