using Project.StateMachine;
using Project.Tester.Inputs;

namespace Project.Tester
{
    public class RedCubeState : BaseState<StateMachineTester, TesterInput>
    {
        #region Constructor & IPooled

        public RedCubeState() : base(null, null, null)
        {

        }

        public RedCubeState(StateMachineTester context, TesterInput input, StateMachine<StateMachineTester, TesterInput> factory) : base(context, input, factory)
        {
            //Change _isRootState here
        }

        #endregion


        #region State Methods

        protected override void Enter()
        {
            //When the state is entered, we change the material's color to red.
            _ctx.Renderer.sharedMaterial.SetColor("_Color", _ctx._cubeColor);
            base.Enter();
        }
        protected override void Exit()
        {
            base.Exit();
        }
        protected override void Update()
        {
            base.Update();
        }
        protected override void FixedUpdate() 
        {
            base.FixedUpdate();
        }
        protected override void CheckSwitchStates()
        {
            
        }
        protected override void InitSubState()
        {
            
        } 

        #endregion
    }
}
