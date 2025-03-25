using TMPro;
using UnityEngine;

namespace Data
{
    public class LeaderboardItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI rank;
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI bestScore;
        
        public void SetData(int rank, string playerName, int bestScore)
        {
            this.rank.text = (rank+1).ToString();
            this.playerName.text = playerName;
            this.bestScore.text = bestScore.ToString();
        }
    }
}
