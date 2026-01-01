using Core.Systems;
using Game.Enums;
using UnityEngine;

namespace Game.Systems.Turn
{
    public class WaitPhase : ITurnPhase
    {
        public TurnPhase Phase { get; } = TurnPhase.Wait;
        public int Time { get; } = 3;
        public void Enter()
        {
            Debug.Log("Entering WaitPhase");
        }

        public void Exit()
        {
            
        }
    }
}