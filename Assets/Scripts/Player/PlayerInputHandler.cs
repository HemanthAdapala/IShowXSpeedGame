using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private Ease ease = Ease.InCubic;

    private PlayerController _playerController;
    private bool _isJumping = false;


    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        
    }

    private void TryJump()
    {
        _playerController.PlayerAnimator.TriggerJump();
    }

}