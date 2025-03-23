using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Handlers
{
    public class TipsHandler : MonoBehaviour
    {
        [Header("Tips Lists")]
        [SerializeField] private List<string> streakTips;
        [SerializeField] private List<string> multiplierTips;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI textForTips;

        [Header("Timing Configurations")]
        [SerializeField] private float tipDisplayDuration = 3f; // How long the tip stays visible
        [SerializeField] private float tipFadeDuration = 0.5f;  // Smooth fade duration
        [SerializeField] private float tipInterval = 5f;        // Time between tips

        private void Start()
        {
            if (textForTips == null)
            {
                Debug.LogError("TextForTips is not assigned in TipsHandler!");
                return;
            }

            StartCoroutine(ShowRandomTips());
        }

        private IEnumerator ShowRandomTips()
        {
            while (true)
            {
                string randomTip = GetRandomTip();
                if (!string.IsNullOrEmpty(randomTip))
                {
                    yield return ShowTip(randomTip);
                }

                yield return new WaitForSeconds(tipInterval);
            }
        }

        private string GetRandomTip()
        {
            bool showStreakTip = Random.value > 0.5f; // 50% chance for either tip

            if (showStreakTip && streakTips.Count > 0)
            {
                return streakTips[Random.Range(0, streakTips.Count)];
            }
            else if (multiplierTips.Count > 0)
            {
                return multiplierTips[Random.Range(0, multiplierTips.Count)];
            }

            return string.Empty; // No tips available
        }

        private IEnumerator ShowTip(string tipText)
        {
            textForTips.text = tipText;
            textForTips.alpha = 0; // Ensure it's invisible before fading in

            // Fade in
            textForTips.DOFade(1, tipFadeDuration);
            yield return new WaitForSeconds(tipDisplayDuration);

            // Fade out
            textForTips.DOFade(0, tipFadeDuration);
            yield return new WaitForSeconds(tipFadeDuration);
        }
    }
}
