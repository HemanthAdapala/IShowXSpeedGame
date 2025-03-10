using UnityEngine;
using DG.Tweening; // Import DOTween

public class TargetFinder : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private Ease rotationEase = Ease.InOutSine;
    [SerializeField] private bool useLocalRotation = false;

    private Tween rotationTween;

    // Reference to the target
    private Transform currentTarget;

    private void Update()
    {
        if (currentTarget != null)
        {
            // Calculate the direction to look at
            Vector3 targetDirection = currentTarget.position - transform.position;

            // Only rotate if we have a valid direction
            if (targetDirection != Vector3.zero)
            {
                // Create the target rotation
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // Kill any existing rotation tween to prevent conflicts
                if (rotationTween != null && rotationTween.IsActive())
                {
                    rotationTween.Kill();
                }

                // Use DOTween to smoothly rotate towards the target
                if (useLocalRotation)
                {
                    rotationTween = transform.DOLocalRotateQuaternion(targetRotation, rotationDuration)
                        .SetEase(rotationEase);
                }
                else
                {
                    rotationTween = transform.DORotateQuaternion(targetRotation, rotationDuration)
                        .SetEase(rotationEase);
                }
            }
        }
    }

    // Method to set the target
    public void SetTarget(Transform target)
    {
        currentTarget = target;

        if (currentTarget != null)
        {
            // Look at the new target
            transform.LookAt(currentTarget);
        }
        else
        {
            Debug.Log("Target is null, stopping look behavior.");
            // Reset rotation or look in a default direction
        }
    }


    // Clean up any active tweens when the object is destroyed
    private void OnDestroy()
    {
        if (rotationTween != null && rotationTween.IsActive())
        {
            rotationTween.Kill();
        }
    }
}