using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    public static WorldSpaceUI Instance;

    public BuildPromptUI buildPromptUI { get; private set; }

    private void Awake()
    {
        Instance = this;
        buildPromptUI = GetComponentInChildren<BuildPromptUI>();
    }

}
