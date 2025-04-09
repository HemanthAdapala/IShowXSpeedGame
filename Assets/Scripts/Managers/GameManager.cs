using System;
using System.Collections.Generic;
using Data;
using NUnit.Framework;
using Player;
using Plugins;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public PlayerData PlayerData { get; private set; }
        public List<BaseVehicleDataUI> BaseVehicleDataUI { get; private set; }
        private GameSessionData GameSessionData { get; set; }

        private readonly string _rewardsUIScene = "RewardsUIScene";

        public Action<int> OnPlayerDataCoinsUpdatedEvent;


        private async void Awake()
        {
            try
            {
                if (Instance == null)
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                    LoadPlayerData();
                    LoadShopPurchasedData();
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

        private void LoadShopPurchasedData()
        {
            if (SaveSystem.SaveExists(SaveSystem._saveShopDataPath))
            {
                BaseVehicleDataUI = SaveSystem.LoadShopData();
                if (BaseVehicleDataUI == null || BaseVehicleDataUI.Count == 0)
                {
                    Debug.LogWarning("⚠️ No valid Shop Data Found. Creating new data.");
                    CreateNewShopData();
                }
                else
                {
                    Debug.Log("✅ Shop Data Loaded Successfully.");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ No Shop Data Found. Creating new data.");
                BaseVehicleDataUI = null;
            }
        }

        private void LoadPlayerData()
        {
            if (SaveSystem.SaveExists(SaveSystem._savePlayerDataPath))
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
            var initialPlayerCoins = 5000;
            PlayerData = new PlayerData(playerName, "Clarie", 1, 0, 1.0f, 0, 0, 0.0F, initialPlayerCoins);
            CreateNewShopData();
            LeaderboardManager.Instance.AddScore(PlayerData.bestScore);
            SavePlayerData();
        }

        public void CreateNewShopData()
        {
            BaseVehicleDataUI = new List<BaseVehicleDataUI>();
            BaseVehicleDataUI baseVehicleData = new BaseVehicleDataUI
            {
                vehicleId = 1,
                isPurchased = true,
                isUnlocked = true
            };
            BaseVehicleDataUI.Add(baseVehicleData);
            SaveShopUIData();
            Debug.Log("✅ Shop Data Created.");
        }

        private void SavePlayerData()
        {
            if (PlayerData != null)
            {
                SaveSystem.SavePlayerData(PlayerData);
                PlayerData = SaveSystem.LoadPlayerData();
                Debug.Log("Player Data Saved.");
            }
        }

        private void SaveShopUIData()
        {
            if(BaseVehicleDataUI != null)
            {
                SaveSystem.SaveShopData(BaseVehicleDataUI);
                BaseVehicleDataUI = SaveSystem.LoadShopData();
                Debug.Log("Shop Data Saved.");
            }
        }

        public void SaveGameSession()
        {
            GameSessionManager.Instance.SaveRequiredInfoForGameSessionAtEnd();
            GameSessionData = GamePlayManager.Instance.GetGameSessionData();
            Debug.Log("Final Game Session Data Saved.");
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

        public List<BaseVehicleDataUI> GetBaseVehicleDataUI()
        {
            return BaseVehicleDataUI;
        }

        public int GetPlayerCoins()
        {
            return PlayerData.coins;
        }

        public bool PurchaseVehicleItem(VehicleUIItemData vehicleData)
        {
            if(PlayerData.coins >= vehicleData.VehiclePrice)
            {
                //Decrease the player Coins 
                PlayerData.coins -= vehicleData.VehiclePrice;
                OnPlayerDataCoinsUpdatedEvent?.Invoke(PlayerData.coins);
                SavePlayerData();
                BaseVehicleDataUI newVehicleData = new BaseVehicleDataUI()
                {
                    vehicleId = vehicleData.vehicleId,
                    isPurchased = true,
                    isUnlocked = true
                };
                BaseVehicleDataUI.Add(newVehicleData);
                SaveShopUIData();
                return true;
            }
            return false;
        }
    }
}