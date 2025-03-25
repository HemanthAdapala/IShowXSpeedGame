using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnimationsDataConfig", menuName = "Configs/PlayerAnimationsConfig")]
public class PlayerAnimationsDataConfig : ScriptableObject
{
    [Header("Jump Animation Parameters")]
    public string[] JumpTrigger;

    [Header("Celebration Animation Parameters")]
    public string[] CelebrationAnimationTriggers;

    public string GetRandomJumpAnimationTrigger()
    {
        return JumpTrigger[Random.Range(0, JumpTrigger.Length)];
    }

    public string GetRandomCelebrationAnimationTrigger()
    {
        return CelebrationAnimationTriggers[Random.Range(0, CelebrationAnimationTriggers.Length)];
    }
}
