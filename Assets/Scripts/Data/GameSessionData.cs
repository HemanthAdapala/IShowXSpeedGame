namespace Data
{
    public class GameSessionData
    {
        public int Score;
        public int MaxStreak;
        public float MaxMultiplier;
        public int SessionXp;
        public int SessionCoins;
        public int TotalXp;
        public int TotalCoins;

        public GameSessionData(int score, int maxStreak, float maxMultiplier, int sessionXp,int sessionCoins)
        {
            Score = score;
            MaxStreak = maxStreak;
            MaxMultiplier = maxMultiplier;
            SessionXp = sessionXp;
            SessionCoins = sessionCoins;
        }
    }
}