using System.Collections.Generic;
using States;
using UnityEngine;

namespace Player.Scripts.States
{
    public class WinWalkState : StateBase<PlayerController>
    {
        private readonly Rigidbody rb;

        public WinWalkState(PlayerController controller) : base(controller) { }
        
        public override void Enter()
        {
            Controller.OnPlayerWon.Invoke(PlayerStates.WinWalking);
            Controller.PlayerAnimationController.TriggerAnimation(PlayerStates.WinWalking);
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            
        }

    
    }
}
 