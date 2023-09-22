using States;

namespace Player.Scripts.States
{
    public class WalkState : StateBase<PlayerController>
    {
        public WalkState(PlayerController controller) : base(controller) { }
        
        public override void Enter()
        {
            Controller.PlayerAnimationController.TriggerAnimation(PlayerStates.Walking);
        }

        public override void Update()
        {
            var velocity = Controller.transform.forward.normalized * (Controller.GetMoveSpeed());
            velocity.x = Controller.GetHorizontalSensitivity() * Controller.InputPlayer.x * Controller.GetMoveSpeed();
            
            Controller.Move(velocity , true);
        }

        public override void Exit()
        {
            
        }

    
    }
}
 