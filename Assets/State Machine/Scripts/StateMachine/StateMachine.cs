using Project.Pool;

namespace Project.StateMachine
{

    /// <summary>
    /// This class will be in charge of instantiating and pooling all States of our StateMachine.
    /// </summary>
    /// <typeparam name="TContext">The script holding all the values to edit.</typeparam>
    /// <typeparam name="TInput">The Type of the InputSystem to use (Keyboard, Controller, etc.)</typeparam>
    public class StateMachine<TContext, TInput>
    {
        #region Accessors

        private BaseState<TContext, TInput> CurState { get; set; }
        private TContext _ctx { get; set; }
        private TInput _input { get; set; }
        private ClassPooler<State> _statesPooler { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Use this if multiple factories use the same ClassPooler
        /// </summary>
        public StateMachine(TContext context, TInput input, ClassPooler<State> statesPooler)
        {
            _ctx = context;
            _input = input;
            _statesPooler = statesPooler;
        }

        /// <summary>
        /// Use this if this factory onle needs its own pools
        /// </summary>
        public StateMachine(TContext context, TInput input, params Pool<State>[] pools)
        {
            _ctx = context;
            _input = input;
            _statesPooler = new ClassPooler<State>(pools);
        }

        #endregion


        #region Mono


        public void Start<TState>() where TState : BaseState<TContext, TInput>, new()
        {
            CurState = GetState<TState>();
            CurState.EnterStates();
        }

        public void Update()
        {
            CurState.UpdateStates();
        }

        public void FixedUpdate()
        {
            CurState.FixedUpdateStates();
        }


        #endregion


        #region Accessors

        public BaseState<TContext, TInput> GetState<TState>(string key = null) where TState : BaseState<TContext, TInput>, new()
        {
            TState state = _statesPooler.GetFromPool<TState>(key);
            state.SetContextAndInput(_ctx, _input, this);
            return state;
        }

        public void ReturnState(State pooledState, string key = null)
        {
            _statesPooler.ReturnToPool(pooledState, key);
        }


        public string GetCurStateHierarchy()
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