using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Events;
using Core.Systems;
using Game.Enums;
using Game.Data.System;
using Game.Events;
using Game.Systems.Turn;
using UnityEngine;

namespace Game.Systems
{
    public class TurnSystem : SystemBase,ITurnSystem
    {
        private TurnScheduler turnScheduler; //调度器
        public TurnState CurrentState { get; private set; } //回合状态
        private bool isTurnCycleRunning = false; //回合循环运行
        public int currentPlayerIndex { get; private set; } //当前玩家下标
        private List<int> playerOrder = new (); //所有玩家
        
        // ========== 计时器相关 ==========
        private CancellationTokenSource timerCancellationTokenSource; //
        private bool isTimerRunning = false; //时间中
        private float currentPhaseTime = 0f; //回合时间
        private float currentPhaseRemainingTime = 0f; //回合剩余时间
        
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
            isTurnCycleRunning = false;
            StopTimer();
            
            currentPlayerIndex = -1;
            playerOrder.Clear();
            Debug.Log("TurnSystem: 清理完成");
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
            if (turnScheduler == null)
            {
                Debug.LogWarning("TurnSystem: turnScheduler为空");
                return;
            }

            Debug.Log($"TurnSystem: 开始回合循环，总阶段数: {turnScheduler.GetPhaseCount()}");
            
            if (turnScheduler.HasNextPhase())
            {
                turnScheduler.StartFristPhase();
            }
            
            //阶段循环
            while (isTurnCycleRunning)
            {
                var currentPhase = turnScheduler.CurrentPhase;
                Debug.Log($"TurnSystem: 当前阶段索引: {turnScheduler.CurrentPhase}, 阶段: {currentPhase?.GetType().Name}");
                
                if (currentPhase != null)
                {
                    Debug.Log($"TurnSystem: 开始等待阶段 {currentPhase.GetType().Name}，时间: {currentPhase.Time}秒");
                    // 启动当前阶段的计时器
                    await StartPhaseTimerAsync(currentPhase);
                }
                
                if (!isTurnCycleRunning)
                {
                    Debug.Log("TurnSystem: 回合循环已停止");
                    break;
                }
                
                // 移动到下一个阶段
                if (turnScheduler.HasNextPhase())
                {
                    Debug.Log($"TurnSystem: 切换到下一个阶段");
                    turnScheduler.MoveToNextPhase();
                }
                else
                {
                    Debug.LogWarning("TurnSyatem: 无下一个阶段");
                    break; // 没有下一个阶段，结束循环
                }
            }
            
            Debug.Log("TurnSystem: 回合循环结束");
            isTurnCycleRunning = false;
            CurrentState = TurnState.Idle;
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

            if (phase.Time <= 0)
            {
                Debug.Log($"TurnSystem: Phase {phase.GetType().Name} 的时间为 {phase.Time}，不启动计时器（无限等待）");
                return;
            }
            
            StopTimer();
            currentPhaseTime = phase.Time;
            currentPhaseRemainingTime = phase.Time;
            isTimerRunning = true;
            
            timerCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = timerCancellationTokenSource.Token;
            try
            {
                float updateInterval = 0.1f; // 每0.1秒更新一次
                
                while (currentPhaseRemainingTime > 0 && !cancellationToken.IsCancellationRequested && isTurnCycleRunning)
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
                if (isTimerRunning && currentPhaseRemainingTime <= 0 && !cancellationToken.IsCancellationRequested && isTurnCycleRunning)
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

        /// <summary>
        /// 停止时间
        /// </summary>
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

        /// <summary>
        /// 回合时间到
        /// </summary>
        /// <param name="phase"></param>
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

        /// <summary>
        /// 获取当前回合
        /// </summary>
        /// <returns></returns>
        public ITurnPhase GetCurrentPhase()
        {
            return turnScheduler.CurrentPhase;
        }
        
        /// <summary>
        /// 获取剩余时间
        /// </summary>
        /// <returns></returns>
        public float GetRemainingTime()
        {
            return currentPhaseRemainingTime;
        }
    }
}