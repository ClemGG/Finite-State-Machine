using Project.Pool;

namespace Project.StateMachine
{

    /// <summary>
    /// This class will be in charge of instantiating and pooling all States of our StateMachine
    /// </summary>
    /// <typeparam name="TContext">The script holding all the values to edit.</typeparam>
    /// <typeparam name="TInput">The Type of the InputSystem to use (Keyboard, Controller, etc.)</typeparam>
    public class StateMachine<TContext, TInput>
    {
        #region Accessors

        public BaseState<TContext, TInput> CurState { get; set; }
        private ClassPooler<BaseState<TContext, TInput>> _statesPooler { get; set; }

        #endregion


        #region Public Methods

        /// <summary>
        /// Uses the empty state constructors if this factory directly uses a ClassPooler from elsewhere
        /// </summary>
        public StateMachine(ClassPooler<BaseState<TContext, TInput>> statesPooler)
        {
            _statesPooler = statesPooler;
        }
        
        /// <summary>
        /// Uses the parametered state constructors if this factory creates its own pools
        /// </summary>
        public StateMachine(params Pool<BaseState<TContext, TInput>>[] pools)
        {
            _statesPooler = new ClassPooler<BaseState<TContext, TInput>>(pools);
        }


        public BaseState<TContext, TInput> GetState<TState>(TContext context, TInput input, string key = null) where TState : BaseState<TContext, TInput>, new()
        {
            TState state = _statesPooler.GetFromPool<TState>(key);
            state.SetContextAndInput(context, input, this);
            return state;
        }

        public void ReturnState(BaseState<TContext, TInput> pooledState, string key = null)
        {
            _statesPooler.ReturnToPool(pooledState, key);
        }

        public void Init<TState>(TContext context, TInput input) where TState : BaseState<TContext, TInput>, new()
        {
            CurState = GetState<TState>(context, input);
            CurState.EnterStates();
        }

        public string GetCurState()
        {
            return CurState.ToString();
        }

        /// <summary>
        /// Retrives the bottommost state in the hierarchy and returns true if its type matches the parameter type.
        /// </summary>
        public bool IsInState<TState>() where TState : BaseState<TContext, TInput>
        {
            return CurState.IsInState<TState>();
        }

        #endregion


    }
}