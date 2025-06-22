using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewCharacter" , menuName ="Game/Character")]
public class CharacterData : ScriptableObject
{//½ÇÉ«Êý¾Ý
    public string characterName;
    public GameObject modelPrefab;
    public Sprite portrait;
    public SkillBase skillBase;
}
