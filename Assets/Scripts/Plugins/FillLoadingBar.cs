using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins
{
    public class FillLoadingBar : MonoBehaviour
    {
        [SerializeField] private Slider loadingBar;
        
        private float _initialLoadingValue = 0f;
        private readonly float _loadingCompleteValue = 1.0f;
        
        private readonly string _mainGameSceneName = "MainGameScene";

        public void StartLoadingBarFill()
        {
            _initialLoadingValue = loadingBar.value;
            loadingBar.DOValue(_loadingCompleteValue, 2f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Debug.Log("Loading Complete!");
                SceneLoader.LoadScene(_mainGameSceneName);
            });
        }
    }
}
