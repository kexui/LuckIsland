using Core.Systems;
using Game.Enums;
using UnityEngine;

namespace Game.Systems.Turn
{
    public class StartPhase:ITurnPhase
    {
        public TurnPhase Phase { get; } = TurnPhase.Start;
        public int Time { get; } = 3;

        public void Enter()
        {
            Debug.Log("Entering StartPhase");
        }

        public void Exit()
        {
            
        }
    }
}