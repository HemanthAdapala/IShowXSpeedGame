using System;
using UnityEngine;

namespace Controllers
{
    public class VehicleAudioController : MonoBehaviour
    {
        [Header("Audio References")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float minVolume = 0.2f;
        [SerializeField] private float maxVolume = 1f;
        [SerializeField] private float maxDistance = 50f;

        [Header("Basic Audio Clips")]
        [SerializeField] private AudioClip baseAudioClip;
        [SerializeField] private AudioClip scareCarAudioClip;

        private VehicleController _vehicleController;
        private Transform _playerTransform;
        private bool _isScareCar = false;
        private float _initialDistance;

        private void Start()
        {
            _vehicleController = GetComponent<VehicleController>();
            _playerTransform = PlayerController.Instance.transform;
            _isScareCar = _vehicleController.IsScareCar;

            // Initialize audio
            audioSource.volume = minVolume;
            audioSource.spatialBlend = 0; // Ensure 2D audio
            audioSource.loop = true;

            if (!_isScareCar)
            {
                PlayBaseAudioClip();
            }
            else
            {
                PlayScareAudioClip();
            }

            _initialDistance = Vector3.Distance(transform.position, _playerTransform.position);
        }

        private void Update()
        {
            if (_playerTransform == null) return;

            float currentDistance = Vector3.Distance(transform.position, _playerTransform.position);
            UpdateVolumeBasedOnDistance(currentDistance);
        }

        private void UpdateVolumeBasedOnDistance(float currentDistance)
        {
            // Calculate volume based on distance (inverse relationship)
            float normalizedDistance = Mathf.Clamp01(currentDistance / maxDistance);
            float volume = Mathf.Lerp(maxVolume, minVolume, normalizedDistance);

            // Smooth volume transition
            audioSource.volume = Mathf.Lerp(audioSource.volume, volume, Time.deltaTime * 5f);
        }

        public void PlayBaseAudioClip()
        {
            audioSource.clip = baseAudioClip;
            PlayAudioClip();
        }

        public void PlayScareAudioClip()
        {
            audioSource.clip = scareCarAudioClip;
            PlayAudioClip();
        }

        private void PlayAudioClip()
        {
            if (audioSource.clip != null)
                audioSource.Play();
        }

        public void PlayAudioClip(AudioClip emojiSound)
        {
            audioSource.clip = emojiSound;
            PlayAudioClip();
        }

        public void StopAudioClip()
        {
            audioSource.Stop();
        }

        private void OnDestroy()
        {
            // if (_vehicleController != null)
            // {
            //     _vehicleController.OnCarEnteredScreen -= OnCarEnteredScreen_VehicleController;
            //     _vehicleController.OnCarExitedScreen -= OnCarExitedScreen_VehicleController;
            // }
        }

        private void OnCarExitedScreen_VehicleController()
        {
            throw new NotImplementedException();
        }

        private void OnCarEnteredScreen_VehicleController()
        {
            throw new NotImplementedException();
        }
    }
}