using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject playerNameEnterPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitNameButton;

    private string _gameSceneName = "MainGameScene";

    private void Awake()
    {
        playButton.onClick.AddListener(OnClickPlayButton);
        submitNameButton.onClick.AddListener(OnSubmitName);
        playerNameEnterPanel.SetActive(false);
    }

    private void OnEnable()
    {
        CheckPlayerData();
    }

    private void CheckPlayerData()
    {
        if (GameManager.Instance.PlayerData == null)
        {
            if (!playerNameEnterPanel.activeInHierarchy)
            {
                playerNameEnterPanel.SetActive(true);   
            }
            playButton.interactable = false;
        }
        else
        {
            playerNameEnterPanel.SetActive(false);
            playButton.interactable = true;
        }
    }

    private void OnSubmitName()
    {
        string playerName = nameInputField.text.Trim();

        if (!string.IsNullOrEmpty(playerName))
        {
            GameManager.Instance.CreateNewPlayer(playerName);
            playerNameEnterPanel.SetActive(false);
            playButton.interactable = true;
        }
    }

    private void OnClickPlayButton()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    
}