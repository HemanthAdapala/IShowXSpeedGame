using System;
using System.Collections.Generic;
using DG.Tweening;
using Handlers;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerCanvasUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform streakCanvas;
        [SerializeField] private Transform multiplierCanvas;
        [SerializeField] private TextMeshProUGUI streakText;
        [SerializeField] private TextMeshProUGUI multiplierText;

        [Header("Text Configuration")]
        [SerializeField] private TextConfig textConfig;

        private Vector3 _initialStreakPosition;
        private Vector3 _initialMultiplierPosition;
        private Queue<Action> _textQueue = new Queue<Action>(); // ✅ Queue for staggered text display
        private bool _isTextShowing = false; // ✅ Flag to track animations

        private void Awake()
        {
            _initialStreakPosition = streakCanvas.localPosition;
            _initialMultiplierPosition = multiplierCanvas.localPosition;
        }

        private void OnEnable()
        {
            StreakHandler.OnStreakUpdated += QueueStreakText;
            StreakHandler.OnStreakMilestone += QueueMultiplierText;
            StreakHandler.OnStreakReset += QueueLostStreakMessage;
        }

        private void OnDisable()
        {
            StreakHandler.OnStreakUpdated -= QueueStreakText;
            StreakHandler.OnStreakMilestone -= QueueMultiplierText;
            StreakHandler.OnStreakReset -= QueueLostStreakMessage;
        }

        // ✅ QUEUE SYSTEM: Add Streak text to queue instead of playing instantly
        private void QueueStreakText(int value)
        {
            _textQueue.Enqueue(() =>
            {
                string message = $"+{value}"; // Always show +1 streak
                if (UnityEngine.Random.value < 0.2f) // 20% chance for special message
                {
                    message = textConfig.GetRandomContinuousMessage() + $" +{value}";
                }
                ShowFloatingText(streakCanvas, streakText, message, _initialStreakPosition);
            });

            ProcessQueue();
        }

        // ✅ QUEUE SYSTEM: Add Multiplier text to queue instead of playing instantly
        private void QueueMultiplierText(float value)
        {
            _textQueue.Enqueue(() =>
            {
                string message = $"+{value:F1}"; // Always show +1.0 multiplier
                if (UnityEngine.Random.value < 0.2f) // 20% chance for special message
                {
                    message = textConfig.GetRandomContinuousMessage() + $" +{value:F1}";
                }
                ShowFloatingText(multiplierCanvas, multiplierText, message, _initialMultiplierPosition);
            });

            ProcessQueue();
        }

        // ✅ QUEUE SYSTEM: Add Lost Streak Message to queue
        private void QueueLostStreakMessage()
        {
            _textQueue.Enqueue(() =>
            {
                string message = textConfig.GetRandomLostMessage();
                ShowFloatingText(streakCanvas, streakText, message, _initialStreakPosition);
            });

            ProcessQueue();
        }

        // ✅ Process the text queue (Ensures they don't overlap)
        private void ProcessQueue()
        {
            if (!_isTextShowing && _textQueue.Count > 0)
            {
                _isTextShowing = true;
                _textQueue.Dequeue()?.Invoke();
            }
        }

        // ✅ Shows Floating Text and calls the next queue item after animation
        private void ShowFloatingText(Transform canvas, TextMeshProUGUI text, string displayText, Vector3 initialPosition)
        {
            text.text = displayText;
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
            canvas.localPosition = initialPosition;

            if (!canvas.gameObject.activeInHierarchy)
            {
                canvas.gameObject.SetActive(true);
            }

            // Animate upwards (1 unit) & fade out
            canvas.DOLocalMoveY(initialPosition.y + 1f, 1f).SetEase(Ease.OutQuad);
            text.DOFade(0, 1f).OnComplete(() =>
            {
                canvas.gameObject.SetActive(false);
                _isTextShowing = false;
                ProcessQueue(); // Move to the next text in queue
            });
        }
    }
}
