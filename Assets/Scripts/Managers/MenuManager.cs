using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] private GameObject SettingUI;

    public void OnSinglePlayerClick()
    { //����ģʽ
        sceneLoader.LoadGameScene();
    }
    public void OnMultiplayerClick()
    { //����ģʽ
    
    }
    public void OnSettingsClick()
    { //����
        SettingUI.SetActive(true);
    }
    // void OnTutorialClick(){ }
    public void OnQuitClick()
    { 
        sceneLoader.QuitGame();
    }
}
