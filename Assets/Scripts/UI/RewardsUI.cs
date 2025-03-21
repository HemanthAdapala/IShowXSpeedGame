using Managers;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class RewardsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coins;
        [SerializeField] private TextMeshProUGUI gems;

        private PlayerData _playerData;

        public void SetRewardsPanelUIData()
        {
            _playerData = GameManager.Instance.GetPlayerData();
            if (_playerData is not null)
            {
                coins.text = _playerData.coins.ToString();
            }
        }
    }
}
