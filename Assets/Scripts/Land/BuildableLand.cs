using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildableLand : FunctionalLand
{//ø…Ω®‘ÏLand
    private bool isBuild;
    [SerializeField] private GameObject fence;

    [SerializeField] private GameObject BusinessLv1;
    [SerializeField] private GameObject BusinessLv2;
    [SerializeField] private GameObject BusinessLv3;


    private void Awake()
    {
        Building = fence;
    }
    public void SpawmBusiness()
    {
        Destroy(Building);
        Building = Instantiate(BusinessLv1, GetTopPosition(), Quaternion.identity, this.transform);
    }
    public void UpgradeBusiness()
    { 
        Destroy(Building);
        Building = BusinessLv2;
    }
    
}
