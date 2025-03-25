using System;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerAnimator), typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        #region Singleton

        private static PlayerController _instance;
        public static PlayerController Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _originalPosition = transform.position;
        }

        #endregion

        private bool _isJumping = false;
        private Vector3 _originalPosition;
        private Transform _currentTarget;

        public float jumpHeight = 2f;
        public float jumpDuration = 0.5f;
        public float perfectJumpDistance = 1.5f;
        public float earlyJumpThreshold = 2f;
        public float lateJumpThreshold = 1f;

        public event Action OnJumpEarly;
        public event Action OnJumpPerfect;
        public event Action OnJumpLate;


        [SerializeField] private Ease rotateEase;
        [SerializeField] private PlayerAnimator playerAnimator;
        public PlayerAnimator PlayerAnimator => playerAnimator;
        [SerializeField] private PlayerParticleEffectsHandler playerParticleEffectsHandler;
        public PlayerParticleEffectsHandler PlayerParticleEffectsHandler => playerParticleEffectsHandler;

        void Start()
        {
            if (playerAnimator == null)
                playerAnimator = GetComponent<PlayerAnimator>();

            if (playerParticleEffectsHandler == null)
                playerParticleEffectsHandler = GetComponentInChildren<PlayerParticleEffectsHandler>();

            this.transform.position = _originalPosition;
            GamePlayManager.Instance.OnVehicleSpawned += OnVehicleSpawned;
            GameEventManager.OnFailedJump += HandlePlayerCollision;

        }

        private void OnDestroy()
        {
            GameEventManager.OnFailedJump -= HandlePlayerCollision;
        }

        private void HandlePlayerCollision()
        {
            ResetValues();
        }

        private void OnVehicleSpawned(object sender, GamePlayManager.VehicleSpawnedEventArgs e)
        {
            if (_currentTarget is null)
            {
                _currentTarget = e.CarController.transform;
                RotateTowardsTarget(_currentTarget);
            }
        }

        private void RotateTowardsTarget(Transform target)
        {
            // Calculate horizontal direction to target
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Ignore vertical difference

            // Only rotate if there's a valid direction
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.DORotateQuaternion(targetRotation, 0.5f).SetEase(rotateEase);
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ResetAllAnimations();
                Jump();
            }
        }

        private void ResetAllAnimations()
        {

        }

        void Jump()
        {
            if (_isJumping) return;

            _isJumping = true;
            playerAnimator?.TriggerJumpAnimation(); // Play animation first

            //StartJumpMovement();
            // // Add a small delay before movement (sync with animation)
            // float animationDelay = 0.1f; // Adjust this based on your animation
            // DOVirtual.DelayedCall(animationDelay, () =>
            // {
            //     CheckJumpTiming();

            //     transform.DOMoveY(_originalPosition.y + jumpHeight, jumpDuration / 2)
            //         .SetEase(Ease.OutQuad)
            //         .OnComplete(() =>
            //         {
            //             transform.DOMoveY(_originalPosition.y, jumpDuration / 2)
            //                 .SetEase(Ease.InQuad)
            //                 .OnComplete(ResetValues);
            //         });
            // });
        }

        //Start JumpMovement based on ANIMATION EVENT
        public void StartJumpMovement()
        {
            CheckJumpTiming();

            transform.DOMoveY(_originalPosition.y + jumpHeight, jumpDuration / 2)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    transform.DOMoveY(_originalPosition.y, jumpDuration / 2)
                        .SetEase(Ease.InQuad)
                        .OnComplete(ResetValues);
                });
        }


        private void ResetValues()
        {
            Debug.Log("Player collision detected_PlayerController!");
            _isJumping = false;
            _currentTarget = null;
        }

        void CheckJumpTiming()
        {
            if (_currentTarget is null) return;

            float distanceToVehicle = Vector3.Distance(transform.position, _currentTarget.position);
            if (distanceToVehicle >= earlyJumpThreshold) OnJumpEarly?.Invoke();
            else if (distanceToVehicle <= lateJumpThreshold) OnJumpLate?.Invoke();
            else OnJumpPerfect?.Invoke();
        }
    }
}
