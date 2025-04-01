using UnityEngine;
using DG.Tweening;
using System;
using Configs;

namespace Controllers
{
    public class VehicleEmojiTransformController : MonoBehaviour
    {
        [Header("Emoji Prefab Setup")]
        [SerializeField] private Transform emojiParentTransform;
        [SerializeField] private GameObject emojiPrefab = null;

        private GameObject _currentEmoji;
        private VehicleController _vehicleController;
        private VehicleDataConfig _vehicleDataConfig;

        void Start()
        {
            _vehicleController = GetComponent<VehicleController>();
            _vehicleController.OnCarEnteredScreen += OnCarEnteredScreen;
            _vehicleController.OnCarExitedScreen += OnCarExitedScreen;
            _vehicleDataConfig = _vehicleController.GetVehicleData();

            InstantiateEmoji();
        }

        private void InstantiateEmoji()
        {
            var vehicleEmojiData = _vehicleDataConfig.GetRandomEmoji();
            emojiPrefab = vehicleEmojiData.emoji;

            if (emojiPrefab == null || emojiParentTransform == null)
            {
                Debug.LogError("Emoji prefab or parent transform is missing!");
                return;
            }

            _currentEmoji = Instantiate(emojiPrefab, emojiParentTransform.position, Quaternion.identity, emojiParentTransform);
            _currentEmoji.SetActive(false);
        }

        private void OnCarEnteredScreen()
        {
            if (_currentEmoji == null) return;

            _currentEmoji.SetActive(true);


            // Play emoji particle effect
            ParticleSystem emojiParticles = _currentEmoji.GetComponent<ParticleSystem>();
            if (emojiParticles != null)
            {
                emojiParticles.Play();
            }

            // Animate emoji (fade out & destroy)
            _currentEmoji.transform.DOLocalMoveY(emojiParentTransform.localPosition.y + 0.1f, 1f).SetEase(Ease.OutQuad);
            _currentEmoji.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() =>
            {
                Destroy(_currentEmoji);
            });
        }

        private void OnCarExitedScreen()
        {

        }

        private void OnDestroy()
        {
            _vehicleController.OnCarEnteredScreen -= OnCarEnteredScreen;
            _vehicleController.OnCarExitedScreen -= OnCarExitedScreen;
        }
    }
}
