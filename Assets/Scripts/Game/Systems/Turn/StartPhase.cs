using Core.Systems;
using UnityEngine;

namespace Game.Systems.Turn
{
    public class StartPhase:ITurnPhase
    {
        public int time { get; } = 3;

        public void Enter()
        {
            Debug.Log("Entering StartPhase");
        }

        public void Exit()
        {
            
        }
    }
}