using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public abstract void Activate(PlayerData user);
}
