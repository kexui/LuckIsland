using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] private GameObject SettingUI;

    public void OnSinglePlayerClick()
    { //单人模式
        sceneLoader.LoadGameScene();
    }
    public void OnMultiplayerClick()
    { //多人模式
    
    }
    public void OnSettingsClick()
    { //设置
        SettingUI.SetActive(true);
    }
    // void OnTutorialClick(){ }
    public void OnQuitClick()
    { 
        sceneLoader.QuitGame();
    }
}
