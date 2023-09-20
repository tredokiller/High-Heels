using System;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerAnimationController
    {
        private readonly PlayerController _playerController;
        private readonly Animator _animator;

        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int WinDancing = Animator.StringToHash("WinDancing");
        private static readonly int WinWalking = Animator.StringToHash("WinWalking");

        public PlayerAnimationController(PlayerController playerController)
        {
            _playerController = playerController ? playerController : throw new ArgumentNullException(nameof(playerController));
            _animator = _playerController.PlayerAnimator;
        }

        public void TriggerAnimation(PlayerStates state)
        {
            switch (state)
            {
                case PlayerStates.Walking:
                    _animator.SetTrigger(Walking);
                    break;
                case PlayerStates.WinDancing:
                    _animator.SetTrigger(WinDancing);
                    break;
                case PlayerStates.WinWalking:
                    _animator.SetTrigger(WinWalking);
                    break;
            }
        }
    }
}
