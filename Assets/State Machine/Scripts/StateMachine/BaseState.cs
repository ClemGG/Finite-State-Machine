using System.Text;

namespace Project.StateMachine
{
    /// <summary>
    /// The base interface all other States will inherit from.
    /// </summary>
    /// <typeparam name="TContext">The script holding all the values to edit.</typeparam>
    /// <typeparam name="TInput">The Type of the InputSystem to use (Keyboard, Controller, etc.)</typeparam>
    public abstract class BaseState<TContext, TInput> : State
    {
        #region Protected Fields

        protected TContext _ctx { get; private set; }
        protected TInput _input { get; private set; }
        protected StateMachine<TContext, TInput> _factory { get; private set; }
        private BaseState<TContext, TInput> _curSubState { get; set; }
        private BaseState<TContext, TInput> _curSuperState { get; set; }

        #endregion


        #region Constructor & IPooled
        

        //Sets TContext, TInput and the factory manually when we retrieve a state from the factory
        public void SetContextAndInput(TContext context, TInput input, StateMachine<TContext, TInput> factory)
        {
            _ctx = context;
            _input = input;
            _factory = factory;
        }

        #endregion


        #region State Methods



        ///Changes the current state (root or not) when all conditions are met
        protected virtual void CheckSwitchStates() { }   
        ///Assigns a SubState if needed
        protected virtual void InitSubState() { }



        public void EnterStates()
        {
            InitSubState();
            Enter();
        }
        public void ExitStates()
        {
            if (_curSubState != null)
            {
                _curSubState.ExitStates();
            }
            Exit();

            _factory.ReturnState(this);
        }
        public void UpdateStates()
        {
            Update();
            if (_curSubState != null)
            {
                _curSubState.UpdateStates();
            }
            CheckSwitchStates();
        }
        public void FixedUpdateStates()
        {
            FixedUpdate();
            if (_curSubState != null)
            {
                _curSubState.FixedUpdateStates();
            }
        }

        

        protected void SwitchState<TState>() where TState : BaseState<TContext, TInput>, new()
        {
            //Quits the current state
            ExitStates();


            //If it is a RootState, we directly assign it to the _factory
            if (_isRootState)
            {
                //Enters the new state
                _factory.Start<TState>();
            }
            //Otherwise, we transfer that state's SuperState to the new state
            else if(_curSuperState != null)
            {
                _curSuperState.SetSubState<TState>();
            }

        }
        protected void SetSubState<TState>() where TState : BaseState<TContext, TInput>, new()
        {
            BaseState<TContext, TInput> newSubState = _factory.GetState<TState>();
            _curSubState = newSubState;
            newSubState.SetSuperState(this);
            newSubState.EnterStates();
        }
        private void SetSuperState(BaseState<TContext, TInput> newSuperState)
        {
            _curSuperState = newSuperState;
        }


        public string ToString(StringBuilder sb = null)
        {
#if UNITY_EDITOR
            if (sb is null)
            {
                sb = new StringBuilder(100);
            }
            sb.Append(GetType().Name);

            if(_curSubState != null)
            {
                sb.Append("/");
                _curSubState.ToString(sb);
            }

            return sb.ToString();
#endif
        }
        /// <summary>
        /// Retrives the bottommost state in the hierarchy and returns true if its type matches the parameter type.
        /// </summary>
        public bool IsInState<TState>() where TState : BaseState<TContext, TInput>
        {
            if(_curSubState != null)
            {
                return _curSubState.IsInState<TState>();
            }

            return this is TState;
        }

        #endregion
    }
}