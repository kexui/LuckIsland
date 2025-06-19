using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBankCard", menuName = "Card/SpringTrap")]
public class SpringTrap : CardDataBase
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
        //�˺󷽷�
        Debug.Log("����");
    }
}
