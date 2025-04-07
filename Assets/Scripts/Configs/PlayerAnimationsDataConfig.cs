using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnimationsDataConfig", menuName = "Configs/PlayerAnimationsConfig")]
public class PlayerAnimationsDataConfig : ScriptableObject
{
    [Header("Jump Animation Parameters")]
    public string[] JumpTrigger;

    [Header("Celebration Animation Parameters")]
    public string[] CelebrationAnimationTriggers;

    [Header("Death Animation Parameters")]
    public string[] DeathAnimationTriggers;

    public string GetRandomJumpAnimationTrigger()
    {
        return JumpTrigger[Random.Range(0, JumpTrigger.Length)];
    }

    public string GetRandomCelebrationAnimationTrigger()
    {
        return CelebrationAnimationTriggers[Random.Range(0, CelebrationAnimationTriggers.Length)];
    }

    public string GetRandomDeathAnimationTrigger()
    {
        return DeathAnimationTriggers[Random.Range(0, DeathAnimationTriggers.Length)];
    }
}
