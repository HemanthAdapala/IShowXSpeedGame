using System;
using System.Collections.Generic;
using Controllers;
using DG.Tweening;
using Managers;
using Player;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator), typeof(CharacterController),typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    #region Singleton Implementation
    private static PlayerController _instance;
    public static PlayerController Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        Initialize();
    }
    #endregion

    [Header("Movement Settings")]
    [SerializeField] private Ease rotateEase = Ease.OutQuad;

    [Header("Dependencies")]
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private PlayerParticleEffectsHandler playerParticleEffectsHandler;

    private Vector3 _originalPosition;
    private Transform _currentTarget;
    private VehicleController _currentVehicleController;
    private Queue<VehicleController> _vehicleQueue;

    public Transform CurrentTarget => _currentTarget;
    public PlayerAnimator PlayerAnimator => playerAnimator;
    public PlayerParticleEffectsHandler PlayerParticleEffectsHandler => playerParticleEffectsHandler;

    private void Initialize()
    {
        _originalPosition = transform.position;
        _vehicleQueue = new Queue<VehicleController>();
        CacheComponents();
    }

    private void CacheComponents()
    {
        if (playerAnimator == null) playerAnimator = GetComponent<PlayerAnimator>();
        if (playerParticleEffectsHandler == null)
            playerParticleEffectsHandler = GetComponentInChildren<PlayerParticleEffectsHandler>();
    }

    private void Start()
    {
        RegisterEventHandlers();
        ResetToStartPosition();
    }

    private void RegisterEventHandlers()
    {
        if (GamePlayManager.Instance != null)
            GamePlayManager.Instance.OnVehicleSpawned += OnVehicleSpawned;

        GameEventManager.OnFailedJump += HandleFailedJump;
    }

    private void OnDestroy()
    {
        UnregisterEventHandlers();
        ClearCurrentTarget();
        _vehicleQueue?.Clear();
    }

    private void UnregisterEventHandlers()
    {
        if (GamePlayManager.Instance != null)
            //GamePlayManager.Instance.OnVehicleSpawned -= OnVehicleSpawned;
            GamePlayManager.Instance.OnVehicleSpawned -= OnVehicleSpawned;


        GameEventManager.OnFailedJump -= HandleFailedJump;
    }

    private void OnVehicleSpawned(VehicleController controller)
    {
        if(controller == null) return;
        _vehicleQueue.Enqueue(controller);
        ProcessNextVehicleIfIdle();
    }

    private void ResetToStartPosition()
    {
        transform.position = _originalPosition;
    }

    private void ProcessNextVehicleIfIdle()
    {
        if (_currentTarget == null)
            ProcessNextVehicle();
    }

    private void ProcessNextVehicle()
    {
        ClearCurrentTarget();

        while (_vehicleQueue.Count > 0)
        {
            var nextVehicle = _vehicleQueue.Dequeue();
            if (IsValidVehicle(nextVehicle))
            {
                SetCurrentTarget(nextVehicle);
                return;
            }
        }
    }

    private bool IsValidVehicle(VehicleController vehicle)
    {
        return vehicle != null && vehicle.gameObject.activeInHierarchy;
    }

    private void SetCurrentTarget(VehicleController vehicle)
    {
        _currentTarget = vehicle.transform;
        _currentVehicleController = vehicle;

        vehicle.OnVehicleDestroyed += OnCurrentVehicleCompleted;
        vehicle.OnVehicleReachedDestination += OnCurrentVehicleCompleted;

        RotateTowardsTarget(_currentTarget);
    }

    private void ClearCurrentTarget()
    {
        if (_currentVehicleController == null) return;

        _currentVehicleController.OnVehicleDestroyed -= OnCurrentVehicleCompleted;
        _currentVehicleController.OnVehicleReachedDestination -= OnCurrentVehicleCompleted;

        _currentTarget = null;
        _currentVehicleController = null;
    }

    private void OnCurrentVehicleCompleted()
    {
        ProcessNextVehicle();
    }

    private void RotateTowardsTarget(Transform target)
    {
        if (target == null || !target.gameObject.activeInHierarchy) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(targetRotation, 0.5f).SetEase(rotateEase);
        }
    }

    private void HandleFailedJump()
    {
        ResetState();
    }

    private void ResetState()
    {
        ClearCurrentTarget();
        ProcessNextVehicle();
    }

    private void Update()
    {
        ValidateCurrentTarget();
    }

    private void ValidateCurrentTarget()
    {
        if (_currentTarget != null && !_currentTarget.gameObject.activeInHierarchy)
        {
            OnCurrentVehicleCompleted();
        }
    }
}