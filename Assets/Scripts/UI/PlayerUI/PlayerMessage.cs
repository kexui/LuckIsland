using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    private Image icon;

    private void Start()
    {
        if (messageText == null)
        {
            messageText = GetComponentInChildren<TextMeshProUGUI>();
        }
        icon = GetComponentInChildren<Image>();
    }



}
