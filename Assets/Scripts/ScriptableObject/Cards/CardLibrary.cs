using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardLibrary", menuName = "Card/CardLibrary")]
public class CardLibrary : ScriptableObject
{
    public List<CardDataBase> allCard;
}
