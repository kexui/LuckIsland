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
        private List<ITurnPhase> allPhases;
        private ITurnPhase currentPhase;
        private int currentPhaseIndex = -1;
        public ITurnPhase CurrentPhase =>  currentPhase;
        
        private TurnSystem turnSystem;
        
        public  TurnScheduler(TurnPhaseData data,TurnSystem turnSystem)
        {
            allPhases = new List<ITurnPhase>(data.AllTurnPhases);
            this.turnSystem = turnSystem;
        }
    
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

        public bool HasNextPhase()
        {
            return allPhases != null && currentPhaseIndex < allPhases.Count;
        }

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
                var phaseChangeedEvent = new TurnPhaseChangedEvent(previousPhase, currentPhase);
                EventBus.Instance.Publish(phaseChangeedEvent);
            }
            
            Debug.Log($"TurnScheduler: 切换到阶段 {currentPhase.GetType().Name}");
        }
        
    }
}