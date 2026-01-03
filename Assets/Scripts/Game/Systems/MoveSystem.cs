using System.Collections.Generic;
using Core.Events;
using Core.Systems;
using Game.Data.System;
using Game.Enums;
using Game.Events;
using UnityEngine;

namespace Game.Systems
{
    public class MoveSystem: SystemBase , IMoveSystem
    {
        private IMapSystem mapSystem;
        private IPlayerSystem playerSystem;
        private Dictionary<int, MoveContext> moveContexts = new();

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
            if (!moveContexts.TryGetValue(evt.PlayerId,out var ctx))
            {
                Debug.LogError($"MoveSystem: PlayerStepAnimationFinishedEvent的playerid{evt.PlayerId}出错");
                return;
            }
            
            ctx.CurrentTileIndex = evt.CurrentTileIndex;
            ctx.RemainingSteps--;
            
            //playerSystem订阅？
            var player = playerSystem.GetPlayer(evt.PlayerId);
            player.SetCurrentTileIndex(evt.CurrentTileIndex);
            
            TryMoveNextStep(evt.PlayerId);
        }

        // ========== IMoveSystem 实现 ==========

        public void MovePhase()
        {
            //读取数据
            moveContexts.Clear();
            
            var players = playerSystem.GetAllPlayers();
            foreach (var player in players)
            {
                var id = player.GetId();
                var newMoveContext = new MoveContext()
                {
                    CurrentTileIndex = player.GetCurrentTileIndex(),
                    RemainingSteps = player.GetRollResult(),
                    IsFinished = false
                };
                moveContexts.Add(id, newMoveContext);
                TryMoveNextStep(id);
            }
        }

        void TryMoveNextStep(int playerId)
        {
            var ctx = moveContexts[playerId];
            if (ctx.IsFinished)
            {
                TryFinishMovePhase();
                return;
            }

            if (ctx.RemainingSteps <= 0)
            {
                ctx.IsFinished = true;
                
                return;
            }

            int currentTileIndex = ctx.CurrentTileIndex;
            int nextTileIndex = mapSystem.GetNextTile(currentTileIndex);
            
            //动画
            var moveEvent = new PlayerMoveStepRequestEvent()
            {
                PlayerId = playerId,
                NextTileId = nextTileIndex
            };
            EventBus.Instance.Publish(moveEvent);
        }

        private void TryFinishMovePhase()
        {
            foreach (var ctx in moveContexts.Values)
            {
                if (!ctx.IsFinished)
                {
                    return;
                }
                
                EventBus.Instance.Publish(new MovePhaseFinishedEvent());
            }
        }
    }
}