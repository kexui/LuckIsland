using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerController : BasePlayerController
{
    public override IEnumerator RollDice()
    {
        hasStopRoll = true;
        yield return new WaitForSeconds(1f);
        playerData.dice.StartStopRollDice(2f); //AI×Ô¶¯Ò¡÷»×Ó
        yield return new WaitForSeconds(0.1f);
    }
}
