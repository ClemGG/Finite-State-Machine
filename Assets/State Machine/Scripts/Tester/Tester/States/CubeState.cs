using UnityEngine;

namespace Project.Tester
{
    //This state replaces the object's mesh with a cube
    //and spins it over time.
    //This is a root state, which means RedCubeState will be its sub-state
    //in charge of coloring the mesh in red.


    public class CubeState : PlayerState
    {
        #region Constructor & IPooled

        public CubeState()
        {
            //Change _isRootState here
            _isRootState = true;
        }

        #endregion


        #region State Methods

        protected override void Enter()
        {
            //When the state is entered, we change the mesh to a cube.
            //We change the color in the sub-state for demonstration purposes.
            Filter.mesh = _ctx._cubeMesh;
        }
        protected override void Exit()
        {
            //When the state is exited, we reset all modified values if necessary for the next state.
            T.rotation = Quaternion.identity;
        }
        protected override void FixedUpdate() 
        {
            //Spins the cube on the Y axis
            T.Rotate(Vector3.up, _ctx._spinSpeed * Time.fixedDeltaTime);
        }
        protected override void CheckSwitchStates()
        {
            //When the spacebar is pressed, we swap to a Sphere mesh and a blue color.
            if (_input.SwapState)
            {
                SwitchState<SphereState>();
            }
        }
        protected override void InitSubState()
        {
            //When we enter this RootState, we immediately assign the RedCube SubState to it
            //so that we can color the object in red.
            SetSubState<RedCubeState>();
        } 

        #endregion
    }
}
