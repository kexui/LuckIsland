using Core.Systems;
using Game.Enums;
using UnityEngine;

namespace Game.Systems.Turn
{
    public class MovePhase : ITurnPhase
    {
        public TurnPhase Phase { get; } = TurnPhase.Move;
        public int Time { get; } = 7;
        public void Enter()
        {
            Debug.Log("Entered MovePhase");
        }

        public void Exit()
        {
            
        }
    }
}