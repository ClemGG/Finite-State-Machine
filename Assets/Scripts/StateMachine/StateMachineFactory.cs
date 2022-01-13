using Project.Pool;
using System;

namespace Project.StateMachine
{

    /// <summary>
    /// This class will be in charge of instantiating and pooling all States of our StateMachine
    /// </summary>
    /// <typeparam name="TContext">The script holding all the values to edit.</typeparam>
    /// <typeparam name="TInput">The Type of the InputSystem to use (Keyboard, Controller, etc.)</typeparam>
    public class StateMachineFactory<TContext, TInput>
    {
        #region Accessors

        public BaseState<TContext, TInput> CurState { get; set; }
        private ClassPooler<BaseState<TContext, TInput>> _statesPooler { get; set; }

        #endregion


        #region Public Methods

        public StateMachineFactory(TContext currentContext, params (string key, int defaultCapacity, Func<BaseState<TContext, TInput>> createFunc)[] pools)
        {
            _statesPooler = new ClassPooler<BaseState<TContext, TInput>>(pools);
        }


        public BaseState<TContext, TInput> GetState<TState>(TContext context, TInput input, string key = null) where TState : BaseState<TContext, TInput>, new()
        {
            TState state = _statesPooler.GetFromPool<TState>(key);
            state.OnDequeued(context, input);
            return state;
        }

        public void ReturnState(TContext context, TInput input, BaseState<TContext, TInput> pooledState, string key = null)
        {
            pooledState.OnEnqueued(context, input);
            _statesPooler.ReturnToPool(pooledState, key);
        }

        #endregion


    }
}