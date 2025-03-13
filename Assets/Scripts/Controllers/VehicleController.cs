using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class VehicleController : MonoBehaviour
    {
        private PrometeoCarController _carController;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private bool _isMoving = false;
        private GameObject _indicator;
        
        [SerializeField] private float destroyDelay = 1f;
        [SerializeField] private Ease rotateEase = Ease.OutSine;
        
        public OffscreenIndicatorManager offscreenIndicatorManager;

        
        void Start()
        {
            GameEventManager.OnPlayerCollision += HandlePlayerCollision;
            _carController = GetComponent<PrometeoCarController>();
            if (_carController == null)
            {
                Debug.LogError("Car controller is null!");
                return;
            }

            InitializeMovement();

            // Create the indicator when the vehicle starts moving
            if (offscreenIndicatorManager != null)
            {
                _indicator = offscreenIndicatorManager.CreateIndicator(transform);
            }
            _isMoving = true;
        }

        private void InitializeMovement()
        {
            _startPosition = transform.position;
            _endPosition = -transform.position;
            RotateTowards(_endPosition);
        }

        void Update()
        {
            if (_isMoving && _carController is not null)
            {
                _carController.GoForward(); // Keep accelerating every frame
            }

            if (_isMoving && Vector3.Distance(transform.position, _endPosition) < 2f)
            {
                StopAndDestroy();
            }
        }

        private void StopAndDestroy()
        {
            if (_carController is not null)
            {
                _carController.accelerationMultiplier = 0;
            }

            _isMoving = false;

            // Destroy the indicator if it exists
            if (offscreenIndicatorManager is not null && _indicator is not null)
            {
                offscreenIndicatorManager.DestroyIndicator(_indicator);
            }

            // Destroy the vehicle after a delay
            Destroy(gameObject, destroyDelay);
        }

        private void RotateTowards(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            direction.y = 0; // Prevent unwanted vertical rotation
            
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.DORotateQuaternion(targetRotation, 0.5f).SetEase(rotateEase);
            }
        }
        
        private void HandlePlayerCollision()
        {
            Destroy(gameObject);
            if (Camera.main != null) Camera.main.transform.DOShakePosition(0.5f, 0.5f, 10, 90, false, true);
        }
        
        void OnDisable()
        {
            GameEventManager.OnPlayerCollision -= HandlePlayerCollision;
        }
    }
}
