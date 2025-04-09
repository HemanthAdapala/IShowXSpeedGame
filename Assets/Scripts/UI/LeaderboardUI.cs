using System;
using Data;
using DG.Tweening;
using Managers;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LeaderboardUI : MonoBehaviour, IScreenBase
    {
        [SerializeField] private Button backButton;
        [SerializeField] private LeaderboardItem leaderboardItemPrefab;
        [SerializeField] private LeaderboardItem playerStaticLeaderboardItemPrefab;
        [SerializeField] private Transform leaderboardContainer;
        
        [SerializeField] private MainMenuUI mainMenuUI;

        private void OnEnable()
        {
            LeaderboardManager.Instance.OnPlayerScoreUpdated += OnLeaderboardPlayerScoreUpdated;
            LeaderboardManager.Instance.OnLeaderboardUpdatedData += OnLeaderboardUpdatedData;
            backButton.onClick.AddListener(OnClickBackButton);
            LeaderboardManager.Instance.GetPlayerScore();
            LeaderboardManager.Instance.GetScores();
            ResetLeaderboardUI();
        }

        private void OnDisable()
        {
            backButton.onClick.RemoveListener(OnClickBackButton);
            LeaderboardManager.Instance.OnPlayerScoreUpdated -= OnLeaderboardPlayerScoreUpdated;
            LeaderboardManager.Instance.OnLeaderboardUpdatedData -= OnLeaderboardUpdatedData;
            ResetLeaderboardUI();
        }

        private void OnLeaderboardUpdatedData(LeaderboardScoresPage data)
        {
            UpdateLeaderBoardUI(data);
        }

        private float instantiateDelay = 0.1f;
        private void UpdateLeaderBoardUI(LeaderboardScoresPage data)
        {
            int entriesCount = data.Results.Count;
            int entriesToInstantiate = (entriesCount > 50) ? 50 : entriesCount;

            for (int i = 0; i < entriesToInstantiate; i++)
            {
                LeaderboardEntry leaderboardEntry = data.Results[i];
                LeaderboardItem leaderboardItem = Instantiate(leaderboardItemPrefab, leaderboardContainer);
                leaderboardItem.transform.localScale = Vector3.zero;

                float delay = instantiateDelay * i;
                leaderboardItem.transform.DOScale(1f, 0.5f).SetDelay(delay).SetEase(Ease.OutBack);
                leaderboardItem.SetData(leaderboardEntry.Rank, leaderboardEntry.PlayerName, (int)leaderboardEntry.Score);
            }
        }

        private void ResetLeaderboardUI()
        {
            foreach (Transform child in leaderboardContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnLeaderboardPlayerScoreUpdated(LeaderboardEntry obj)
        {
            SetPlayerBoardData(obj);
        }

        private void SetPlayerBoardData(LeaderboardEntry leaderboardEntry)
        {
            playerStaticLeaderboardItemPrefab.SetData(leaderboardEntry.Rank, leaderboardEntry.PlayerName, (int)leaderboardEntry.Score);
        }

        private void OnClickBackButton()
        {
            UIManager.Instance.GoBack();
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}
