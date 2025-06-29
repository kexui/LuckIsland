using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayerResultItemUI playerResultItemUI;
    [SerializeField] private Transform RankUI;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void Show(List<PlayerData> playerDatas)
    { 
        gameObject.SetActive(true);
        foreach (PlayerData playerData in playerDatas)
        {
            GameObject go = Instantiate(playerResultItemUI.gameObject, RankUI.transform.position, Quaternion.identity, RankUI.transform);
            go.GetComponent<PlayerResultItemUI>().PlayerResult(playerData);
        }
    }
    private void UpdateData()
    { 
        
    }
}
