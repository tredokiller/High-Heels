using States;
using UnityEngine;

namespace Player.Scripts.States
{
    public class TwineState : StateBase<PlayerController>
    {
        public TwineState(PlayerController controller) : base(controller) { }
        
        public override void Enter()
        {
            Controller.PlayerAnimationController.TriggerAnimation(PlayerStates.Twine);
            Controller.Rb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
        }

        public override void Update()
        {
            var velocity = Controller.transform.forward.normalized * (Controller.GetMoveSpeed());
            velocity.x = PlayerController.HorizontalOnTwineSensitivityMultiplier * Controller.GetHorizontalSensitivity() * Controller.InputPlayer.x * Controller.GetMoveSpeed();
            
            Controller.Move(velocity , true);
        }

        public override void Exit()
        {
            Controller.Rb.constraints |= RigidbodyConstraints.FreezeRotationZ;
        }
    }
}
 