using System;
using Configs;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;
using Player;
using Plugins;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject playerNameEnterPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitNameButton;
    
    
    [Header("Lobby UI References")]
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xp;
    [SerializeField] private Slider xpSlider;
    
    [Header("LevelConfig")]
    [SerializeField] private LevelProgressionConfig levelConfig;

    private string _loadingScene = "LoadingScene";
    private PlayerData _playerData;


    private void OnEnable()
    {
        playButton.onClick.AddListener(OnClickPlayButton);
        submitNameButton.onClick.AddListener(OnSubmitName);
        playerNameEnterPanel.SetActive(false);
    }

    private void Start()
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
            lobbyPanel.SetActive(false);

        }
        else
        {
            _playerData = GameManager.Instance.PlayerData;
            playerNameEnterPanel.SetActive(false);
            lobbyPanel.SetActive(true);
            SetLobbyUIData();
        }
    }

    private void OnSubmitName()
    {
        string playerName = nameInputField.text.Trim();

        if (!string.IsNullOrEmpty(playerName))
        {
            GameManager.Instance.CreateNewPlayer(playerName);
            playerNameEnterPanel.SetActive(false);
            lobbyPanel.SetActive(true);
            playButton.interactable = true;
            _playerData = GameManager.Instance.PlayerData;
            SetLobbyUIData();
        }
    }
    
    private void SetLobbyUIData()
    {
        playerNameText.text = _playerData.playerName;
        levelText.text = _playerData.level.ToString();
        SetXpSliderData();
        bestScoreText.text = "BEST SCORE:- " + _playerData.bestScore;
    }

    private void SetXpSliderData()
    {
        var level = _playerData.level;
        var xpRequired = levelConfig.GetXpForLevel(level);
        xp.text = _playerData.xp + "/" + xpRequired;
        var nextLevel = levelConfig.GetNextLevel(level);
        xpSlider.value = (float)_playerData.xp / xpRequired;
    }

    private void OnClickPlayButton()
    {
        SceneLoader.UnloadScene("RewardsUIScene");
        SceneLoader.LoadScene(_loadingScene);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnClickPlayButton);
        submitNameButton.onClick.RemoveListener(OnSubmitName);
    }
}