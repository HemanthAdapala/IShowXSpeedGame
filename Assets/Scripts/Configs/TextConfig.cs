using UnityEngine;

[CreateAssetMenu(fileName = "TextConfig", menuName = "Game/TextConfig")]
public class TextConfig : ScriptableObject
{
    [Header("Encouragement Messages for Streak Gains")]
    public string[] continuousStreakMessages =
    {
        "You're on fire!",
        "Unstoppable! +{value}",
        "You're gaining like a pro! +{value}",
        "Legendary moves! +{value}",
        "You're leveling up in style! +{value}"
    };

    [Header("Messages for Streak Loss")]
    public string[] lostStreakMessages =
    {
        "Oops! You lost the streak!",
        "The comeback is always stronger!",
        "Shake it off! Next one’s legendary!",
        "Even pros miss sometimes!"
    };

    public string GetRandomStreakMessage()
    {
        if (Random.value < 0.2f) // 20% chance for special message
        {
            return continuousStreakMessages[Random.Range(0, continuousStreakMessages.Length)];
        }
        return "+{value}"; // Default text
    }

    public string GetRandomLostMessage()
    {
        if (Random.value < 0.2f) // 20% chance for special message
        {
            return lostStreakMessages[Random.Range(0, lostStreakMessages.Length)];
        }
        return "Streak Lost!"; // Default text
    }

    public string GetRandomContinuousMessage()
    {
        return continuousStreakMessages[Random.Range(0, continuousStreakMessages.Length)];
    }
}