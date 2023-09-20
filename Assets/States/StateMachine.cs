using UnityEngine;

namespace States
{
    public class StateMachine
    {
        private IState _state;

        public void SetState(IState state)
        {
            if (_state != null)
            {
                var previousState = _state;
                if (previousState != state)
                {
                    state.Enter();
                }
            }
            else
            {
                state.Enter();
            }
            _state = state;
        }

        public void UpdateStateMachine()
        {
            if (_state != null)
            {
                _state.Update();
            }
        }
    }
}
