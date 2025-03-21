using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(fileName = "LevelProgressionConfig", menuName = "Configs/Level Progression")]
    public class LevelProgressionConfig : ScriptableObject
    {
        [System.Serializable]
        public class LevelData
        {
            public int level;
            public int xpRequired;
        }

        public LevelData[] levels;

        public int GetXpForLevel(int level)
        {
            foreach (var lvl in levels)
            {
                if (lvl.level == level)
                {
                    return lvl.xpRequired;
                }
            }
            return levels.Length > 0 ? levels[^1].xpRequired : 100; // Default if level is out of bounds
        }

        public int GetNextLevel(int currentLevel)
        {
            if (currentLevel < levels.Length)
            {
                return currentLevel + 1; // Next level is the one after the current one
            }
            else if (currentLevel == levels.Length)
            {
                return currentLevel; // If we reached the last level, stay there
            }
            else
            {
                Debug.LogError("Level out of bounds: " + currentLevel);
                return 1; // Default level if the current level is out of bounds
            }
        }

        public int GetLevelForXp(int xp)
        {
            if (levels == null || levels.Length == 0)
            {
                Debug.LogError("Level data is empty!");
                return 1;
            }

            int level = 1; // Default to level 1

            for (int i = 0; i < levels.Length; i++)
            {
                if (xp < levels[i].xpRequired)
                {
                    level = levels[i].level;
                    return level; // Return the last valid level
                }
            }

            return levels[^1].level; // If XP exceeds all levels, return the max level
        }


        
        public int[] GetLevelsSurpassed(int currentXp, int totalXp)
        {
            List<int> surpassedLevels = new List<int>();

            int newXp = currentXp;
            int currentLevel = GetLevelForXp(newXp);

            while (newXp < totalXp)
            {
                int requiredXp = GetXpForLevel(currentLevel + 1);
        
                if (newXp >= requiredXp)
                {
                    surpassedLevels.Add(currentLevel + 1);
                    currentLevel++;
                }
                else
                {
                    break;
                }
            }

            return surpassedLevels.ToArray();
        }



        // Method to Set the XP required for each level
        public void RandomizeValues()
        {
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i].level = i + 1;
                levels[i].xpRequired = levels[i].level * 1000; // Random XP required for each level
                levels[i].xpRequired -= 1; // Random XP required for each level
            }
        }

        
    }
}