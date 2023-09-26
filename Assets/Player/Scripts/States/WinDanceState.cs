using System.Collections.Generic;
using States;
using UnityEngine;

namespace Player.Scripts.States
{
    public class WinDanceState : StateBase<PlayerController>
    {
        private readonly Rigidbody rb;

        public WinDanceState(PlayerController controller) : base(controller) { }
        
        public override void Enter()
        {
            Controller.OnPlayerWon.Invoke(PlayerStates.WinDancing);
            Controller.PlayerAnimationController.TriggerAnimation(PlayerStates.WinDancing);
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            
        }

    
    }
}
 