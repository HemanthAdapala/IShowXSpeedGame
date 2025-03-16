using Plugins;
using UnityEngine;
using UnityEngine.UI;

namespace Handlers
{
    public class LoadingSceneHandler : MonoBehaviour
    {
        [SerializeField] private FillLoadingBar loadingBar;
        
        private void Awake()
        {
            loadingBar.StartLoadingBarFill();
        }
    }
}
