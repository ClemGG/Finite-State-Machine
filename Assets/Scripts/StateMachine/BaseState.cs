
namespace Project.StateMachine
{
    /// <summary>
    /// The base interface all other States will inherit from.
    /// </summary>
    /// <typeparam name="TContext">The script holding all the values to edit.</typeparam>
    /// <typeparam name="TInput">The Type of the InputSystem to use (Keyboard, Controller, etc.)</typeparam>
    public abstract class BaseState<TContext, TInput>
    {
        #region Protected Fields
        protected bool _isRootState { private get; set; } = false;
        protected TContext _ctx { get; private set; }
        protected TInput _input { get; private set; }
        protected StateMachineFactory<TContext, TInput> _factory { get; private set; }
        private BaseState<TContext, TInput> _curSubState { get; set; }
        private BaseState<TContext, TInput> _curSuperState { get; set; }

        #endregion

        #region Constructor & IPooled

        public BaseState(TContext context, TInput input, StateMachineFactory<TContext, TInput> factory)
        {
            _ctx = context;
            _input = input;
            _factory = factory;
        }

        //To init the State in case TContext and TInput aren't set properly
        public virtual void OnEnqueued(TContext context, TInput input) { }
        public virtual void OnDequeued(TContext context, TInput input)
        {
            _ctx = context;
            _input = input;
        }

        #endregion


        #region State Methods

        protected virtual void Enter()
        {
            InitSubState();
        }
        protected virtual void Exit()
        {
            _factory.ReturnState(_ctx, _input, this);
        }
        protected virtual void Update()
        {
            CheckSwitchStates();
        }
        protected virtual void FixedUpdate() 
        {

        }
        ///Changes the current state (root or not) when all conditions are met
        protected abstract void CheckSwitchStates();   
        ///Assigns a SubState if needed
        protected abstract void InitSubState();    



        public void EnterStates()
        {
            Enter();
            if (_curSubState != null)
            {
                _curSubState.EnterStates();
            }
        }
        public void ExitStates()
        {
            if (_curSubState != null)
            {
                _curSubState.ExitStates();
            }
            Exit();
        }
        public void UpdateStates()
        {
            Update();
            if (_curSubState != null)
            {
                _curSubState.UpdateStates();
            }
        }
        public void FixedUpdateStates()
        {
            FixedUpdate();
            if (_curSubState != null)
            {
                _curSubState.FixedUpdateStates();
            }
        }
        protected void SwitchState(BaseState<TContext, TInput> newState)
        {
            //Quits the current state
            ExitStates();

            //Enters the new state
            newState.EnterStates();

            //If it is a RootState, we directly assign it to the _ctx
            if (_isRootState)
            {
                //curState is _ctx.CurState
                _factory.CurState = newState;
            }
            //Otherwise, we transfer that state's SuperState to the new state
            else if(_curSuperState != null)
            {
                _curSuperState.SetSubState(newState);
            }

        }
        protected void SetSubState(BaseState<TContext, TInput> newSubState)
        {
            _curSubState = newSubState;
            newSubState.SetSuperState(this);
            newSubState.Enter();
        }
        private void SetSuperState(BaseState<TContext, TInput> newSuperState)
        {
            _curSuperState = newSuperState;
        }

        #endregion
    }
}