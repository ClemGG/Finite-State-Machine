using Project.Pool;

namespace Project.StateMachine
{

    /// <summary>
    /// Charg� d'instancier et enregistrer les �tats de notre machine � �tats
    /// </summary>
    /// <typeparam name="TContext">Contient les valeurs � lire et �diter.</typeparam>
    /// <typeparam name="TInput">Le type d'InputSystem � utilsier (Clavier, Manette, etc.)</typeparam>
    public class StateMachine<TContext, TInput>
    {
        #region Variables d'instance

        /// <summary>
        /// L'�tat racine actuel
        /// </summary>
        private BaseState<TContext, TInput> CurRootState { get; set; }

        /// <summary>
        /// Contient les valeurs � lire et �diter
        /// </summary>
        private TContext Ctx { get; set; }

        /// <summary>
        /// Lit les actions du joueur
        /// </summary>
        private TInput Input { get; set; }

        /// <summary>
        /// La r�serve contenant les �tats
        /// </summary>
        private ClassPooler<State> StatesPooler { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// A utiliser si plusieurs machines utilisent le m�me ClassPooler
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
        /// Cr�e une nouvelle hi�rarchie d'�tats
        /// </summary>
        /// <typeparam name="TState">Le type de l'�tat racine</typeparam>
        public void Start<TState>() where TState : BaseState<TContext, TInput>, new()
        {
            CurRootState = GetState<TState>();
            CurRootState.EnterStates();
        }

        /// <summary>
        /// Callback appel� quand on m�j la machine dans une m�thode Update()
        /// </summary>
        public void Update()
        {
            CurRootState.UpdateStates();
        }

        /// <summary>
        /// Callback appel� quand on m�j la machine dans une m�thode FixedUpdate()
        /// </summary>
        public void FixedUpdate()
        {
            CurRootState.FixedUpdateStates();
        }

        #endregion

        #region Accessors

        /// <summary>
        /// R�cup�re un �tat de la r�serve pour l'ajouter � la hi�rarchie
        /// </summary>
        /// <typeparam name="TState">Le type de l'�tat � r�cup�rer</typeparam>
        /// <param name="key">Le nom de l'�tat pour le retrouver dans la r�serve, si besoin</param>
        public BaseState<TContext, TInput> GetState<TState>(string key = null) where TState : BaseState<TContext, TInput>, new()
        {
            TState state = StatesPooler.GetFromPool<TState>(key);
            state.SetContextAndInput(Ctx, Input, this);
            return state;
        }

        /// <summary>
        /// Renvoie l'�tat dans la r�serve lorsqu'on ne l'utilise plus
        /// </summary>
        /// <param name="pooledState"></param>
        /// <param name="key"></param>
        public void ReturnState(State pooledState, string key = null)
        {
            StatesPooler.ReturnToPool(pooledState, key);
        }

        /// <summary>
        /// Affiche le nom de tous les �tats de la hi�rarchie
        /// </summary>
        /// <param name="sb">Le StringBuilder, � laisser null</param>
        /// <returns>Une string au format "Etat1/Etat2/etc."</returns>
        public string GetCurStateHierarchy()
        {
            return CurRootState.ToString();
        }

        /// <summary>
        /// R�cup�re l'�tat tout en bas de la hi�rarchie et renvoie TRUE s'il est du type renseign�
        /// </summary>
        public bool IsInState<TState>() where TState : BaseState<TContext, TInput>
        {
            return CurRootState.IsInState<TState>();
        }

        #endregion

    }
}