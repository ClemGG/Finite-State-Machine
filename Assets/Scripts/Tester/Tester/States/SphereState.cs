using Project.StateMachine;
using Project.Tester.Inputs;
using UnityEngine;

namespace Project.Tester
{
    //This state replaces the object's mesh with a sphere, colors it blue
    //and changes its scale over time

    public class SphereState : BaseState<StateMachineTester, TesterInput>
    {
        private float _pingPongValue { get; set; }

        #region Constructor & IPooled


        public SphereState() : base(null, null, null)
        {

        }

        public SphereState(StateMachineTester context, TesterInput input, StateMachine<StateMachineTester, TesterInput> factory) : base(context, input, factory)
        {
            //Change _isRootState here
            _isRootState = true;
        }

        #endregion


        #region State Methods

        protected override void Enter()
        {
            //When the state is entered, we change the mesh to a sphere and we color it in blue.
            _ctx.Filter.mesh = _ctx._sphereMesh;
            _ctx.Renderer.sharedMaterial.SetColor("_Color", _ctx._sphereColor);
            base.Enter();
        }
        protected override void Exit()
        {
            //When the state is exited, we reset all modified values if necessary for the next state.
            _ctx.T.localScale = Vector3.one;

            base.Exit();
        }
        protected override void Update()
        {
            //Sets the value of the new scale between O and 3 over time.
            _pingPongValue = Mathf.PingPong(Time.time * _ctx._scaleSpeed, 3f);

            base.Update();
        }
        protected override void FixedUpdate() 
        {
            //Sets the value of the new scale over time.
            _ctx.T.localScale = Vector3.one * _pingPongValue;

            base.FixedUpdate();
        }
        protected override void CheckSwitchStates()
        {
            //When the spacebar is pressed, we swap to a Cube mesh and a red color.
            if (_input.SwapState)
            {
                SwitchState(_factory.GetState<CubeState>(_ctx, _input));
            }
        }
        protected override void InitSubState()
        {
            //We don't have a sub-state for the SphereState, so we don't do anything here.
        } 

        #endregion
    }
}
