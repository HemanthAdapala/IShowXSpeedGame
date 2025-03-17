using System;
using Player;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static Action<int> OnLevelUp;
    public static Action<int> OnExperienceGained;

    public static void AddExperience(int baseXP)
    {
        int xpGain = Mathf.RoundToInt(baseXP * PlayerProfileSystem.PlayerData.experienceMultiplier);
        PlayerProfileSystem.PlayerData.experience += xpGain;
        OnExperienceGained?.Invoke(xpGain);
        CheckLevelUp();
    }

    private static void CheckLevelUp()
    {
        while (PlayerProfileSystem.PlayerData.experience >= PlayerProfileSystem.PlayerData.RequiredExperience)
        {
            PlayerProfileSystem.PlayerData.experience -= PlayerProfileSystem.PlayerData.RequiredExperience;
            PlayerProfileSystem.PlayerData.level++;
            OnLevelUp?.Invoke(PlayerProfileSystem.PlayerData.level);
        }
        PlayerProfileSystem.SavePlayerData();
    }
}
