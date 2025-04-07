namespace Data
{
    public class GameSessionData
    {
        public int Score;
        public int CurrentStreak;
        public int MaxStreak;
        public float MaxMultiplier;
        public float CurrentMultiplier;
        public int SessionXp;
        public int SessionCoins;
        public int TotalXp;
        public int TotalCoins;

        public GameSessionData(int score,int currentStreak, int maxStreak, float maxMultiplier, float currentMultiplier ,int sessionXp,int sessionCoins)
        {
            Score = score;
            CurrentStreak = currentStreak;
            MaxStreak = maxStreak;
            MaxMultiplier = maxMultiplier;
            CurrentMultiplier = currentMultiplier;
            SessionXp = sessionXp;
            SessionCoins = sessionCoins;
        }
    }
}