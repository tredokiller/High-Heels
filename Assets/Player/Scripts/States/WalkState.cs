using System;
using DG.Tweening;
using States;
using UnityEngine;

namespace Player.Scripts.States
{
    public class WalkState : StateBase<PlayerController>
    {
        private readonly PlayerIKController _playerIKController;

        public WalkState(PlayerController controller, PlayerIKController playerIKController) : base(controller)
        {
            _playerIKController = playerIKController ? playerIKController : throw new ArgumentNullException(nameof(playerIKController));
        }

        private float lastHeightInState; 
        
        public override void Enter()
        {
            Controller.PlayerAnimationController.TriggerAnimation(PlayerStates.Walking);
            Controller.Rb.constraints |= RigidbodyConstraints.FreezeRotationZ;

            Controller.transform.DOMoveY(Controller.transform.position.y + (lastHeightInState - Controller.transform.position.y) , 
                PlayerController.DurationChangePositionY);

            SetRotationToDefault();
        }

        public override void Update()
        {
            var velocity = Controller.transform.forward.normalized * (Controller.GetMoveSpeed());
            velocity.x = Controller.GetHorizontalSensitivity() * Controller.InputPlayer.x * Controller.GetMoveSpeed();
            
            _playerIKController.UpdateSpineRotationZ(velocity.x);
            
            Controller.Move(velocity , true);
        }

        private void SetRotationToDefault()
        {
            var transform = Controller.transform;
            var rotation = transform.rotation;
            rotation.z = 0;

            transform.rotation = rotation;
        }

        public override void Exit()
        {
            lastHeightInState = Controller.transform.position.y;
        }
    }
}
 