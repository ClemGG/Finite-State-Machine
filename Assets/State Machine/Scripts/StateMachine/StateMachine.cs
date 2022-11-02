using Project.Pool;

namespace Project.StateMachine
{

    /// <summary>
    /// Chargé d'instancier et enregistrer les états de notre machine à états
    /// </summary>
    /// <typeparam name="TContext">Contient les valeurs à lire et éditer.</typeparam>
    /// <typeparam name="TInput">Le type d'InputSystem à utilsier (Clavier, Manette, etc.)</typeparam>
    public class StateMachine<TContext, TInput>
    {
        #region Variables d'instance

        /// <summary>
        /// L'état racine actuel
        /// </summary>
        private BaseState<TContext, TInput> CurRootState { get; set; }

        /// <summary>
        /// Contient les valeurs à lire et éditer
        /// </summary>
        private TContext Ctx { get; set; }

        /// <summary>
        /// Lit les actions du joueur
        /// </summary>
        private TInput Input { get; set; }

        /// <summary>
        /// La réserve contenant les états
        /// </summary>
        private ClassPooler<State> StatesPooler { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// A utiliser si plusieurs machines utilisent le même ClassPooler
        /// </summary>
        public StateMachine(TContext context, TInput input, ClassPooler<State> statesPooler)
        {
            Ctx = context;
            Input = input;
            StatesPooler = statesPooler;
        }

        /// <summary>
        /// A utiliser si la machine a besoin de son propre ClassPooler
        /// </summary>
        public StateMachine(TContext context, TInput input, params IPool<State>[] pools)
        {
            Ctx = context;
            Input = input;
            StatesPooler = new ClassPooler<State>(pools);
        }

        #endregion

        #region Mono

        /// <summary>
        /// Crée une nouvelle hiérarchie d'états
        /// </summary>
        /// <typeparam name="TState">Le type de l'état racine</typeparam>
        public void Start<TState>() where TState : BaseState<TContext, TInput>, new()
        {
            CurRootState = GetState<TState>();
            CurRootState.EnterStates();
        }

        /// <summary>
        /// Callback appelé quand on màj la machine dans une méthode Update()
        /// </summary>
        public void Update()
        {
            CurRootState.UpdateStates();
        }

        /// <summary>
        /// Callback appelé quand on màj la machine dans une méthode FixedUpdate()
        /// </summary>
        public void FixedUpdate()
        {
            CurRootState.FixedUpdateStates();
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Récupère un état de la réserve pour l'ajouter à la hiérarchie
        /// </summary>
        /// <typeparam name="TState">Le type de l'état à récupérer</typeparam>
        /// <param name="key">Le nom de l'état pour le retrouver dans la réserve, si besoin</param>
        public BaseState<TContext, TInput> GetState<TState>(string key = null) where TState : BaseState<TContext, TInput>, new()
        {
            TState state = StatesPooler.GetFromPool<TState>(key);
            state.SetContextAndInput(Ctx, Input, this);
            return state;
        }

        /// <summary>
        /// Renvoie l'état dans la réserve lorsqu'on ne l'utilise plus
        /// </summary>
        /// <param name="pooledState"></param>
        /// <param name="key"></param>
        public void ReturnState(State pooledState, string key = null)
        {
            StatesPooler.ReturnToPool(pooledState, key);
        }

        /// <summary>
        /// Affiche le nom de tous les états de la hiérarchie
        /// </summary>
        /// <param name="sb">Le StringBuilder, à laisser null</param>
        /// <returns>Une string au format "Etat1/Etat2/etc."</returns>
        public string GetCurStateHierarchy()
        {
            return CurRootState.ToString();
        }

        /// <summary>
        /// Récupère l'état tout en bas de la hiérarchie et renvoie TRUE s'il est du type renseigné
        /// </summary>
        public bool IsInState<TState>() where TState : BaseState<TContext, TInput>
        {
            return CurRootState.IsInState<TState>();
        }

        #endregion

    }
}