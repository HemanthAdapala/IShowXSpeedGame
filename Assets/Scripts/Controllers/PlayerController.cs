using UnityEngine;
using System;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public float JumpHeight = 2f;
    public float JumpDuration = 0.5f;
    public float PerfectJumpDistance = 1.5f;
    public float EarlyJumpThreshold = 2f;
    public float LateJumpThreshold = 1f;

    public event Action OnJumpEarly;
    public event Action OnJumpPerfect;
    public event Action OnJumpLate;

    private bool isJumping = false;
    private Vector3 originalPosition;
    private Transform currentTarget;
    [SerializeField] private Ease rotateEase;
    [SerializeField] private PlayerAnimator playerAnimator;

    void Start()
    {
        if (playerAnimator == null)
            playerAnimator = GetComponent<PlayerAnimator>();

        originalPosition = transform.position;
        GameManager.Instance.OnVehicleSpawned += OnVehicleSpawned;
    }

    private void OnVehicleSpawned(object sender, GameManager.VehicleSpawnedEventArgs e)
    {
        if (currentTarget == null)
        {
            currentTarget = e.carController.transform;
            RotateTowardsTarget(currentTarget);
        }
    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
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
            Jump();
        }
    }

    void Jump()
    {
        if (isJumping) return;

        isJumping = true;
        playerAnimator?.TriggerJumpAnimation();
        CheckJumpTiming();

        transform.DOMoveY(originalPosition.y + JumpHeight, JumpDuration / 2)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOMoveY(originalPosition.y, JumpDuration / 2)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() => isJumping = false);
            });
    }

    void CheckJumpTiming()
    {
        if (currentTarget == null) return;

        float distanceToVehicle = Vector3.Distance(transform.position, currentTarget.position);
        if (distanceToVehicle >= EarlyJumpThreshold) OnJumpEarly?.Invoke();
        else if (distanceToVehicle <= LateJumpThreshold) OnJumpLate?.Invoke();
        else OnJumpPerfect?.Invoke();
    }
}
