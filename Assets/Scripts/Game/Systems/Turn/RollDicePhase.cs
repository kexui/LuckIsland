using Core.Systems;
using UnityEngine;

namespace Game.Systems.Turn
{
    public class RollDicePhase : ITurnPhase
    {
        public int time { get; } = 10;
        public void Enter()
        {
            Debug.Log("Entering StartPhase");
        }

        public void Exit()
        {
            
        }
    }
}