using UnityEngine;

public class OffscreenIndicatorManager : MonoBehaviour
{
    [Header("References")]
    public Camera MainCamera; // Reference to the main camera
    public GameObject OffscreenIndicatorPrefab; // Reference to the indicator prefab
    public Canvas TargetCanvas; // Reference to the canvas

    [Header("Settings")]
    public float EdgePadding = 50f; // Padding from the edge of the screen
    public float IndicatorScale = 1f; // Scale of the indicator

    // Create an indicator for a target
    public GameObject CreateIndicator(Transform target)
    {
        if (OffscreenIndicatorPrefab == null || MainCamera == null || TargetCanvas == null)
        {
            Debug.LogError("OffscreenIndicatorManager: Assign MainCamera, OffscreenIndicatorPrefab, and TargetCanvas in the Inspector.");
            return null;
        }

        // Instantiate the indicator inside the canvas
        GameObject indicator = Instantiate(OffscreenIndicatorPrefab, TargetCanvas.transform);

        // Add the OffscreenIndicator script to the indicator
        OffscreenIndicator offscreenIndicator = indicator.AddComponent<OffscreenIndicator>();
        offscreenIndicator.MainCamera = MainCamera;
        offscreenIndicator.Target = target;
        offscreenIndicator.EdgePadding = EdgePadding;
        offscreenIndicator.IndicatorScale = IndicatorScale;

        return indicator;
    }

    // Destroy an indicator
    public void DestroyIndicator(GameObject indicator)
    {
        if (indicator != null)
        {
            Destroy(indicator);
        }
    }
}