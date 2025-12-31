using Core.Systems;
using Game.Enums;
using UnityEngine;

namespace Game.Systems.Turn
{
    public class PlayerPhase : ITurnPhase
    {
        public  TurnPhase Phase { get; } = TurnPhase.Player;
        public int Time { get; } = 15;
        public void Enter()
        {
            Debug.Log("Entering StartPhase");
        }

        public void Exit()
        {
            
        }
    }
}