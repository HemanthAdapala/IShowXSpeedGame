using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PersistentDataPathTool : EditorWindow
    {
        private static string playerDataPath = Application.persistentDataPath + "/PlayerData.json";

        [MenuItem("Tools/Open Persistent Data Path")]
        public static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("Tools/Delete Player Data File")]
        public static void DeletePlayerDataFile()
        {
            if (File.Exists(playerDataPath))
            {
                File.Delete(playerDataPath);
                Debug.Log("✅ PlayerData.json deleted successfully!");
            }
            else
            {
                Debug.LogWarning("⚠️ No PlayerData.json file found to delete.");
            }
        }
    }
}