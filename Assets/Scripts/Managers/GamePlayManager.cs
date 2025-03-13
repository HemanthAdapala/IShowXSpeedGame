using System;
using System.Collections;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static Action<float> OnMultiplierUpdated;
    public static Action<int> OnStreakUpdated;
    private float multiplierGrowth = 0.1f;
    private float maxMultiplier = 2.0f;
    private float decayTime = 3f;
    private Coroutine decayCoroutine;

    public void IncreaseStreak()
    {
        PlayerProfileSystem.playerData.streakCounter++;
        PlayerProfileSystem.playerData.experienceMultiplier = Mathf.Min(
            1.0f + (PlayerProfileSystem.playerData.streakCounter * multiplierGrowth), maxMultiplier);

        OnStreakUpdated?.Invoke(PlayerProfileSystem.playerData.streakCounter);
        OnMultiplierUpdated?.Invoke(PlayerProfileSystem.playerData.experienceMultiplier);

        if (decayCoroutine != null) StopCoroutine(decayCoroutine);
        decayCoroutine = StartCoroutine(MultiplierDecay());
    }

    public void ResetStreak()
    {
        PlayerProfileSystem.playerData.streakCounter = 0;
        PlayerProfileSystem.playerData.experienceMultiplier = 1.0f;
        OnStreakUpdated?.Invoke(0);
        OnMultiplierUpdated?.Invoke(1.0f);
    }

    private IEnumerator MultiplierDecay()
    {
        yield return new WaitForSeconds(decayTime);
        ResetStreak();
    }
}
