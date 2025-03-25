using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Managers
{
    public class LobbyManager : MonoBehaviour
    {
        #region SINGLETON

        private static LobbyManager _instance;
        public static LobbyManager Instance { get => _instance; }
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);

                //Initialize UnityServices
                InitializeUnityServices();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        private async void InitializeUnityServices()
        {
            await UnityServices.InitializeAsync();
            await SignInAnonymously();
        }

        async Task SignInAnonymously()
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
                Player.PlayerData playerData = GameManager.Instance.GetPlayerData();
                if (playerData != null)
                {
                    LeaderboardManager.Instance.AddScore(playerData.bestScore);
                }
            };
            AuthenticationService.Instance.SignInFailed += s =>
            {
                // Take some action here...
                Debug.Log(s);
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }




    }
}
