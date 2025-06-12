using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableLand : FunctionalLand
{
    private bool isBuild;
    [SerializeField] private GameObject fence;
    [SerializeField] private GameObject BusinessLv1;
    [SerializeField] private GameObject BusinessLv2;
    [SerializeField] private GameObject BusinessLv3;


    private void Awake()
    {
        Instantiate(fence,transform);
    }
}
