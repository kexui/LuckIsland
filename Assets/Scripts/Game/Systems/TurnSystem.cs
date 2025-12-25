using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Events;
using Core.Systems;
using Game.Enums;
using Core.Systems;
using Game.Data.Config;
using Game.Data.System;
using Game.Events;
using Game.Systems.Turn;
using UnityEngine;

namespace Game.Systems
{
    public class TurnSystem : SystemBase,ITurnSystem
    {
        private TurnScheduler turnScheduler;
        public TurnState CurrentState { get; private set; }
        private bool isTurnCycleRunning = false;
        private int currentPlayerIndex;
        private List<int> playerOrder = new ();
        
        // ========== 计时器相关 ==========
        private CancellationTokenSource timerCancellationTokenSource;
        private bool isTimerRunning = false;
        private float currentPhaseTime = 0f;
        private float currentPhaseRemainingTime = 0f;
        
        private const string path = "";

        protected override void OnInitialize()
        {
            CurrentState = TurnState.Idle;
            currentPlayerIndex = -1;
            //var config = Resources.Load<TurnPhaseConfig>(path);
            turnScheduler = new TurnScheduler(new TurnPhaseData(),this);
        }

        protected override void OnCleanup()
        {
            currentPlayerIndex = -1;
            playerOrder.Clear();
        }

        public void StartTurnCycle()
        {
            if (isTurnCycleRunning)
            {
                Debug.LogWarning("TurnSystem:回合循环已在运行中");
                return;
            }

            isTurnCycleRunning = true;
            CurrentState = TurnState.Running;

            _ = RunTurnCycleAsync();
        }

        private async Task RunTurnCycleAsync()
        {
            if (turnScheduler != null && turnScheduler.HasNextPhase())
            {
                turnScheduler.StartFristPhase();
            }
            
            while (isTurnCycleRunning && turnScheduler != null && turnScheduler.HasNextPhase())
            {
                var currentPhase = turnScheduler.CurrentPhase;
                if (currentPhase != null)
                {
                    // 启动当前阶段的计时器
                    await StartPhaseTimerAsync(currentPhase);
                }
                
                // 移动到下一个阶段
                if (turnScheduler.HasNextPhase())
                {
                    turnScheduler.MoveToNextPhase();
                }
                else
                {
                    break; // 没有下一个阶段，结束循环
                }
            }
        }

        /// <summary>
        /// 启动阶段计时器
        /// </summary>
        public async Task StartPhaseTimerAsync(ITurnPhase phase)
        {
            if (phase == null)
            {
                Debug.LogWarning("TurnSystem: 尝试启动计时器，但Phase为null");
                return;
            }

            if (phase.time <= 0)
            {
                Debug.Log($"TurnSystem: Phase {phase.GetType().Name} 的时间为 {phase.time}，不启动计时器（无限等待）");
                return;
            }
            
            StopTimer();
            currentPhaseTime = phase.time;
            currentPhaseRemainingTime = phase.time;
            isTimerRunning = true;
            
            timerCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = timerCancellationTokenSource.Token;

            try
            {
                float updateInterval = 0.1f; // 每0.1秒更新一次
                
                while (currentPhaseRemainingTime > 0 && !cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay((int)(updateInterval * 1000), cancellationToken);
                    
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    currentPhaseRemainingTime -= updateInterval;
                    
                    // 发布计时器更新事件
                    if (EventBus.Instance != null)
                    {
                        var timerEvent = new TurnTimerEvent(
                            currentPhaseRemainingTime,
                            currentPhaseTime,
                            phase
                        );
                        EventBus.Instance.Publish(timerEvent);
                    }
                }

                // 时间到，触发阶段结束
                if (isTimerRunning && currentPhaseRemainingTime <= 0 && !cancellationToken.IsCancellationRequested)
                {
                    OnPhaseTimeUp(phase);
                }
            }
            catch (TaskCanceledException)
            {
                // 计时器被取消，正常情况
                Debug.Log($"TurnSystem: Phase {phase.GetType().Name} 的计时器被取消");
            }
            finally
            {
                isTimerRunning = false;
            }
        }

        public void StopTimer()
        {
            if (timerCancellationTokenSource != null)
            {
                timerCancellationTokenSource.Cancel();
                timerCancellationTokenSource.Dispose();
                timerCancellationTokenSource = null;
            }
            isTimerRunning = false;
            currentPhaseTime = 0f;
            currentPhaseRemainingTime = 0f;
        }

        private void OnPhaseTimeUp(ITurnPhase phase)
        {
            isTimerRunning = false;

            if (EventBus.Instance != null)
            {
                var timerUpEvent = new TurnPhaseTimeUpEvent(phase);
                EventBus.Instance.Publish(timerUpEvent);
            }
            
            Debug.Log($"TurnSystem: Phase {phase.GetType().Name} 时间到");
        }

        /// <summary>
        /// 手动切换到下一个阶段
        /// </summary>
        public void MoveToNextPhase()
        {
            if (turnScheduler != null && turnScheduler.HasNextPhase())
            {
                StopTimer();
                turnScheduler.MoveToNextPhase();
            }
        }

        public ITurnPhase GetCurrentPhase()
        {
            return turnScheduler.CurrentPhase;
        }
        
        public float GetRemainingTime()
        {
            return currentPhaseRemainingTime;
        }
    }
}