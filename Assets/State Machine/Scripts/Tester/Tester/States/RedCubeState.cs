
namespace Project.Tester
{
    public class RedCubeState : PlayerState
    {
        #region Constructor & IPooled

        public RedCubeState()
        {
            //Change _isRootState here
        }

        #endregion


        #region State Methods

        protected override void Enter()
        {
            //When the state is entered, we change the material's color to red.
            Renderer.material.SetColor("_Color", _ctx._cubeColor);
        }

        #endregion
    }
}
