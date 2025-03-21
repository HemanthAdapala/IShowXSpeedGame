using System;
using Configs;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Handlers
{
    public class StreakHandler : MonoBehaviour
    {
        public static event Action<int> OnStreakUpdated;  
        public static event Action<float> OnStreakMilestone; 
        public static event Action OnStreakReset; 

        [SerializeField] private StreakConfig streakConfig;
        [SerializeField] private TextConfig textConfig;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI streakText;
        [SerializeField] private Slider streakSlider;

        private int _streakCount = 0;
        private float _initialScoreMultiplier = 1.0f;
        private float _scoreMultiplier = 1.0f;
        private float _sliderValue = 0f;

        private void OnEnable()
        {
            GameEventManager.OnSuccessfulJump += IncreaseStreak;
            GameEventManager.OnFailedJump += ResetStreak;
            ResetUIData();
            // Set initial UI text
            streakText.text = "Max:- " + streakConfig.milestoneThresholds[0].ToString();
        }

        private void OnDisable()
        {
            GameEventManager.OnSuccessfulJump -= IncreaseStreak;
            GameEventManager.OnFailedJump -= ResetStreak;
        }

        private void IncreaseStreak()
        {
            _streakCount++;
            _scoreMultiplier = streakConfig.GetMultiplier(_streakCount);

            StreakUpdated();

            if (!Mathf.Approximately(_scoreMultiplier, _initialScoreMultiplier))
            {
                _initialScoreMultiplier = _scoreMultiplier;
                MultiplierUpdated();
                Debug.Log("🎉 Milestone reached!" + _scoreMultiplier);
            }

            SetUIData();
            Debug.Log($"🔥 Streak: {_streakCount} | Multiplier: {_scoreMultiplier}");
        }

        private void MultiplierUpdated()
        {
            OnStreakMilestone?.Invoke(_scoreMultiplier);
            GameEventManager.TriggerMultiplierUpdated(_scoreMultiplier);
        }

        private void StreakUpdated()
        {
            OnStreakUpdated?.Invoke(_streakCount);
            GameEventManager.TriggerStreakUpdated(_streakCount);
        }

        private void SetUIData()
        {
            for (int i = 0; i < streakConfig.milestoneThresholds.Length; i++)
            {
                int streakThreshold = streakConfig.milestoneThresholds[i];
                if (_streakCount <= streakThreshold)
                {
                    streakText.text = "Max: " + streakThreshold.ToString();
                    SetSliderValue();
                    return;
                }
            }

            void SetSliderValue()
            {
                _sliderValue = Mathf.Clamp(_streakCount / 10f, 0f, 1f);
                streakSlider.DOValue(_sliderValue, 0.5f).SetEase(Ease.OutQuad);
            }
        }

        private void ResetStreak()
        {
            Debug.Log("Player collision detected_StreakHandler!");
            _streakCount = 0;
            _scoreMultiplier = 1.0f;
            GameEventManager.TriggerStreakUpdated(_streakCount);
            GameEventManager.TriggerMultiplierUpdated(_scoreMultiplier);

            OnStreakReset?.Invoke();
            ResetUIData();
            Debug.Log("Streak reset!");
        }

        private void ResetUIData()
        {
            streakSlider.DOValue(0f, 0.3f).SetEase(Ease.OutQuad);
        }

        public float GetMultiplier()
        {
            return _scoreMultiplier;
        }
    }
}
