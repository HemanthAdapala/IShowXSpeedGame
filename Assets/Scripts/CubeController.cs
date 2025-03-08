using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour
{
    public OffscreenIndicatorManager IndicatorManager; // Reference to the OffscreenIndicatorManager
    public float MoveSpeed = 2f; // Speed of the cube (set by GameManager)

    private GameObject indicator; // Reference to the cube's indicator
    private Vector3 endPosition; // End position of the cube

    void Start()
    {
        // Calculate the end position (opposite side of the circle)
        endPosition = -transform.position;

        // Create the indicator when the cube starts moving
        if (IndicatorManager != null)
        {
            indicator = IndicatorManager.CreateIndicator(transform);
        }

        // Start moving the cube
        StartCoroutine(MoveCube());
    }

    IEnumerator MoveCube()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < 1f)
        {
            // Move the cube from start to end
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime);
            elapsedTime += Time.deltaTime * MoveSpeed;
            yield return null;
        }

        // Ensure the cube reaches the end position
        transform.position = endPosition;

        // Destroy the cube and its indicator
        if (IndicatorManager != null && indicator != null)
        {
            IndicatorManager.DestroyIndicator(indicator);
        }
        Destroy(gameObject);
    }
}