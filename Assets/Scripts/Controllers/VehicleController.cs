using System;
using Configs;
using Data;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class VehicleController : MonoBehaviour
    {
        private PrometeoCarController _carController;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector3 _targetPosition;
        private bool _isMoving = false;
        private bool _isScareCar = false;
        public bool IsScareCar => _isScareCar;
        private bool _wasOnScreen = false; // Track if the car was previously on screen
        private GameObject _indicator;

        [SerializeField] private float destroyDelay = 1f;
        [SerializeField] private Ease rotateEase = Ease.OutSine;

        [Header("Data Configs")]
        [SerializeField] private VehicleDataRewardConfig vehicleDataRewardConfig;
        [SerializeField] private VehicleDataConfig vehicleData;

        [Header("Controllers")]
        private VehicleAudioController _vehicleAudioController;
        private VehicleEmojiTransformController _vehicleEmojiTransformController;

        public event Action OnCarEnteredScreen; // Event when car enters screen space
        public event Action OnCarExitedScreen;  // Event when car exits screen space

        public event Action OnVehicleDestroyed; // Event when vehicle is destroyed
        public event Action OnVehicleReachedDestination; // Event when vehicle reaches its target

        private GameObject _currentEmoji; // Reference to the instantiated emoji

        public OffscreenIndicatorManager offscreenIndicatorManager;

        public VehicleDataRewardConfig GetVehicleDataRewardConfig() => vehicleDataRewardConfig;
        public VehicleDataConfig GetVehicleData() => vehicleData;

        void Start()
        {
            //Controller Initialization
            ControllersInit();

            GameEventManager.OnFailedJump += HandlePlayerCollision;

            if (_carController == null)
            {
                Debug.LogError("Car controller is null!");
                return;
            }
            if (offscreenIndicatorManager != null)
            {
                _indicator = offscreenIndicatorManager.CreateIndicator(transform);
                _indicator.GetComponent<OffscreenIndicator>().SetIndicatorData(vehicleData.vehicleData);
            }
            InitializeMovement();
        }

        private void ControllersInit()
        {
            _carController = GetComponent<PrometeoCarController>();
            _vehicleEmojiTransformController = GetComponent<VehicleEmojiTransformController>();
            _vehicleAudioController = GetComponent<VehicleAudioController>();
        }

        public VehicleAudioController GetVehicleAudioController() => _vehicleAudioController;
        public VehicleEmojiTransformController GetVehicleEmojiTransformController() => _vehicleEmojiTransformController;

        private void InitializeMovement()
        {
            _startPosition = transform.position;
            _endPosition = -transform.position;

            if (_targetPosition == Vector3.zero)
            {
                _targetPosition = _endPosition;
            }

            RotateTowards(_targetPosition);
            _isMoving = true;
        }

        void Update()
        {
            if (_isMoving && _carController is not null)
            {
                _carController.GoForward(); // Keep accelerating every frame
            }

            if (_isMoving && Vector3.Distance(transform.position, _targetPosition) < 2f)
            {
                StopAndDestroy();
            }

            // Check screen space entry/exit events
            CheckScreenSpaceEvents();
        }

        private void CheckScreenSpaceEvents()
        {
            bool isCurrentlyOnScreen = IsVisibleOnScreen();

            if (isCurrentlyOnScreen && !_wasOnScreen) // Only trigger when first entering
            {
                _wasOnScreen = true; // Mark as on-screen
                OnCarEnteredScreen?.Invoke();
            }
            else if (!isCurrentlyOnScreen && _wasOnScreen) // Only trigger when exiting
            {
                _wasOnScreen = false; // Mark as off-screen
                OnCarExitedScreen?.Invoke();
            }
        }



        /// <summary>
        /// Checks if the vehicle is visible on the main camera screen.
        /// </summary>
        private bool IsVisibleOnScreen()
        {
            if (Camera.main == null) return false;

            Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
            return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.z > 0;
        }


        private void StopAndDestroy()
        {
            if (_carController is not null)
            {
                _carController.accelerationMultiplier = 0;
            }

            _isMoving = false;

            if (offscreenIndicatorManager is not null && _indicator is not null)
            {
                offscreenIndicatorManager.DestroyIndicator(_indicator);
            }
            OnVehicleReachedDestination?.Invoke();

            Destroy(gameObject, destroyDelay);
        }

        private void RotateTowards(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.DORotateQuaternion(targetRotation, 0.5f).SetEase(rotateEase);
            }
        }

        private void HandlePlayerCollision()
        {
            Debug.Log("Player collision detected_VehicleController!");
            OnVehicleDestroyed?.Invoke();
            Destroy(gameObject);
            if (Camera.main != null) Camera.main.transform.DOShakePosition(0.5f, 0.5f, 10, 90, false, true);
        }

        void OnDisable()
        {
            GameEventManager.OnFailedJump -= HandlePlayerCollision;
        }

        public void SetScareMode()
        {
            _isScareCar = true;
        }


        public void SetTarget(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
        }

    }
}
