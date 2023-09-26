using System;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerAnimationController
    {
        private readonly Animator _animator;

        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int WinDancing = Animator.StringToHash("WinDancing");
        private static readonly int WinWalking = Animator.StringToHash("WinWalking");
        private static readonly int Idling = Animator.StringToHash("Idling");
        private static readonly int Twine = Animator.StringToHash("Twine");
        private static readonly int StickWalking = Animator.StringToHash("StickWalking");

        public PlayerAnimationController(PlayerController playerController)
        {
            var playerContr = playerController ? playerController : throw new ArgumentNullException(nameof(playerController));
            _animator = playerContr.PlayerAnimator;
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
                case PlayerStates.Idling:
                    _animator.SetTrigger(Idling);
                    break;
                case PlayerStates.Twine:
                    _animator.SetTrigger(Twine);
                    break;
                case PlayerStates.StickWalking:
                    _animator.SetTrigger(StickWalking);
                    break;
            }
        }
    }
}
