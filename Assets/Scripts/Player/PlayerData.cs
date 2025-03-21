namespace Player
{
    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public string playerCurrentCharacterName;
        public int level = 0;
        public int xp = 0;
        public float experienceMultiplier = 1.0f;
        public int bestScore = 0;
        public int bestStreak = 0;
        public float bestMultiplier = 1.0f;
        
        //Rewards
        public int coins = 0;

        public int RequiredExperience => (level + 1) * 10;

        // ✅ Default constructor for JSON serialization
        public PlayerData() { }

        // ✅ Custom constructor for new players
        public PlayerData(string playerName, string playerCurrentCharacterName, int level, int xp, float experienceMultiplier, int bestScore = 0, int bestStreak = 0, float bestMultiplier = 1.0f, int coins = 0)
        {
            this.playerName = playerName;
            this.playerCurrentCharacterName = playerCurrentCharacterName;
            this.level = level;
            this.xp = xp;
            this.experienceMultiplier = experienceMultiplier;
            this.bestScore = bestScore;
            this.bestStreak = bestStreak;
            this.bestMultiplier = bestMultiplier;
            this.coins = coins;
        }
    }
}