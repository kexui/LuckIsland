using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject SinglePlayerUI;
    public GameObject MultiplayerUI;
    public GameObject SettingsUI;
    public GameObject MainMenuUI;


    public void OnClick(MenuStage menuStage)
    {
        switch (menuStage)
        {
            case MenuStage.SinglePlayer:
                SinglePlayerUI.SetActive(true);
                break;
            case MenuStage.Multiplayer:

                break;
            case MenuStage.Settings:
                SettingsUI.SetActive(true);
                break;
            case MenuStage.Quit:
                break;
            default:
                break;
        }
    }
    public void OnSinglePlayerClick()
    { 
        SinglePlayerUI.SetActive(true);
    }
    public void OnMultiplayerClick()
    { 
    
    }
    public void OnSettingsClick()
    { 
        SettingsUI.SetActive(true);
    }
    // void OnTutorialClick(){ }
    public void OnQuitClick()
    { 
        
    }
}
