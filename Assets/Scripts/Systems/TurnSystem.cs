using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using UnityEngine;

public class TurnSystem
{
    public TurnState CurrentState { get; private set; } = TurnState.Start;
    private List<PlayerLogic> Players;

    private bool isTurnCycleRunning = false;

    
    public void StartTurnCycle()
    {
        isTurnCycleRunning = true;
    }

}
