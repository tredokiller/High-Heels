using System;
using States;
using UnityEngine;

namespace Player.Scripts.States
{
    public class WalkState : StateBase<PlayerController>
    {
        private readonly Transform theLowestPoint;
        private Rigidbody rb;
        
        public WalkState(PlayerController controller) : base(controller)
        {
            theLowestPoint = Controller.GetTheLowestPoint();
            rb = Controller.Rb;
        }
    
        public override void Enter()
        {
            Controller.PlayerAnimationController.TriggerAnimation(PlayerStates.Walking);
        }

        public override void Update()
        {
            Move();
        }

        private void Move()
        {
            var velocity = Controller.transform.forward.normalized * (Controller.GetMoveSpeed());

            if (Physics.Raycast(theLowestPoint.position, Vector3.down, 0.7f))
            {
                rb.constraints |= RigidbodyConstraints.FreezePositionY;
            }
            else
            {
                rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            }
            
            if (Mathf.Abs(Controller.InputPlayer.x) > 0.0f)
            {
                velocity.x = Controller.GetHorizontalSensitivity() * Controller.InputPlayer.x * Controller.GetMoveSpeed();
            }
            rb.AddForce(velocity - rb.velocity);
        }

        public override void Exit()
        {
            
        }

    
    }
}
