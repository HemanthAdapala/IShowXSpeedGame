using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float JumpHeight = 2f; // Configurable jump height
    public float JumpDuration = 0.5f; // Duration of the jump

    private bool isJumping = false; // Track if the player is currently jumping
    private Vector3 originalPosition; // Store the original position of the player cube

    void Start()
    {
        // Store the original position of the player cube
        originalPosition = transform.position;
    }

    void Update()
    {
        // Detect tap/click anywhere on the screen
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