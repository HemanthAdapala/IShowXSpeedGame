using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Managers;

public class CircleCalculator : MonoBehaviour
{
    public float Radius = 5f;
    public int Segments = 36;
    public bool ShowSegments = true; // Toggle segment lines
    public GameObject CubePrefab; // Prefab for the cube to place at midpoints
    public float MinMoveTime = 1f;
    public float MaxMoveTime = 5f;

    [Header("Offscreen Indicators")]
    public OffscreenIndicatorManager IndicatorManager; // Reference to the OffscreenIndicatorManager

    [SerializeField] private LineRenderer circleRenderer;
    [SerializeField] private LineRenderer segmentRenderer;
    private GameObject[] segmentCubes; // Array to store cubes at segment midpoints

    void Start()
    {
        if (circleRenderer == null || segmentRenderer == null)
        {
            Debug.LogError("LineRenderers are not assigned. Please assign them in the Inspector.");
            return;
        }

        circleRenderer.useWorldSpace = false;
        circleRenderer.loop = true;
        circleRenderer.positionCount = Segments;

        segmentRenderer.useWorldSpace = false;
        segmentRenderer.positionCount = Segments * 2;

        segmentCubes = new GameObject[Segments];

        GenerateCircle();
        StartCoroutine(MoveAllCubesSequentially());
    }

    void GenerateCircle()
    {
        if (circleRenderer == null || segmentRenderer == null)
        {
            Debug.LogError("LineRenderers are not assigned. Please assign them in the Inspector.");
            return;
        }

        Vector3[] circlePoints = new Vector3[Segments];
        Vector3[] segmentPoints = new Vector3[Segments * 2];

        for (int i = 0; i < Segments; i++)
        {
            float angle = 2 * Mathf.PI * i / Segments;
            float x = Mathf.Cos(angle) * Radius;
            float z = Mathf.Sin(angle) * Radius;
            circlePoints[i] = new Vector3(x, 0, z);
        }

        circleRenderer.SetPositions(circlePoints);

        if (ShowSegments)
        {
            segmentRenderer.enabled = true;
            for (int i = 0; i < Segments; i++)
            {
                segmentPoints[i * 2] = circlePoints[i];
                segmentPoints[i * 2 + 1] = circlePoints[(i + 1) % Segments];
            }
            segmentRenderer.SetPositions(segmentPoints);
        }
        else
        {
            segmentRenderer.enabled = false;
        }

        for (int i = 0; i < Segments; i++)
        {
            Vector3 startPoint = circlePoints[i];
            Vector3 endPoint = circlePoints[(i + 1) % Segments];
            Vector3 midpoint = (startPoint + endPoint) / 2f;

            if (CubePrefab != null)
            {
                GameObject cube = Instantiate(CubePrefab, transform);
                cube.transform.localPosition = midpoint;
                cube.transform.localScale = Vector3.one * 0.2f;
                segmentCubes[i] = cube; // Store the cube in the array

                // Create an indicator for this cube
                if (IndicatorManager != null)
                {
                    IndicatorManager.CreateIndicator(cube.transform);
                }
            }
        }
    }

    IEnumerator MoveAllCubesSequentially()
    {
        // Wait for a moment to ensure all cubes are initialized
        yield return new WaitForSeconds(1f);

        // Loop until all cubes are destroyed
        while (true)
        {
            // Find all remaining cubes
            GameObject[] remainingCubes = System.Array.FindAll(segmentCubes, cube => cube != null);

            // If no cubes are left, break the loop
            if (remainingCubes.Length == 0)
            {
                Debug.Log("All cubes have been destroyed.");
                break;
            }

            // Pick a random cube from the remaining cubes
            int randomIndex = Random.Range(0, remainingCubes.Length);
            GameObject cube = remainingCubes[randomIndex];

            if (cube != null)
            {
                // Calculate the opposite position
                Vector3 startPosition = cube.transform.localPosition;
                Vector3 oppositePosition = -startPosition; // Opposite point on the circle

                // Move the cube to the opposite position
                float moveTime = Random.Range(MinMoveTime, MaxMoveTime);
                yield return StartCoroutine(MoveCube(cube, startPosition, oppositePosition, moveTime));

                // Destroy the cube after it reaches the opposite position
                Destroy(cube);

                // Remove the cube from the segmentCubes array
                int cubeIndex = System.Array.IndexOf(segmentCubes, cube);
                if (cubeIndex >= 0)
                {
                    segmentCubes[cubeIndex] = null;
                }
            }
        }
    }

    IEnumerator MoveCube(GameObject cube, Vector3 start, Vector3 end, float moveTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            cube.transform.localPosition = Vector3.Lerp(start, end, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cube.transform.localPosition = end;
    }

    void OnValidate()
    {
        if (Application.isPlaying && circleRenderer != null && segmentRenderer != null)
            GenerateCircle();
    }
}