using Configs;
using Data;
using Player;
using UnityEngine;

namespace Managers
{
    public class GameSessionManager : MonoBehaviour
    {
        #region SINGLETON

        private static GameSessionManager _instance;

        public static GameSessionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<GameSessionManager>();
                }
                return _instance;
            }
        }

        #endregion
        
        
        private GameSessionData _gameSessionData;
        
        public GameSessionData GameSessionData => _gameSessionData;

        private PlayerData _playerData;

        private void OnEnable()
        {
            _playerData = GameManager.Instance.GetPlayerData();
            _gameSessionData = new GameSessionData(0, 0, 0, 0, 0);
            GameEventManager.OnStreakUpdated += OnStreakUpdated_GameEventManager;
            GameEventManager.OnScoreUpdated += OnScoreUpdated_GameEventManager;
            GameEventManager.OnMultiplierUpdated += OnMultiplierUpdated_GameEventManager;
            GameEventManager.OnSuccessfulJumpWithVehicleRewardConfig += AwardXpAndCoins;
        }

        private void OnDisable()
        {
            GameEventManager.OnStreakUpdated -= OnStreakUpdated_GameEventManager;
            GameEventManager.OnScoreUpdated -= OnScoreUpdated_GameEventManager;
            GameEventManager.OnMultiplierUpdated -= OnMultiplierUpdated_GameEventManager;
            GameEventManager.OnSuccessfulJumpWithVehicleRewardConfig -= AwardXpAndCoins;
        }
        
        private void OnStreakUpdated_GameEventManager(int value)
        {
            SaveMaxStreak(value);
        }
        
        private void SaveMaxStreak(int currentStreak)
        {
            if(currentStreak >= _gameSessionData.MaxStreak) _gameSessionData.MaxStreak = currentStreak;
        }
        
        private void OnScoreUpdated_GameEventManager(int value)
        {
            _gameSessionData.Score = value;
        }
        
        private void OnMultiplierUpdated_GameEventManager(float value)
        {
            SaveMaxMultiplier(value);
        }
        
        private void SaveMaxMultiplier(float currentMultiplier)
        {
            if(currentMultiplier >= _gameSessionData.MaxMultiplier) _gameSessionData.MaxMultiplier = currentMultiplier;
        }

        private void AwardXpAndCoins(VehicleDataRewardConfig vehicleRewardConfig)
        {
            if (vehicleRewardConfig == null) return;

            var vehicleReward = vehicleRewardConfig.vehicleRewardData;

            _gameSessionData.SessionXp += vehicleReward.xp;
            _gameSessionData.SessionCoins += vehicleReward.coins;
        }

        public void SaveRequiredInfoForGameSessionAtEnd()
        {
            SaveTotalCoins();
            SaveTotalXp();
        }
        
        private void SaveTotalCoins()
        {
            _gameSessionData.TotalCoins = _gameSessionData.SessionCoins + _playerData.coins;
        }

        private void SaveTotalXp()
        {
            _gameSessionData.TotalXp = _gameSessionData.SessionXp + _playerData.xp;
        }
    }
}
