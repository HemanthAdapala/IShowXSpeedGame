using System;
using Managers;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class PlayerAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField, Range(0.5f, 3f)] private float perfectJumpDistance = 1.5f;
    [SerializeField, Range(1f, 5f)] private float earlyJumpThreshold = 2f;
    [SerializeField, Range(0.5f, 2f)] private float lateJumpThreshold = 1f;
    [SerializeField, Range(0.5f, 3f)] private float animationSpeedMultiplier = 1.5f;

    [Header("Jump Settings")]
    [SerializeField, Range(0.5f, 3f)] private float jumpHeight = 1.2f;
    [SerializeField, Range(-20f, -9f)] private float gravity = -15.0f;
    [SerializeField, Range(0f, 1f)] private float jumpTimeout = 0.5f;
    [SerializeField, Range(0f, 0.5f)] private float fallTimeout = 0.15f;

    [Header("Ground Settings")]
    [SerializeField, Range(-0.5f, 0f)] private float groundedOffset = -0.14f;
    [SerializeField, Range(0.1f, 0.5f)] private float groundedRadius = 0.28f;
    [SerializeField] private LayerMask groundLayers;

    [Header("Dependencies")]
    [SerializeField] private PlayerAnimationsDataConfig animationsConfig;

    // Events
    public event Action OnJumpEarly;
    public event Action OnJumpPerfect;
    public event Action OnJumpLate;
    public event Action OnJumpComplete;

    // Component references
    private Animator _animator;
    private CharacterController _controller;
    private PlayerController _playerController;

    // Animation IDs - cached for performance
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private int _animIDIdle;
    private int _animIDDeath;

    // Physics state
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    private float _verticalVelocity;
    private const float TerminalVelocity = -53.0f;
    private bool _isGrounded;

    // Property with cached check
    public bool IsGrounded => _isGrounded;

    private void Awake()
    {
        CacheComponents();
        CacheAnimationIDs();
    }

    private void Start()
    {
        GameEventManager.OnFailedJump += OnHitByVehicle;
        GameEventManager.OnGameEnd += OnGameEndEvent;
    }

    private void OnGameEndEvent()
    {
        _animIDDeath = 0;
        string deathAnimation = animationsConfig.GetRandomDeathAnimationTrigger();
        _animIDDeath = Animator.StringToHash(deathAnimation);
        _animator.SetTrigger("Death");
    }

    private void OnDestroy()
    {
        GameEventManager.OnFailedJump -= OnHitByVehicle;
        GameEventManager.OnGameEnd -= OnGameEndEvent;
    }

    private void OnHitByVehicle()
    {
        _animator?.SetTrigger("OnHit");
    }

    private void CacheComponents()
    {
        // Get required components
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _playerController = GetComponent<PlayerController>();

        if (_playerController == null)
        {
            _playerController = GetComponentInParent<PlayerController>();
            if (_playerController == null)
            {
                Debug.LogError($"[{nameof(PlayerAnimator)}] PlayerController not found in this GameObject or parent", this);
            }
        }

        if (animationsConfig == null)
        {
            Debug.LogError($"[{nameof(PlayerAnimator)}] PlayerAnimationsDataConfig is not assigned", this);
            enabled = false;
        }

        // Initialize animation speed
        _animator.speed = animationSpeedMultiplier;
    }

    private void CacheAnimationIDs()
    {
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDIdle = Animator.StringToHash("Idle");
    }

    private void CacheAnimationJumpId(string jumpType)
    {
        _animIDJump = Animator.StringToHash(jumpType);
    }

    private void ResetAnimationJumpId()
    {
        _animIDJump = 0;
    }

    private void Update()
    {
        CheckGrounded();
        ProcessJumpAndGravity();
        MoveVertically();
        UpdateAnimationSpeed();
    }

    private void CheckGrounded()
    {
        // Create sphere position with offset
        Vector3 spherePosition = new Vector3(
            transform.position.x,
            transform.position.y - groundedOffset,
            transform.position.z
        );

        // Perform single sphere cast for ground check
        _isGrounded = Physics.CheckSphere(
            spherePosition,
            groundedRadius,
            groundLayers,
            QueryTriggerInteraction.Ignore
        );

        // Update animator ground state
        _animator.SetBool(_animIDGrounded, _isGrounded);
    }

    private void ProcessJumpAndGravity()
    {
        if (_isGrounded)
        {
            HandleGroundedState();
        }
        else
        {
            HandleAirborneState();
        }

        // Apply gravity if not at terminal velocity
        if (_verticalVelocity > TerminalVelocity)
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void HandleGroundedState()
    {
        // Reset fall timeout
        _fallTimeoutDelta = fallTimeout;

        // Reset jump/fall animations
        _animator.SetBool(_animIDJump, false);
        _animator.SetBool(_animIDFreeFall, false);

        // Detect landing impact
        if (_verticalVelocity < -2.0f)
        {
            OnJumpComplete?.Invoke();
        }

        // Apply small negative velocity to keep grounded
        _verticalVelocity = Mathf.Max(_verticalVelocity, -2f);

        // Process jump input
#if UNITY_EDITOR || UNITY_ANDROID
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) && _jumpTimeoutDelta <= 0.0f)
        {
            // Calculate jump force from physics formula
            _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            ResetAnimationJumpId();
            string randomJumpAnimationId = PlayRandomJumpAnimation();
            CacheAnimationJumpId(randomJumpAnimationId);
            _animator.SetBool(_animIDJump, true);
        }
