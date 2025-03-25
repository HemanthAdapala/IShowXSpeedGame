using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace Managers
{
    public class LeaderboardManager : MonoBehaviour
    {
        #region SINGLETON

        private static LeaderboardManager _instance;

        public static LeaderboardManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (LeaderboardManager)FindAnyObjectByType(typeof(LeaderboardManager));
                    if (_instance == null)
                    {
                        GameObject gameObj = new GameObject();
                        gameObj.name = typeof(LeaderboardManager).Name;
                        _instance = gameObj.AddComponent<LeaderboardManager>();
                    }
                }
                return _instance;
            }
        }

        #endregion

        const string LeaderboardId = "First_Test";

        string VersionId { get; set; }
        int Offset { get; set; }
        int Limit { get; set; }
        int RangeLimit { get; set; }
        List<string> FriendIds { get; set; }
        
        
        public event Action<LeaderboardEntry> OnPlayerScoreUpdated;
        public event Action<LeaderboardScoresPage> OnLeaderboardUpdatedData;


        public async void AddScore(int bestScore)
        {
            try
            {
                var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, bestScore);
                Debug.Log(JsonConvert.SerializeObject(scoreResponse));
            }
            catch (Exception e)
            {
                Debug.Log("Error: " + e);
            }
        }

        public async void GetScores()
        {
            try
            {
                var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
                OnLeaderboardUpdatedData?.Invoke(scoresResponse);
            }
            catch (Exception e)
            {
                Debug.LogError("Error: " + e);
            }
        }

        public async void GetPaginatedScores()
        {
            try
            {
                Offset = 10;
                Limit = 10;
                var scoresResponse =
                    await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions { Offset = Offset, Limit = Limit });
                Debug.Log(JsonConvert.SerializeObject(scoresResponse));
            }
            catch (Exception e)
            {
                Debug.Log("Error: " + e);
            }
        }

        public async void GetPlayerScore()
        {
            try
            {
                var scoreResponse =
                    await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
                Debug.Log(JsonConvert.SerializeObject(scoreResponse));
                OnPlayerScoreUpdated?.Invoke(scoreResponse);
            }
            catch (Exception e)
            {
                Debug.Log("Error: " + e);
            }
        }

        public async void GetVersionScores()
        {
            try
            {
                var versionScoresResponse =
                    await LeaderboardsService.Instance.GetVersionScoresAsync(LeaderboardId, VersionId);
                Debug.Log(JsonConvert.SerializeObject(versionScoresResponse));
            }
            catch (Exception e)
            {
                Debug.Log("Error: " + e);
            }
        }

        public void UpdateLeaderboard()
        {

        }
    }
}
