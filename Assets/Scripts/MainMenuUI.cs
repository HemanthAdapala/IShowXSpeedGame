using System;
using Configs;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;
using Player;
using Plugins;
using UI;

public class MainMenuUI : MonoBehaviour, IScreenBase
{
    [Header("UI References")]
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject playerNameEnterPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitNameButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private LeaderboardUI leaderboardPanel;
    [SerializeField] private ShopUI shopUIPanel;
    
    
    [Header("Lobby UI References")]
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xp;
    [SerializeField] private Slider xpSlider;
    
    [Header("LevelConfig")]
    [SerializeField] private LevelProgressionConfig levelConfig;

    private readonly string _loadingScene = "LoadingScene";
    private PlayerData _playerData;

    private void Awake()
    {
        if (leaderboardPanel.gameObject.activeInHierarchy)
        {
            leaderboardPanel.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        playButton.onClick.AddListener(OnClickPlayButton);
        leaderboardButton.onClick.AddListener(OnClickLeaderboardButton);
        shopButton.onClick.AddListener(OnClickShopButton);
        if (playerNameEnterPanel.activeInHierarchy)
        {
            playerNameEnterPanel.SetActive(false);
        }
    }

    private void OnClickShopButton()
    {
        UIManager.Instance.NavigateTo(shopUIPanel);
    }

    private void OnClickLeaderboardButton()
    {
        UIManager.Instance.NavigateTo(leaderboardPanel);
    }

    private void Start()
    {
        CheckPlayerData();
        UIManager.Instance.NavigateTo(this);
    }

    private void CheckPlayerData()
    {
        if (GameManager.Instance.PlayerData == null)
        {
            if (!playerNameEnterPanel.activeInHierarchy)
            {
                playerNameEnterPanel.SetActive(true);   
                submitNameButton.onClick.AddListener(OnSubmitName);
            }
            lobbyPanel.SetActive(false);

        }
        else
        {
            _playerData = SaveSystem.LoadPlayerData();
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
            SceneLoader.LoadScene("RewardsUIScene",true);
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
        int currentLevel = _playerData.level;
        int currentXp = _playerData.xp;

        // Get XP requirements
        int prevXpRequired = (currentLevel > 1) ? levelConfig.GetXpForLevel(currentLevel - 1) : 0;
        int xpRequired = levelConfig.GetXpForLevel(currentLevel);

        // Ensure XP values are valid
        if (xpRequired <= prevXpRequired)
        {
            Debug.LogError($"Invalid XP range for level {currentLevel}: Prev={prevXpRequired}, Required={xpRequired}");
            return;
        }

        // Update XP text (current XP / XP needed for next level)
        xp.text = $"{currentXp} / {xpRequired}";

        // Calculate slider value as a percentage of progress in the current level
        float sliderValue = (currentXp - prevXpRequired) / (float)(xpRequired - prevXpRequired);
        xpSlider.value = Mathf.Clamp01(sliderValue); // Ensure value is between 0 and 1

        Debug.Log($"XP: {currentXp}, Level: {currentLevel}, Prev XP: {prevXpRequired}, Required XP: {xpRequired}, Slider Value: {sliderValue}");
    }


    private void OnClickPlayButton()
    {
        SceneLoader.LoadScene(_loadingScene);
        SceneLoader.UnloadScene("RewardsUIScene");
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnClickPlayButton);
        submitNameButton.onClick.RemoveListener(OnSubmitName);
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}