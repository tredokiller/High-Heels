using States;

namespace Player.Scripts.States
{
    public class IdleState : StateBase<PlayerController>
    {
        
        public override void Enter()
        {
            Controller.PlayerAnimationController.TriggerAnimation(PlayerStates.Idling);
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
        }

        public IdleState(PlayerController controller) : base(controller)
        {
        }
    }
}
