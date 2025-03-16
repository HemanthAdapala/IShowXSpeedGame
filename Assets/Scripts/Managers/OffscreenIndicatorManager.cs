using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class OffscreenIndicatorManager : MonoBehaviour
    {
        [Header("References")]
        public Camera mainCamera; // Reference to the main camera
        public GameObject offscreenIndicatorPrefab; // Reference to the indicator prefab
        public Canvas targetCanvas; // Reference to the canvas

        [Header("Settings")]
        public float edgePadding = 50f; // Padding from the edge of the screen
        public float indicatorScale = 1f; // Scale of the indicator

        // Create an indicator for a target
        public GameObject CreateIndicator(Transform target)
        {
            if (offscreenIndicatorPrefab == null || mainCamera == null || targetCanvas == null)
            {
                Debug.LogError("OffscreenIndicatorManager: Assign MainCamera, OffscreenIndicatorPrefab, and TargetCanvas in the Inspector.");
                return null;
            }

            // Instantiate the indicator inside the canvas
            GameObject indicator = Instantiate(offscreenIndicatorPrefab, targetCanvas.transform);

            // Add the OffscreenIndicator script to the indicator
            OffscreenIndicator offscreenIndicator = indicator.AddComponent<OffscreenIndicator>();
            offscreenIndicator.MainCamera = mainCamera;
            offscreenIndicator.Target = target;
            offscreenIndicator.EdgePadding = edgePadding;
            offscreenIndicator.IndicatorScale = indicatorScale;

            return indicator;
        }

        // Destroy an indicator
        public void DestroyIndicator(GameObject indicator)
        {
            if (indicator is not null)
            {
                Destroy(indicator);
                //GameManager.Instance.DestroyCurrentTarget();
            }
        }
    }
}