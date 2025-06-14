using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBankCard", menuName = "Card/SpringTrap")]
public class SpringTrap : CardBase
{
    [Range(0,3)]
    public int backwardSteps;

    private void OnEnable()
    {
        cardName = "SpringTrap";
        weight = 2;
    }

    public override void UseCard(BasePlayerController player)
    {
        //退后方法
        Debug.Log("后退");
    }
}
