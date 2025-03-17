using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Handlers
{
    public class MultiplierHandler : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI multiplierText;
        [SerializeField] private Slider multiplierSlider;
        
        [SerializeField] private StreakHandler streakHandler;
        
        private readonly float _initialMultiplierValue = 1.0f;
        private readonly float _maxMultiplierValue = 5.0f;
        private float _sliderValue = 0f;

        private void OnEnable()
        {
            ResetMultiplier(); // Ensure UI starts correctly
            StreakHandler.OnStreakMilestone += UpdateMultiplier;
            StreakHandler.OnStreakReset += ResetMultiplierOnStreakReset;
        }

        private void OnDisable()
        {
            StreakHandler.OnStreakMilestone -= UpdateMultiplier;
            StreakHandler.OnStreakReset -= ResetMultiplierOnStreakReset;
        }

        private void UpdateMultiplier(float value)
        {
            if (value <= _maxMultiplierValue)
            {
                _sliderValue = value / _maxMultiplierValue;
                multiplierSlider.DOValue(_sliderValue, 0.5f).SetEase(Ease.OutQuad);
                multiplierText.text = value.ToString("F1", CultureInfo.InvariantCulture); // Update UI Text
            }
        }

        private void ResetMultiplierOnStreakReset()
        {
            ResetMultiplier();
        }

        private void ResetMultiplier()
        {
            _sliderValue = 0f;
            multiplierSlider.DOValue(_sliderValue, 0.3f).SetEase(Ease.OutQuad);
            multiplierText.text = _initialMultiplierValue.ToString("F1", CultureInfo.InvariantCulture);
        }
    }
}