using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerProfileSystem : MonoBehaviour
    {
        private const string PLAYER_DATA_KEY = "PlayerData";
        public static PlayerData PlayerData;

        private void Awake()
        {
            LoadPlayerData();
        }

        private void LoadPlayerData()
        {
            if (PlayerPrefs.HasKey(PLAYER_DATA_KEY))
            {
                PlayerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(PLAYER_DATA_KEY));
            }
            else
            {
                PlayerData = new PlayerData { playerName = GenerateRandomName(6, 12) };
                SavePlayerData();
            }
        }

        private string GenerateRandomName(int minLength, int maxLength)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int length = UnityEngine.Random.Range(minLength, maxLength);
            char[] nameArray = new char[length];
            for (int i = 0; i < length; i++)
                nameArray[i] = chars[UnityEngine.Random.Range(0, chars.Length)];
            return new string(nameArray);
        }

        public static void SavePlayerData()
        {
            PlayerPrefs.SetString(PLAYER_DATA_KEY, JsonUtility.ToJson(PlayerData));
            PlayerPrefs.Save();
        }
    }
}