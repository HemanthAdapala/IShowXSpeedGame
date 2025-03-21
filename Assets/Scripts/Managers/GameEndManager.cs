using Configs;
using Data;
using Player;
using Plugins;
using UI;
using UnityEngine;

namespace Managers
{
    public class GameEndManager : MonoBehaviour
    {
        private GameSessionData _sessionData;
        private PlayerData _playerData;
        [SerializeField] private GameOverLevelUpPanelUI gameOverLevelUpPanelUI;
        [SerializeField] private LevelProgressionConfig levelProgressionConfig;
        private void Start()
        {
            _playerData = SaveSystem.LoadPlayerData();
            _sessionData = GameManager.Instance.GetGameSessionData();

            // Get game session data
            int score = _sessionData.Score;
            int maxStreak = _sessionData.MaxStreak;
            float maxMultiplier = _sessionData.MaxMultiplier;
            int sessionDataXpEarned = _sessionData.SessionXp;
            int sessionDataEarnedCoins = _sessionData.SessionCoins;
            int totalCoins = _sessionData.TotalCoins;
            int totalXp = _sessionData.TotalXp;
            
            Debug.Log("Score: " + score);
            Debug.Log("Max Streak: " + maxStreak);
            Debug.Log("Max Multiplier: " + maxMultiplier);
            Debug.Log("XP: " + sessionDataXpEarned);
            Debug.Log("Coins: " + sessionDataEarnedCoins);
            Debug.Log("Total Coins: " + totalCoins);
            Debug.Log("Total XP: " + totalXp);
            
            
            //Calculate PlayerData using SessionData
            int[] levelsSurpassed = levelProgressionConfig.GetLevelsSurpassed(_playerData.xp, totalXp);
            int level = levelProgressionConfig.GetLevelForXp(sessionDataXpEarned);
            gameOverLevelUpPanelUI.SetUpUIData();
            
            
            Debug.Log("Levels Surpassed: " + string.Join(", ", levelsSurpassed));
            Debug.Log("Level: " + level);
            
            SaveGameSessionDataToPlayerData();
        }

        private void SaveGameSessionDataToPlayerData()
        {
            if (_playerData != null)
            {
                _playerData.bestScore = _sessionData.Score;
                _playerData.bestStreak = _sessionData.MaxStreak;
                _playerData.bestMultiplier = _sessionData.MaxMultiplier;
                _playerData.xp = _sessionData.SessionXp;
                _playerData.coins = _sessionData.TotalCoins;
                _playerData.level = levelProgressionConfig.GetLevelForXp(_sessionData.TotalXp);
                
                SaveSystem.SavePlayerData(_playerData);
            }
        }
        
    }
}
