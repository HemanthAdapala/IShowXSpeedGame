using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    [System.Serializable]
    public class DifficultyLevel
    {
        public int MinScore; // Minimum score for this difficulty level
        public int MaxScore; // Maximum score for this difficulty level
        public float MinSpawnInterval; // Minimum spawn interval for cubes
        public float MaxSpawnInterval; // Maximum spawn interval for cubes
        public float CubeMoveSpeed; // Speed of the cubes
    }

    public DifficultyLevel[] DifficultyLevels; // Array of difficulty levels

    // Get the difficulty level based on the current score
    public DifficultyLevel GetDifficultyLevel(int score)
    {
        foreach (var level in DifficultyLevels)
        {
            if (score >= level.MinScore && score <= level.MaxScore)
            {
                return level;
            }
        }

        // Return the last level if no match is found
        return DifficultyLevels[DifficultyLevels.Length - 1];
    }
}