using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class VehicleAudioController : MonoBehaviour
    {
        [Header("Audio References")]
        [SerializeField] private AudioSource audioSource;

        [Header("Basic Audio Clips")]
        [SerializeField] private AudioClip baseAudioClip;
        [SerializeField] private AudioClip scareCarAudioClip;


        private VehicleController _vehicleController;

        private void Start()
        {
            _vehicleController = GetComponent<VehicleController>();
            _vehicleController.OnCarEnteredScreen += OnCarEnteredScreen_VehicleController;
            _vehicleController.OnCarExitedScreen += OnCarExitedScreen_VehicleController;
        }

        private void OnCarExitedScreen_VehicleController()
        {

        }

        private void OnCarEnteredScreen_VehicleController()
        {

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
    }
}
