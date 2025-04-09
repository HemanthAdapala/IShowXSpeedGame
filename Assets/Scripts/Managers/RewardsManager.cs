using System;
using Plugins;
using UI;
using UnityEngine;

namespace Managers
{
    public class RewardsManager : MonoBehaviour
    {
        public static RewardsManager Instance { get; set; }
        
        [SerializeField] private RewardsUI rewardsUI;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            GameManager.Instance.OnPlayerDataCoinsUpdatedEvent += UpdateCoins;
        }

        private void UpdateCoins(int coins)
        {
            rewardsUI.UpdateCoinsData(coins);
        }
    }
}
