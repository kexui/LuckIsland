using Core.Events;
using Core.Systems;
using Game.Enums;
using Game.Events;
using Game.Managers;
using UnityEngine;

namespace Game.Systems
{
    public class MoveSystem: SystemBase , IMoveSystem
    {
        private IMapSystem mapSystem;
        private IPlayerSystem playerSystem;

        public  MoveSystem(IMapSystem mapSystem , IPlayerSystem playerSystem)
        {
            this.mapSystem = mapSystem;
            this.playerSystem = playerSystem;
        }

        // ========== SystemBase ==========
        protected override void OnInitialize()
        {
            
        }

        protected override void OnEnable()
        {
            SubscribeEvent<TurnPhaseChangedEvent>(OnPhaseChanged);
            SubscribeEvent<PlayerStepAnimationFinishedEvent>(OnFinishedOnceStep);
        }

        protected override void OnCleanup()
        {
            
        }
        
        // ========== Events ==========

        private void OnPhaseChanged(TurnPhaseChangedEvent evt)
        {
            if (evt.CurrentPhase.Phase == TurnPhase.Move)
            {
                MovePhase();
            }
        }

        private void OnFinishedOnceStep(PlayerStepAnimationFinishedEvent evt)
        {
            TryMoveNextStep(evt.PlayerId);
        }

        // ========== IMoveSystem 实现 ==========

        public void MovePhase()
        {
            var players = playerSystem.GetAllPlayers();
            foreach (var player in players)
            {
                StartMove(player.GetId(),player.GetRemainingSteps());
            }
        }

        private void StartMove(int playerId,int step)
        {
            var player = playerSystem.GetPlayer(playerId);
            if (player == null)
            {
                Debug.LogError($"MoveSystem: 无法找到玩家 {playerId}");
                return;
            }

            if (step < 0||step > 6)
            {
                Debug.LogError($"MoveSystem: 点数有问题");
                return;
            }
            
            //一步一步走
            TryMoveNextStep(playerId);
        }

        void TryMoveNextStep(int playerId)
        {
            var player = playerSystem.GetPlayer(playerId);
            if (player == null)
            {
                Debug.LogError($"MoveSystem: 无法找到玩家 {playerId}");
                return;
            }
            
            int currentTileIndex = player.GetCurrentTileIndex();
            int nextTileIndex = mapSystem.GetNextTile(currentTileIndex);
            int remainingStep = player.GetRemainingSteps();
            
            remainingStep--;
            ApplyStep(playerId, nextTileIndex, remainingStep);
            
            //动画
            var moveEvent = new PlayerMoveStepRequestEvent()
            {
                PlayerId = playerId,
                NextTileId = nextTileIndex
            };
            EventBus.Instance.Publish(moveEvent);
        }

        private void ApplyStep(int playerId,int currentTileIndex,int remainingStep)
        {
            var player = playerSystem.GetPlayer(playerId);
            if (player == null)
            {
                Debug.LogError($"MoveSystem: 无法找到玩家 {playerId}");
                return;
            }

            player.SetCurrentTileIndex(currentTileIndex);
            player.SetRemainingSteps(remainingStep);
        }
    }
}