using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "StreakConfig", menuName = "Configs/StreakConfig")]
    public class StreakConfig : ScriptableObject
    {
        [Header("Streak Milestones")]
        public int[] milestoneThresholds = {5, 10, 15, 20, 25};

        
        [Header("Score Multipliers")]
        public float[] multipliers = {1.0f, 1.2f, 1.5f, 1.8f, 2.0f, 2.5f, 3.0f, 3.5f, 4.0f, 5.0f};


        public float GetMultiplier(int streakCount)
        {
            for (int i = milestoneThresholds.Length - 1; i >= 0; i--)
            {
                if (streakCount >= milestoneThresholds[i])
                {
                    int safeIndex = Mathf.Min(i, multipliers.Length - 1); // Prevent out-of-bounds access
                    return multipliers[safeIndex];
                }
            }
    
            return 1.0f; // Default multiplier
        }
        
        public float GetInitialMultiplier()
        {
            return multipliers[0];
        }

    }
}
