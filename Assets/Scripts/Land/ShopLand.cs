using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopLand : FunctionalLand
{
    private void Awake()
    {
        Transform shopTranform = transform.Find("Shop");//ֻ���ҵ�ֱ��������
        if (shopTranform!=null)
        {
            Building = shopTranform.gameObject;
        }
        else
        {
            Debug.Log("ShopLandû���ҵ�Shopģ��");
        }
    }
}
