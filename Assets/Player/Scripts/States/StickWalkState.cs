using System;
using States;
using UnityEngine;

namespace Player.Scripts.States
{
    public class StickWalkState : StateBase<PlayerController>
    {
        private readonly PlayerIKController _playerIKController;
        private float _zRotation;
        
        
        public StickWalkState(PlayerController controller , PlayerIKController playerIKController) : base(controller)
        {
            _playerIKController = playerIKController ? playerIKController : throw new ArgumentNullException(nameof(playerIKController));
        }

        public override void Enter()
        {
            Controller.PlayerAnimationController.TriggerAnimation(PlayerStates.StickWalking);
            Controller.OnPlayerStickWalkingStarted.Invoke();
        }

        public override void Update()
        {
            var velocity = Controller.transform.forward.normalized * (Controller.GetStickMoveSpeed());
            var horizontalSwipeValue = Controller.GetHorizontalSensitivity() * Controller.InputPlayer.x;

            if (horizontalSwipeValue != 0)
            {
                _zRotation = Mathf.Lerp(_zRotation, horizontalSwipeValue, 2.2f * Time.deltaTime);
            }
            _zRotation = Mathf.Lerp(_zRotation, Controller.GetMaxRootRotationZAngleToFall(), 1.7f * Time.deltaTime);

            _playerIKController.UpdateRootRotationZ(_zRotation);
            Controller.Move(velocity, false);
            
            if (!Controller.IsGrounded() || Mathf.Abs(_playerIKController.GetCurrentRootRotatorZAngle()) > Controller.GetMaxRootRotationZAngleToFall() - 1)
            {
                Controller.SetState(PlayerStates.Death);
            }
        }

        public override void Exit()
        {
            Controller.OnPlayerStickWalkingFinished.Invoke();
            _playerIKController.UpdateRootRotationZ(0);
        }
    }
}
 