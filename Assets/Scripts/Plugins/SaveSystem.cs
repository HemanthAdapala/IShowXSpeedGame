using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Player;
using UnityEngine;

namespace Plugins
{
    public static class SaveSystem
    {
        private static readonly string _savePath = Application.persistentDataPath + "/PlayerData.json";
        private static readonly string _encryptionKey = "SuperImportantKey"; // 🔥 Can be any length, will be hashed

        public static void SavePlayerData(PlayerData playerData)
        {
            string json = JsonUtility.ToJson(playerData);
            string hash = GenerateHash(json);
            string encryptedJson = Encrypt(json + "|" + hash);
            File.WriteAllText(_savePath, encryptedJson);
            Debug.Log("✅ Player Data Encrypted & Securely Saved!");
        }
        
        public static PlayerData LoadPlayerData()
        {
            if (File.Exists(_savePath))
            {
                string encryptedJson = File.ReadAllText(_savePath);
                string decryptedJson = Decrypt(encryptedJson);
        
                string[] parts = decryptedJson.Split('|');
                if (parts.Length < 2) return null;

                string jsonData = parts[0];
                string savedHash = parts[1];

                if (savedHash != GenerateHash(jsonData))
                {
                    Debug.LogWarning("⚠️ Save file was modified! Ignoring data.");
                    return null;
                }

                return JsonUtility.FromJson<PlayerData>(jsonData);
            }
            return null;
        }

        public static bool SaveExists()
        {
            return File.Exists(_savePath);
        }

        private static string Encrypt(string plainText)
        {
            byte[] keyBytes = GetHashedKey(_encryptionKey); // ✅ Use 32-byte key
            byte[] iv = new byte[16]; // AES requires 16-byte IV

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        private static string Decrypt(string encryptedText)
        {
            byte[] keyBytes = GetHashedKey(_encryptionKey); // ✅ Use the same 32-byte key
            byte[] iv = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        
        private static string GenerateHash(string data)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// Ensures the encryption key is exactly 32 bytes by hashing it.
        /// </summary>
        private static byte[] GetHashedKey(string key)
        {
            using (SHA256 sha = SHA256.Create()) // 🔥 Always creates a 32-byte key
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }
    }
}
