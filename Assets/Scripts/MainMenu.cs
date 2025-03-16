using System;
using Plugins;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    
    private string _loadingSceneName = "LoadingScene";

    private void Awake()
    {
        playButton.onClick.AddListener(OnClickPlayButton);
    }

    private void OnClickPlayButton()
    {
        SceneLoader.LoadScene(_loadingSceneName);
    }
}
