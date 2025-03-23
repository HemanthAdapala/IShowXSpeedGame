using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class VehicleAudioController : MonoBehaviour
    {
        [Header("Audio References")]
        [SerializeField] private AudioSource audioSource;
        
        
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

        public void PlayAudioClip(AudioClip emojiSound)
        {
            audioSource.clip = emojiSound;
            audioSource.Play();
        }

        public void StopAudioClip()
        {
            audioSource.Stop();
        }
    }
}
