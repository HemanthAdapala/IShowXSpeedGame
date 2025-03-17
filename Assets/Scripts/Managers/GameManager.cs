using Player;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public PlayerData PlayerData { get; private set; }

        private const string PlayerDataKey = "PlayerData"; // Key for PlayerPrefs storage

        private void Awake()
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

        private void LoadPlayerData()
        {
            if (PlayerPrefs.HasKey(PlayerDataKey))
            {
                PlayerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(PlayerDataKey));
                Debug.Log($"Loaded Player Data: {PlayerData.playerName}");
            }
            else
            {
                PlayerData = null; // No data found
            }
        }

        public void CreateNewPlayer(string playerName)
        {
            PlayerData = new PlayerData(playerName, "Clarie", 0, 0, 1.0f, 0);
            SavePlayerData();
        }

        private void SavePlayerData()
        {
            if (PlayerData != null)
            {
                PlayerPrefs.SetString(PlayerDataKey, JsonUtility.ToJson(PlayerData));
                PlayerPrefs.Save();
                Debug.Log("Player Data Saved.");
            }
        }
    }
}