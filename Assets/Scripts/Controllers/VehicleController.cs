using System;
using Configs;
using Data;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(CubeCollisionDetector),typeof(VehicleAudioController),typeof(VehicleEmojiTransformController))]
    public class VehicleController : MonoBehaviour
    {
        private PrometeoCarController _carController;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector3 _targetPosition;
        private bool _isMoving = false;
        private bool _isScareCar = false;
        private bool _reachedScarePoint = false;
        private Vector3 _scarePoint;
        private Vector3 _finalDestination;
        private GameObject _indicator;
        private bool _wasOnScreen = false;

        [SerializeField] private float destructionDistance = 2f;
        [SerializeField] private Ease rotateEase = Ease.OutSine;

        [Header("Data Configs")]
        [SerializeField] private VehicleDataRewardConfig vehicleDataRewardConfig;
        [SerializeField] private VehicleDataConfig vehicleData;

        [Header("Controllers")]
        private VehicleAudioController _vehicleAudioController;
        private VehicleEmojiTransformController _vehicleEmojiTransformController;

        public event Action OnCarEnteredScreen;
        public event Action OnCarExitedScreen;
        public event Action OnVehicleDestroyed;
        public event Action OnVehicleReachedDestination;

        public OffscreenIndicatorManager offscreenIndicatorManager;
        public bool IsScareCar => _isScareCar;

        void Start()
        {
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

        private void InitializeMovement()
        {
            _startPosition = transform.position;
            RotateTowards(_targetPosition);
            _isMoving = true;
        }

        void Update()
        {
            if (_isMoving && _carController != null)
            {
                _carController.GoForward();

                if (_isScareCar && !_reachedScarePoint &&
                    Vector3.Distance(transform.position, _scarePoint) < destructionDistance)
                {
                    _reachedScarePoint = true;
                    _targetPosition = _finalDestination;
                    RotateTowards(_targetPosition);
                }
            }

            if (_isMoving && Vector3.Distance(transform.position, _targetPosition) < destructionDistance)
            {
                StopAndDestroy();
            }

            CheckScreenSpaceEvents();
        }

        public void SetScareCarPath(Vector3 spawnPos, Vector3 scarePoint, Vector3 endPos)
        {
            _isScareCar = true;
            _scarePoint = scarePoint;
            _finalDestination = endPos;
            _targetPosition = scarePoint;
        }

        public void SetTarget(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
            _endPosition = targetPosition;
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

        public void StopAndDestroy()
        {
            _isMoving = false;
            if (_carController != null)
            {
                _carController.accelerationMultiplier = 0;
            }

            if (offscreenIndicatorManager != null && _indicator != null)
            {
                offscreenIndicatorManager.DestroyIndicator(_indicator);
            }

            OnVehicleReachedDestination?.Invoke();
            Destroy(gameObject, 0.2f);
        }

        private void HandlePlayerCollision()
        {
            OnVehicleDestroyed?.Invoke();
            if (Camera.main != null)
            {
                Camera.main.transform.DOShakePosition(0.5f, 0.5f, 10, 90, false, true);
            }
            StopAndDestroy();
        }

        private void CheckScreenSpaceEvents()
        {
            bool isCurrentlyOnScreen = IsVisibleOnScreen();

            if (isCurrentlyOnScreen && !_wasOnScreen)
            {
                _wasOnScreen = true;
                OnCarEnteredScreen?.Invoke();
            }
            else if (!isCurrentlyOnScreen && _wasOnScreen)
            {
                _wasOnScreen = false;
                OnCarExitedScreen?.Invoke();
            }
        }

        private bool IsVisibleOnScreen()
        {
            if (Camera.main == null) return false;
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
            return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.z > 0;
        }

        void OnDisable()
        {
            GameEventManager.OnFailedJump -= HandlePlayerCollision;
        }

        public VehicleDataRewardConfig GetVehicleDataRewardConfig() => vehicleDataRewardConfig;
        public VehicleDataConfig GetVehicleData() => vehicleData;
        public VehicleAudioController GetVehicleAudioController() => _vehicleAudioController;
        public VehicleEmojiTransformController GetVehicleEmojiTransformController() => _vehicleEmojiTransformController;
    }
}