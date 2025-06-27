using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFloatingUI : MonoBehaviour
{
    [SerializeField] private PlayerMessage playerMessage;
    [SerializeField] private GameObject playerMessageText;

    [SerializeField] private Sprite icon;

    private void Awake()
    {
        if (playerMessage==null &&playerMessageText==null)
        {
            Debug.Log("玩家消息UI未设置引用！！！");
            return;
        }
    }

    public void ShowMessage(int value)
    {
        GameObject go = Instantiate(playerMessage.gameObject, transform.position,Quaternion.identity,transform);
        PlayerMessage Message = go.GetComponent<PlayerMessage>();
        if (Message ==null)
        {
            Debug.Log("PlayerFloatingUI!!!");
            return;
        }
        Message.icon.sprite = icon;
        Message.messageText.text = value.ToString();
        Destroy(go, 2f);
    }
}
