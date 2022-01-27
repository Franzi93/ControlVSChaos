namespace Dmdrn
{
    public class StateMachine<T> where T : class
    {
        public class State
        {
            public StateMachine<T> stateMachine;
            public T owner => stateMachine.owner;

            virtual public void OnEnter() { }
            virtual public void OnExit() { }
            virtual public void Update() { }
        }

        public T owner;
        private State state;

        public StateMachine(T owner)
        {
            this.owner = owner;
        }

        public bool IsInState(State state)
        {
            return this.state == state;
        }

        public State GetCurrentState()
        {
            return state;
        }

        public TState NewState<TState>() where TState : State, new()
        {
            TState newState = new TState
            {
                stateMachine = this
            };

            return newState; 
        }


        public void SetState(State newState)
        {
            if(state != null)
            {
                state.OnExit();
            }

            state = newState;

            state.OnEnter();
        }


        public void Update()
        {
            state.Update();
        }
    }
}