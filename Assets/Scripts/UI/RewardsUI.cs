using System.Collections;
using DG.Tweening;
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

        public void OnEnable()
        {
            _playerData = GameManager.Instance.GetPlayerData();
            if (_playerData is not null)
            {
                coins.text = _playerData.coins.ToString();
            }
        }

        public void UpdateCoinsData(int updatedCoins)
        {
            int startCoins = _playerData.coins;
            int displayedCoins = startCoins;

            DOTween.To(() => displayedCoins, x => displayedCoins = x, updatedCoins, 2f)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    coins.text = $"{displayedCoins}";
                })
                .OnComplete(() =>
                {
                    coins.text = $"gle{updatedCoins}";
                    // Update player data now if needed
                    _playerData.coins = updatedCoins;
                });
        }

    }
}
