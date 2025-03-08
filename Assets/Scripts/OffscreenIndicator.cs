using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class OffscreenIndicator : MonoBehaviour
{
    [Header("References")]
    public Camera MainCamera; // Reference to the main camera
    public RectTransform Indicator; // Reference to the 2D indicator (UI Image)
    public Transform Target; // The target object to track

    [Header("Settings")]
    public float EdgePadding = 50f; // Padding from the edge of the screen
    public float IndicatorScale = 1f; // Scale of the indicator

    private RectTransform canvasRect;

    void Start()
    {
        // Automatically resolve references if not assigned
        ResolveReferences();

        // Validate references
        if (Indicator == null || MainCamera == null || Target == null)
        {
            Debug.LogError("OffscreenIndicator: Required references are missing. Please assign them or ensure the setup is correct.");
            enabled = false;
            return;
        }

        // Get the canvas rect (assuming the indicator is inside a canvas)
        canvasRect = Indicator.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // Set the initial scale of the indicator
        Indicator.localScale = Vector3.one * IndicatorScale;
    }

    void ResolveReferences()
    {
        // Automatically find the main camera if not assigned
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
            if (MainCamera == null)
            {
                Debug.LogError("OffscreenIndicator: No main camera found. Please assign a camera.");
            }
        }

        // Automatically find the indicator if not assigned
        if (Indicator == null)
        {
            Indicator = GetComponent<RectTransform>();
            if (Indicator == null)
            {
                Debug.LogError("OffscreenIndicator: No indicator (RectTransform) found. Please assign an indicator.");
            }
        }

        // Automatically find the target if not assigned
        if (Target == null)
        {
            // Try to find the target in the parent object
            Target = transform.parent;
            if (Target == null)
            {
                Debug.LogError("OffscreenIndicator: No target found. Please assign a target.");
            }
        }
    }

    void Update()
    {
        if (Target == null)
        {
            Indicator.gameObject.SetActive(false);
            return;
        }

        // Get the target's position in viewport space
        Vector3 screenPos = MainCamera.WorldToViewportPoint(Target.position);

        // Check if the target is off-screen
        bool isOffScreen = screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1;

        if (isOffScreen)
        {
            // Enable the indicator
            Indicator.gameObject.SetActive(true);

            // Calculate the direction from the target to the center of the screen
            Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0); // Center of the screen in viewport space
            Vector3 direction = (screenCenter - screenPos).normalized;

            // Calculate the indicator's rotation to point towards the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Indicator.rotation = Quaternion.Euler(0, 0, angle);

            // Clamp the screen position to the edges
            screenPos.x = Mathf.Clamp(screenPos.x, 0.05f, 0.95f);
            screenPos.y = Mathf.Clamp(screenPos.y, 0.05f, 0.95f);

            // Convert viewport position to canvas position
            Vector2 canvasPos = new Vector2(
                (screenPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
                (screenPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)
            );

            // Apply padding
            canvasPos = Vector2.ClampMagnitude(canvasPos, canvasRect.sizeDelta.magnitude * 0.5f - EdgePadding);

            // Set the indicator's position
            Indicator.anchoredPosition = canvasPos;
        }
        else
        {
            // Disable the indicator when the target is on-screen
            Indicator.gameObject.SetActive(false);
        }
    }
}