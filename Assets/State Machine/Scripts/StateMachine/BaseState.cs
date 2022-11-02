using System.Text;

namespace Project.StateMachine
{
    /// <summary>
    /// L'�tat de base dont tous les autres �tats devront d�river
    /// </summary>
    /// <typeparam name="TContext">Contient les valeurs � lire et �diter.</typeparam>
    /// <typeparam name="TInput">Le type d'InputSystem � utilsier (Clavier, Manette, etc.)</typeparam>
    public abstract class BaseState<TContext, TInput> : State
    {
        #region Variables d'instance

        /// <summary>
        /// Contient les valeurs � lire et �diter
        /// </summary>
        protected TContext Ctx { get; private set; }

        /// <summary>
        /// Lit les actions du joueur
        /// </summary>
        protected TInput Input { get; private set; }

        /// <summary>
        /// Instancie et active les �tats
        /// </summary>
        protected StateMachine<TContext, TInput> Factory { get; private set; }

        /// <summary>
        /// L'�tat suivant dans la hi�rarchie
        /// </summary>
        private BaseState<TContext, TInput> SubState { get; set; }

        /// <summary>
        /// L'�tat pr�c�dent dans la hi�rarchie
        /// </summary>
        private BaseState<TContext, TInput> SuperState { get; set; }

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
            Ctx = context;
            Input = input;
            Factory = factory;
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
            if (SubState != null)
            {
                SubState.ExitStates();
            }
            Exit();

            Factory.ReturnState(this);
        }

        /// <summary>
        /// M�j l'�tat depuis l'Update() de la classe cliente
        /// </summary>
        public void UpdateStates()
        {
            Update();
            if (SubState != null)
            {
                SubState.UpdateStates();
            }
            CheckSwitchStates();
        }

        /// <summary>
        /// M�j l'�tat depuis la FixedUpdate() de la classe cliente
        /// </summary>
        public void FixedUpdateStates()
        {
            FixedUpdate();
            if (SubState != null)
            {
                SubState.FixedUpdateStates();
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

            if(SubState != null)
            {
                sb.Append("/");
                SubState.ToString(sb);
            }

            return sb.ToString();
        }

        /// <summary>
        /// R�cup�re l'�tat tout en bas de la hi�rarchie et renvoie TRUE s'il est du type renseign�
        /// </summary>
        public bool IsInState<TState>() where TState : BaseState<TContext, TInput>
        {
            if(SubState != null)
            {
                return SubState.IsInState<TState>();
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
                Factory.Start<TState>();
            }
            //Sinon, on transf�re notre super-�tat � ce nouvel �tat
            else if (SuperState != null)
            {
                SuperState.SetSubState<TState>();
            }

        }

        /// <summary>
        /// Assigne un sosu-�tat � cet �tat
        /// </summary>
        /// <typeparam name="TState">L'�tat du sous-�tat</typeparam>
        protected void SetSubState<TState>() where TState : BaseState<TContext, TInput>, new()
        {
            BaseState<TContext, TInput> newSubState = Factory.GetState<TState>();
            SubState = newSubState;
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
            SuperState = newSuperState;
        }


        #endregion
    }
}