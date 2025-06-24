using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        
    }

    
}
