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
        /// Contient les valeurs � lire et �diter
        /// </summary>
        protected TContext _ctx { get; private set; }

        /// <summary>
        /// Lit les actions du joueur
        /// </summary>
        protected TInput _input { get; private set; }

        /// <summary>
        /// Instancie et active les �tats
        /// </summary>
        protected StateMachine<TContext, TInput> _factory { get; private set; }

        /// <summary>
        /// L'�tat suivant dans la hi�rarchie
        /// </summary>
        private BaseState<TContext, TInput> _curSubState { get; set; }

        /// <summary>
        /// L'�tat pr�c�dent dans la hi�rarchie
        /// </summary>
        private BaseState<TContext, TInput> _curSuperState { get; set; }

        #endregion

        #region Constructeurs

        /// <summary>
        /// Sets TContext, TInput and the factory manually when we retrieve a state from the factory
        /// </summary>
        /// <param name="context">Contient les valeurs � lire et �diter</param>
        /// <param name="input">Lit les actions du joueur</param>
        /// <param name="factory"> Instancie et active les �tats</param>
        public void SetContextAndInput(TContext context, TInput input, StateMachine<TContext, TInput> factory)
        {
            _ctx = context;
            _input = input;
            _factory = factory;
        }

        #endregion

        #region Fonctions publiques

        /// <summary>
        /// Entre dans l'�tat pour l'initialiser
        /// </summary>
        public void EnterStates()
        {
            InitSubState();
            Enter();
        }

        /// <summary>
        /// Quitte l'�tat et le renvoie dans la machine
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
        /// M�j l'�tat depuis l'Update() de la classe cliente
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
        /// M�j l'�tat depuis la FixedUpdate() de la classe cliente
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
        /// Affiche le nom de tous les �tats de la hi�rarchie
        /// </summary>
        /// <param name="sb">Le StringBuilder, � laisser null</param>
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
        /// R�cup�re l'�tat tout en bas de la hi�rarchie et renvoie TRUE s'il est du type renseign�
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

        #region Fonctions prot�g�es

        /// <summary>
        /// Change l'�tat actuel (racine ou non) 
        /// lorsque les conditions impl�ment�es dans la classe fille sont remplies
        /// </summary>
        protected virtual void CheckSwitchStates() { }

        /// <summary>
        /// Assigne un sous-�tat si besoin
        /// </summary>
        protected virtual void InitSubState() { }

        /// <summary>
        /// Echange l'�tat actuel par un autre du type renseign�
        /// </summary>
        /// <typeparam name="TState">Le type du nouvel �tat</typeparam>
        protected void SwitchState<TState>() where TState : BaseState<TContext, TInput>, new()
        {
            // Quitte l'�tat actuel ainsi que tous ses sous-�tats
            ExitStates();

            // Si c'est un �tat racine, on d�marre une nouvelle hi�rarchie depuis la machine
            if (IsRootState)
            {
                _factory.Start<TState>();
            }
            //Sinon, on transf�re notre super-�tat � ce nouvel �tat
            else if (_curSuperState != null)
            {
                _curSuperState.SetSubState<TState>();
            }

        }

        /// <summary>
        /// Assigne un sosu-�tat � cet �tat
        /// </summary>
        /// <typeparam name="TState">L'�tat du sous-�tat</typeparam>
        protected void SetSubState<TState>() where TState : BaseState<TContext, TInput>, new()
        {
            BaseState<TContext, TInput> newSubState = _factory.GetState<TState>();
            _curSubState = newSubState;
            newSubState.SetSuperState(this);
            newSubState.EnterStates();
        }

        #endregion

        #region Fonctions priv�es

        /// <summary>
        /// Assigne un super-�tat � celui-ci
        /// </summary>
        /// <param name="newSuperState">Le super-�tat qui dirige celui-ci</param>
        private void SetSuperState(BaseState<TContext, TInput> newSuperState)
        {
            _curSuperState = newSuperState;
        }


        #endregion
    }
}