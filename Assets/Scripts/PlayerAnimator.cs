using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;


    [Header("Animation Parameters")]
    public string JumpTrigger = "Jump";

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void TriggerJumpAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(JumpTrigger);
        }
    }
}
