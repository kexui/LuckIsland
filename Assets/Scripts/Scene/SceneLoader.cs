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
    public static SceneLoader Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public void LoadSceneByName(SceneName sceneName)
    { 
        SceneManager.LoadScene(sceneName.ToString());
    }
    public void LoadNextScene()
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
        LoadingUIManager.Instance.Show(); // ��ʾ���ؽ���
        StartCoroutine(LoadSceneAsymc(SceneName.GameScene));
    }
    public void QuitGame()
    { 
        Application.Quit();
    }
    public IEnumerator LoadSceneAsymc(SceneName sceneName)
    { 
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName.ToString());
        operation.allowSceneActivation = false;
        while (operation.progress<0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingUIManager.Instance.SetPressgress(progress);
            yield return new WaitForSeconds(0.1f);
        }
        LoadingUIManager.Instance.SetPressgress(1f);
        yield return new WaitForSeconds(1f); // �ȴ�1��������ʾ������ɵĽ���
        operation.allowSceneActivation = true;
    }
}
