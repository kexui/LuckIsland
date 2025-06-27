using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUIManager : MonoBehaviour
{
    public static LoadingUIManager Instance { get; private set; }
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        Instance = this;
    }

    public void Show()
    { 
        loadingPanel.SetActive(true);
        
    }
    public void Hide()
    { 
        loadingPanel.SetActive(false);
    }
    public void SetPressgress(float progress)
    {
        if (slider != null)
        {
            slider.value = progress;
        }
    }
}
