using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    MainMenu,
    GameScene,
    GameOverScene
}
public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    { 
        SceneManager.LoadScene(sceneName);
    }
    public void LoadNectScene()
    { 
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index+1);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(SceneName.MainMenu.ToString());
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene(SceneName.GameScene.ToString());
        StartCoroutine(LoadSceneAsymc(SceneName.GameScene));
    }
    public void QuitGame()
    { 
        Application.Quit();
    }
    public IEnumerator LoadSceneAsymc(SceneName sceneName)
    { 
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName.ToString());
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
    }
}
