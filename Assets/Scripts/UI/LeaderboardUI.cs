using System;
using Data;
using DG.Tweening;
using Managers;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private LeaderboardItem leaderboardItemPrefab;
        [SerializeField] private LeaderboardItem playerStaticLeaderboardItemPrefab;
        [SerializeField] private Transform leaderboardContainer;
        
        [SerializeField] private MainMenuUI mainMenuUI;

        private void OnEnable()
        {
            LeaderboardManager.Instance.OnPlayerScoreUpdated += OnLeaderboardPlayerScoreUpdated;
            LeaderboardManager.Instance.OnLeaderboardUpdatedData += OnLeaderboardUpdatedData;
            backButton.onClick.AddListener(OnClickBackButton);
            homeButton.onClick.AddListener(OnClickHomeButton);
            LeaderboardManager.Instance.GetPlayerScore();
            LeaderboardManager.Instance.GetScores();
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
            Hide();
            if (mainMenuUI != null)
            {
                this.gameObject.SetActive(false);
                mainMenuUI.gameObject.SetActive(true);
            }
        }

        private void OnClickHomeButton()
        {
            Hide();
            if (mainMenuUI != null)
            {
                this.gameObject.SetActive(false);
                mainMenuUI.gameObject.SetActive(true);
            }
        }

        private void Hide()
        {
            this.gameObject.SetActive(false);
            ResetLeaderboardUI();
            LeaderboardManager.Instance.OnPlayerScoreUpdated -= OnLeaderboardPlayerScoreUpdated;
            LeaderboardManager.Instance.OnLeaderboardUpdatedData -= OnLeaderboardUpdatedData;
        }
        
        private void Show()
        {
            this.gameObject.SetActive(true);
        }
    }
}
