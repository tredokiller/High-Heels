using System.Collections.Generic;
using States;
using UnityEngine;

namespace Player.Scripts.States
{
    public class DeathState : StateBase<PlayerController>
    {
        private readonly Rigidbody rb;

        public DeathState(PlayerController controller) : base(controller) { }
        
        public override void Enter()
        {
            Controller.OnPlayerDiedHandler();
            
            Controller.PlayerAnimator.enabled = false;
            Controller.EnableRagDoll(true);
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
 