using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerAnimationsDataConfig playerAnimationsDataConfig;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void TriggerJumpAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(playerAnimationsDataConfig.GetRandomJumpAnimationTrigger());
        }
    }

    public void TriggerCelebrationAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(playerAnimationsDataConfig.GetRandomCelebrationAnimationTrigger());
        }
    }
}
