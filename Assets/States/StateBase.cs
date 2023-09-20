using System;

namespace States
{
    public abstract class StateBase<T> : IState
    {
        protected readonly T Controller;

        protected StateBase(T controller)
        {
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        public abstract void Enter();

        public abstract void Update();

        public abstract void Exit();
    }
}
