using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPromptUI : MonoBehaviour
{
    private Button buildButton, cancelButton;

    private void OnEnable()
    {
        GameManager.OnInitUI += GameManager_OnInitUI;
        TurnManager.OnEndTurn += TurnManager_OnEndTurn;
    }

    private void TurnManager_OnEndTurn()
    {
        Hide();
    }

    private void GameManager_OnInitUI()
    {
        buildButton = transform.Find("BuildButton").GetComponent<Button>();
        cancelButton = transform.Find("CancelButton").GetComponent<Button>();
        //Hide();
        transform.position = Vector3.zero;
    }

    
    public void Show(Vector3 pos,System.Action onConfirm)//位置，建造方法
    {
        transform.position = pos+Vector3.up*3f;
        Show();
        buildButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        buildButton.onClick.AddListener(() =>
        {
            onConfirm.Invoke();
            Hide();
        });
        cancelButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }
    public void Hide()
    {
        buildButton.enabled = false;
        cancelButton.enabled = false;
        transform.position= Vector3.zero;
    }
    public void Show()
    {
        buildButton.enabled = true;
        cancelButton.enabled = true;
    }
}
