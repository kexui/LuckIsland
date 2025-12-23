using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Systems;
using Game.Enums;
using Core.Systems;
using Game.Data.Config;
using Game.Data.System;
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
        private List<int> playerOrder;

        private const string path = "";

        public TurnSystem()
        {
            playerOrder = new ();
        }

        protected override void OnInitialize()
        {
            CurrentState = TurnState.Idle;
            currentPlayerIndex = -1;
            //var config = Resources.Load<TurnPhaseConfig>(path);
            turnScheduler = new TurnScheduler(new TurnPhaseData());
        }

        protected override void OnCleanup()
        {
            currentPlayerIndex = -1;
            playerOrder.Clear();
        }

        public void StartTurnCycle()
        {
            isTurnCycleRunning = true;
            CurrentState = TurnState.Running;
        }

        
        
        public void AutoChangeTurn()
        {
            
        }
    }
}