#endif

        // Decrement jump timeout
        if (_jumpTimeoutDelta > 0.0f)
        {
            _jumpTimeoutDelta -= Time.deltaTime;
        }
    }

    

    private void HandleAirborneState()
    {
        // Reset jump timeout
        _jumpTimeoutDelta = jumpTimeout;

        // Process fall timeout for animation delay
        if (_fallTimeoutDelta > 0.0f)
        {
            _fallTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            _animator.SetBool(_animIDFreeFall, true);
        }
    }

    private void MoveVertically()
    {
        // Apply vertical movement from gravity/jumping
        Vector3 verticalMovement = new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime;
        _controller.Move(verticalMovement);
    }

    private void UpdateAnimationSpeed()
    {
        if (!_isGrounded)
        {
            // Dynamic animation speed based on vertical velocity
            float velocityFactor = Mathf.Abs(_verticalVelocity) / 10f;
            float baseJumpSpeed = Mathf.Clamp(velocityFactor, 0.8f, 2.0f) * animationSpeedMultiplier;

            // Different speeds for rising vs falling
            _animator.speed = _verticalVelocity > 0
                ? baseJumpSpeed
                : baseJumpSpeed * 1.2f; // Slightly faster on descent

            // Update motion speed parameter
            _animator.SetFloat(_animIDMotionSpeed, velocityFactor);
        }
        else
        {
            // Reset to normal speed when grounded
            _animator.speed = animationSpeedMultiplier;
            _animator.SetFloat(_animIDMotionSpeed, 1.0f);
        }
    }

    public void TriggerJump()
    {
        if (animationsConfig == null) return;

        PlayRandomJumpAnimation();
        EvaluateJumpTiming();
    }

    public void EvaluateJumpTiming()
    {
        if (_playerController?.CurrentTarget == null) return;

        float distance = Vector3.Distance(transform.position, _playerController.CurrentTarget.position);

        if (distance >= earlyJumpThreshold)
            OnJumpEarly?.Invoke();
        else if (distance <= lateJumpThreshold)
            OnJumpLate?.Invoke();
        else
            OnJumpPerfect?.Invoke();
    }

    public string PlayRandomJumpAnimation()
    {
        string triggerName = animationsConfig.GetRandomJumpAnimationTrigger();
        return triggerName;
    }

    public void PlayCelebrationAnimation()
    {
        string triggerName = animationsConfig.GetRandomCelebrationAnimationTrigger();
        _animator.SetTrigger(triggerName);
    }

    public void ResetAllAnimations()
    {
        if (_animator == null) return;

        // Clear all animation parameters efficiently
        foreach (AnimatorControllerParameter param in _animator.parameters)
        {
            switch (param.type)
            {
                case AnimatorControllerParameterType.Trigger:
                    _animator.ResetTrigger(param.nameHash);
                    break;
                case AnimatorControllerParameterType.Bool:
                    _animator.SetBool(param.nameHash, false);
                    break;
                case AnimatorControllerParameterType.Float:
                    _animator.SetFloat(param.nameHash, 0f);
                    break;
                case AnimatorControllerParameterType.Int:
                    _animator.SetInteger(param.nameHash, 0);
                    break;
            }
        }

        _animator.SetTrigger(_animIDIdle);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize ground check
        Gizmos.color = _isGrounded
            ? new Color(0.0f, 1.0f, 0.0f, 0.35f)  // Green when grounded
            : new Color(1.0f, 0.0f, 0.0f, 0.35f); // Red when airborne

        Vector3 spherePosition = new Vector3(
            transform.position.x,
            transform.position.y - groundedOffset,
            transform.position.z
        );

        Gizmos.DrawSphere(spherePosition, groundedRadius);
    }
}