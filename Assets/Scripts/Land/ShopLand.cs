using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopLand : FunctionalLand
{
    private void Awake()
    {
        Transform shopTranform = transform.Find("Shop");//只能找到直接子物体
        if (shopTranform!=null)
        {
            Building = shopTranform.gameObject;
        }
        else
        {
            Debug.Log("ShopLand没有找到Shop模型");
        }
    }
}
