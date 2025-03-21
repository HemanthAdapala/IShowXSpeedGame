using System;
using Configs;
using DG.Tweening;
using Managers;
using Player;
using Plugins;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOverLevelUpPanelUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI levelUpText;
        [SerializeField] private LevelProgressionConfig levelProgressionConfig;
        [SerializeField] private Button continueButton;

        private PlayerData _playerData;
        private int _targetXP; // Total XP earned in session
        private int _remainingXP; // XP left to process
        private int _currentLevel;
        private bool _isAnimating = false;

        private string _lobbyScene = "LobbyScene";
        private string _gameOverScene = "GameOverScene";

        public void SetUpUIData()
        {
            continueButton.onClick.AddListener(OnClickContinueButton);
            _playerData = GameManager.Instance.GetPlayerData();
            _targetXP = GameManager.Instance.GetGameSessionData().SessionXp;
            _currentLevel = _playerData.level;
            _remainingXP = _targetXP;

            SetPrePlayerData();
            StartXPAnimation();
        }

        private void OnClickContinueButton()
        {
            SceneLoader.LoadScene("RewardsUIScene",true);
            SceneLoader.LoadScene(_lobbyScene,true);
            SceneLoader.UnloadScene(_gameOverScene);
        }

        private void SetPrePlayerData()
        {
            slider.value = (float)_playerData.xp / levelProgressionConfig.GetXpForLevel(_playerData.level);
            levelUpText.text = _currentLevel.ToString();
        }

        private void StartXPAnimation()
        {
            if (_isAnimating) return; // Prevent multiple animations
            _isAnimating = true;
            ProcessXPIncrement();
        }

        private void ProcessXPIncrement()
        {
            if (_remainingXP <= 0)
            {
                _isAnimating = false;
                return;
            }

            int xpRequired = levelProgressionConfig.GetXpForLevel(_currentLevel);
            int xpToNextLevel = xpRequired - _playerData.xp;
            int xpGain = Mathf.Min(_remainingXP, xpToNextLevel); // Ensure we don’t exceed level cap

            // Animate XP Bar
            float targetValue = (_playerData.xp + xpGain) / (float)xpRequired;
            slider.DOValue(targetValue, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                _playerData.xp += xpGain;
                _remainingXP -= xpGain;

                if (_playerData.xp >= xpRequired)
                {
                    // Level Up!
                    _playerData.xp = 0; // Reset XP
                    _currentLevel++;
                    levelUpText.text = _currentLevel.ToString();
                    slider.value = 0; // Reset slider instantly
                }

                ProcessXPIncrement(); // Continue XP animation
            });
        }

        private void OnDisable()
        {
            continueButton.onClick.RemoveListener(OnClickContinueButton);
        }
    }
}
