using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMessage : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public Image icon;

    private void Start()
    {
        if (messageText == null)
        {
            messageText = GetComponentInChildren<TextMeshProUGUI>();
        }
        icon = GetComponentInChildren<Image>();

        if (messageText==null&&icon==null)
        {
            Debug.Log("玩家message引用未实现！！！");
        }
    }

}
