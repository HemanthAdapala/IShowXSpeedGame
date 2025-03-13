using System;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static Action<int> OnLevelUp;
    public static Action<int> OnExperienceGained;

    public static void AddExperience(int baseXP)
    {
        int xpGain = Mathf.RoundToInt(baseXP * PlayerProfileSystem.playerData.experienceMultiplier);
        PlayerProfileSystem.playerData.experience += xpGain;
        OnExperienceGained?.Invoke(xpGain);
        CheckLevelUp();
    }

    private static void CheckLevelUp()
    {
        while (PlayerProfileSystem.playerData.experience >= PlayerProfileSystem.playerData.RequiredExperience)
        {
            PlayerProfileSystem.playerData.experience -= PlayerProfileSystem.playerData.RequiredExperience;
            PlayerProfileSystem.playerData.level++;
            OnLevelUp?.Invoke(PlayerProfileSystem.playerData.level);
        }
        PlayerProfileSystem.SavePlayerData();
    }
}
