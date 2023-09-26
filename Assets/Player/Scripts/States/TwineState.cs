using States;
using UnityEngine;

namespace Player.Scripts.States
{
    public class TwineState : StateBase<PlayerController>
    {
        public TwineState(PlayerController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            Controller.PlayerAnimationController.TriggerAnimation(PlayerStates.Twine);
            Controller.Rb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
        }

        public override void Update()
        {
            var velocity = Controller.transform.forward.normalized * (Controller.GetTwineMoveSpeed());
            velocity.x = PlayerController.HorizontalOnTwineSensitivityMultiplier *
                         Controller.GetHorizontalSensitivity() * Controller.InputPlayer.x * Controller.GetTwineMoveSpeed();

            Controller.Move(velocity, false);

            if (Mathf.Abs(Controller.transform.rotation.z) >= Mathf.Deg2Rad * PlayerController.MaxAngleToFallOnTwin || Controller.Rb.velocity.y < -7f)
            {
                Controller.SetState(PlayerStates.Death);
            }
        }

        public override void Exit()
        {
            Controller.Rb.constraints |= RigidbodyConstraints.FreezeRotationZ;
        }
    }
}
 