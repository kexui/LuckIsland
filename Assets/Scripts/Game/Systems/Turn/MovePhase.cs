using Core.Systems;
using UnityEngine;

namespace Game.Systems.Turn
{
    public class MovePhase : ITurnPhase
    {
        public int time { get; } = 7;
        public void Enter()
        {
            Debug.Log("Entered MovePhase");
        }

        public void Exit()
        {
            
        }
    }
}