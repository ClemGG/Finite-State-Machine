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
        #region Variables d'instance

        /// <summary>
        /// Contient les valeurs à lire et éditer
        /// </summary>
        protected TContext _ctx { get; private set; }

        /// <summary>
        /// Lit les actions du joueur
        /// </summary>
        protected TInput _input { get; private set; }

        /// <summary>
        /// Instancie et active les états
        /// </summary>
        protected StateMachine<TContext, TInput> _factory { get; private set; }

        /// <summary>
        /// L'état suivant dans la hiérarchie
        /// </summary>
        private BaseState<TContext, TInput> _curSubState { get; set; }

        /// <summary>
        /// L'état précédent dans la hiérarchie
        /// </summary>
        private BaseState<TContext, TInput> _curSuperState { get; set; }

        #endregion

        #region Constructeurs

        /// <summary>
        /// Sets TContext, TInput and the factory manually when we retrieve a state from the factory
        /// </summary>
        /// <param name="context">Contient les valeurs à lire et éditer</param>
        /// <param name="input">Lit les actions du joueur</param>
        /// <param name="factory"> Instancie et active les états</param>
        public void SetContextAndInput(TContext context, TInput input, StateMachine<TContext, TInput> factory)
        {
            _ctx = context;
            _input = input;
            _factory = factory;
        }

        #endregion

        #region Fonctions publiques

        /// <summary>
        /// Entre dans l'état pour l'initialiser
        /// </summary>
        public void EnterStates()
        {
            InitSubState();
            Enter();
        }

        /// <summary>
        /// Quitte l'état et le renvoie dans la machine
        /// </summary>
        public void ExitStates()
        {
            if (_curSubState != null)
            {
                _curSubState.ExitStates();
            }
            Exit();

            _factory.ReturnState(this);
        }

        /// <summary>
        /// Màj l'état depuis l'Update() de la classe cliente
        /// </summary>
        public void UpdateStates()
        {
            Update();
            if (_curSubState != null)
            {
                _curSubState.UpdateStates();
            }
            CheckSwitchStates();
        }

        /// <summary>
        /// Màj l'état depuis la FixedUpdate() de la classe cliente
        /// </summary>
        public void FixedUpdateStates()
        {
            FixedUpdate();
            if (_curSubState != null)
            {
                _curSubState.FixedUpdateStates();
            }
        }

        /// <summary>
        /// Affiche le nom de tous les états de la hiérarchie
        /// </summary>
        /// <param name="sb">Le StringBuilder, à laisser null</param>
        /// <returns>Une string au format "Etat1/Etat2/etc."</returns>
        public string ToString(StringBuilder sb = null)
        {
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
        }

        /// <summary>
        /// Récupère l'état tout en bas de la hiérarchie et renvoie TRUE s'il est du type renseigné
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

        #region Fonctions protégées

        /// <summary>
        /// Change l'état actuel (racine ou non) 
        /// lorsque les conditions implémentées dans la classe fille sont remplies
        /// </summary>
        protected virtual void CheckSwitchStates() { }

        /// <summary>
        /// Assigne un sous-état si besoin
        /// </summary>
        protected virtual void InitSubState() { }

        /// <summary>
        /// Echange l'état actuel par un autre du type renseigné
        /// </summary>
        /// <typeparam name="TState">Le type du nouvel état</typeparam>
        protected void SwitchState<TState>() where TState : BaseState<TContext, TInput>, new()
        {
            // Quitte l'état actuel ainsi que tous ses sous-états
            ExitStates();

            // Si c'est un état racine, on démarre une nouvelle hiérarchie depuis la machine
            if (IsRootState)
            {
                _factory.Start<TState>();
            }
            //Sinon, on transfère notre super-état à ce nouvel état
            else if (_curSuperState != null)
            {
                _curSuperState.SetSubState<TState>();
            }

        }

        /// <summary>
        /// Assigne un sosu-état à cet état
        /// </summary>
        /// <typeparam name="TState">L'état du sous-état</typeparam>
        protected void SetSubState<TState>() where TState : BaseState<TContext, TInput>, new()
        {
            BaseState<TContext, TInput> newSubState = _factory.GetState<TState>();
            _curSubState = newSubState;
            newSubState.SetSuperState(this);
            newSubState.EnterStates();
        }

        #endregion

        #region Fonctions privées

        /// <summary>
        /// Assigne un super-état à celui-ci
        /// </summary>
        /// <param name="newSuperState">Le super-état qui dirige celui-ci</param>
        private void SetSuperState(BaseState<TContext, TInput> newSuperState)
        {
            _curSuperState = newSuperState;
        }


        #endregion
    }
}