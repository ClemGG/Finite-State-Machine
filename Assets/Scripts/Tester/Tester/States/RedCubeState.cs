using Project.StateMachine;
using Project.Tester.Inputs;

namespace Project.Tester
{
    public class RedCubeState : BaseState<StateMachineTester, TesterInput>
    {
        #region Constructor & IPooled

        public RedCubeState() : base(null, null, null)
        {
            //Change _isRootState here
        }

        #endregion


        #region State Methods

        protected override void Enter()
        {
            //When the state is entered, we change the material's color to red.
            _ctx.Renderer.material.SetColor("_Color", _ctx._cubeColor);
        }

        #endregion
    }
}
