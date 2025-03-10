using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float JumpHeight = 2f; // Configurable jump height
    public float JumpDuration = 0.5f; // Duration of the jump

    private bool isJumping = false; // Track if the player is currently jumping
    private Vector3 originalPosition; // Store the original position of the player cube

    private Transform currentTarget;
    [SerializeField] private Ease rotateEase;
    [SerializeField] private PlayerAnimator playerAnimator;

    void Start()
    {
        if (playerAnimator == null)
            playerAnimator = GetComponent<PlayerAnimator>();
        // Store the original position of the player cube
        originalPosition = transform.position;
        GameManager.Instance.OnVehicleSpawned += OnVehicleSpawned;
        GameEventManager.OnCubePassedCenter += OnCubePassedCenter;
    }

    private void OnCubePassedCenter()
    {
        currentTarget = null;
    }

    private void OnVehicleSpawned(object sender, GameManager.VehicleSpawnedEventArgs e)
    {
        if (currentTarget == null)
        {
            currentTarget = e.cubeController.transform;
            RotateTowardsTarget(currentTarget);
        }
    }

    // Smoothly rotate the player towards the target
    private void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Ignore vertical rotation

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate using DoTween over 0.5 seconds
            transform.DORotateQuaternion(targetRotation, 0.5f).SetEase(rotateEase);
        }
    }

    void Update()
    {
        //Detect tap/ click anywhere on the screen
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button (or touch on mobile)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (!isJumping)
        {
            StartCoroutine(PerformJump());

        }
    }

    IEnumerator PerformJump()
    {
        isJumping = true;

        playerAnimator?.TriggerJumpAnimation();

        Vector3 startPosition = transform.position;
        Vector3 endPosition = originalPosition + Vector3.up * JumpHeight;

        float elapsedTime = 0f;

        // Move up
        while (elapsedTime < JumpDuration / 2)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / (JumpDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Move down
        elapsedTime = 0f;
        while (elapsedTime < JumpDuration / 2)
        {
            transform.position = Vector3.Lerp(endPosition, originalPosition, elapsedTime / (JumpDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the player cube returns to the original position
        transform.position = originalPosition;

        isJumping = false;
    }
}