using System;
using Data;
using Player;
using Plugins;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public PlayerData PlayerData { get; private set; }
        private GameSessionData GameSessionData { get; set; }

        private readonly string _rewardsUIScene = "RewardsUIScene";


        private async void Awake()
        {
            try
            {
                if (Instance == null)
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                    LoadPlayerData();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            catch (Exception e)
            {
                // TODO handle exception
                Debug.Log("Error: " + e);
            }
        }


        private void LoadPlayerData()
        {
            if (SaveSystem.SaveExists())
            {
                PlayerData = SaveSystem.LoadPlayerData();
                SceneLoader.LoadScene(_rewardsUIScene, true);
                Debug.Log("✅ Player Data Loaded.");
            }
            else
            {
                Debug.LogWarning("⚠️ No Save File Found.");
                PlayerData = null;
            }
        }

        public void CreateNewPlayer(string playerName)
        {
            var initialPlayerCoins = 1000;
            PlayerData = new PlayerData(playerName, "Clarie", 1, 0, 1.0f, 0, 0, 0.0F, initialPlayerCoins);
            LeaderboardManager.Instance.AddScore(PlayerData.bestScore);
            SavePlayerData();
        }

        private void SavePlayerData()
        {
            if (PlayerData != null)
            {
                SaveSystem.SavePlayerData(PlayerData);
                PlayerData = SaveSystem.LoadPlayerData();
                Debug.Log("✅ Player Data Saved.");
            }
        }

        public void SaveGameSession()
        {
            GameSessionManager.Instance.SaveRequiredInfoForGameSessionAtEnd();
            GameSessionData = GamePlayManager.Instance.GetGameSessionData();
            Debug.Log("✅ Final Game Session Data Saved.");
        }

        public void SetUpdatedPlayerData(PlayerData playerData)
        {
            PlayerData = playerData;
        }

        public GameSessionData GetGameSessionData()
        {
            return GameSessionData;
        }

        public PlayerData GetPlayerData()
        {
            return PlayerData;
        }
    }
}