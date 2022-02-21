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


        public SphereState()
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
            _ctx.Renderer.material.SetColor("_Color", _ctx._sphereColor);
        }
        protected override void Exit()
        {
            //When the state is exited, we reset all modified values if necessary for the next state.
            _ctx.T.localScale = Vector3.one;
        }
        protected override void Update()
        {
            //Sets the value of the new scale between O and 3 over time.
            _pingPongValue = Mathf.PingPong(Time.time * _ctx._scaleSpeed, 3f);
        }
        protected override void FixedUpdate() 
        {
            //Sets the value of the new scale over time.
            _ctx.T.localScale = Vector3.one * _pingPongValue;
        }
        protected override void CheckSwitchStates()
        {
            //When the spacebar is pressed, we swap to a Cube mesh and a red color.
            if (_input.SwapState)
            {
                SwitchState<CubeState>();
            }
        }
        protected override void InitSubState()
        {
            //We don't have a sub-state for the SphereState, so we don't do anything here.
        } 

        #endregion
    }
}