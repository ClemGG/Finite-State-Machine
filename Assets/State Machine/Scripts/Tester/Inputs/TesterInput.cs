
namespace Project.Tester.Inputs
{
    public class TesterInput
    {
        #region Fields

        private TesterControls _controls { get; set; } = new TesterControls();

        public bool SwapState { get; set; }

        #endregion


        #region Methods

        public void Enable()
        {
            _controls.Enable();
        }
        public void Disable()
        {
            _controls.Disable();
        }


        public void Update()
        {
            SwapState = _controls.Tester.SwapState.triggered;
        }

        #endregion
    }
}