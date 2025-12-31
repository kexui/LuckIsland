using System;
using System.Collections.Generic;
using Core.Events;
using Core.Systems;
using Game.Data.System;
using Game.Events;
using UnityEngine;

namespace Game.Systems.Turn
{
    public class TurnScheduler
    {
        private List<ITurnPhase> allPhases; //所有阶段
        private ITurnPhase currentPhase;
        public ITurnPhase CurrentPhase =>  currentPhase; //当前阶段
        private int currentPhaseIndex = -1; //当前阶段下标
        
        private TurnSystem turnSystem;
        
        public  TurnScheduler(TurnPhaseData data,TurnSystem turnSystem)
        {
            allPhases = new List<ITurnPhase>(data.AllTurnPhases);
            this.turnSystem = turnSystem;
        }
    
        /// <summary>
        /// 开始第一个阶段
        /// </summary>
        public void StartFristPhase()
        {
            if (allPhases == null || allPhases.Count == 0)
            {
                Debug.LogWarning("TurnScheduler: 没有可用的阶段");
                return;
            }

            currentPhaseIndex = 0;
            ChangePhase(allPhases[currentPhaseIndex]);
        }

        /// <summary>
        /// 移动到下一个阶段
        /// </summary>
        public void MoveToNextPhase()
        {
            if (!HasNextPhase())
            {
                Debug.Log("TurnScheduler: 没有下一个阶段，回合结束");
                // TODO: 触发回合结束事件
                return;
            }

            currentPhaseIndex++;
            ChangePhase(allPhases[currentPhaseIndex]);
        }

        /// <summary>
        /// 是否有下一个阶段
        /// </summary>
        /// <returns></returns>
        public bool HasNextPhase()
        {
            return allPhases != null && currentPhaseIndex < allPhases.Count - 1;
        }

        /// <summary>
        /// 改变阶段
        /// </summary>
        /// <param name="next"></param>
        public void ChangePhase(ITurnPhase next)
        {
            if (next == null)
            {
                Debug.LogWarning("TurnScheduler: 尝试切换到null阶段");
                return;
            }
            ITurnPhase previousPhase = currentPhase;
            
            if (currentPhase != null)
            {
                currentPhase.Exit();
            }
            
            currentPhase = next;
            currentPhase?.Enter();

            if (EventBus.Instance != null)
            {
                var phaseChangedEvent = new TurnPhaseChangedEvent(currentPhase, previousPhase);
        
                try
                {
                    EventBus.Instance.Publish(phaseChangedEvent);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"TurnScheduler: 发布 TurnPhaseChangedEvent 时出错: {ex.Message}\n{ex.StackTrace}");
                }
            }
            
            Debug.Log($"TurnScheduler: 切换到阶段 {currentPhase.GetType().Name}");
        }

        public int GetPhaseCount() => allPhases.Count;
    }
}