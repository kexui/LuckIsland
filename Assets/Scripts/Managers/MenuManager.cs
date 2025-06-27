using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;
    public void OnSinglePlayerClick()
    { //����ģʽ
        sceneLoader.LoadGameScene();
    }
    public void OnMultiplayerClick()
    { //����ģʽ
    
    }
    public void OnSettingsClick()
    { //����
        
    }
    // void OnTutorialClick(){ }
    public void OnQuitClick()
    { 
        sceneLoader.QuitGame();
    }
}
