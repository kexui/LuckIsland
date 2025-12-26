using Core.Systems;
using UnityEngine;

namespace Game.Systems.Turn
{
    public class PlayerPhase : ITurnPhase
    {
        public int time { get; } = 15;
        public void Enter()
        {
            Debug.Log("Entering StartPhase");
        }

        public void Exit()
        {
            
        }
    }
}