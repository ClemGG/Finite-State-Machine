using UnityEngine;
using Project.StateMachine;
using Project.Tester.Inputs;
using Project.Pool;

namespace Project.Tester
{
    //In this example, we want to instantiate a mesh defined in the sub-states
    //and swap between meshes when the spacebar is pressed.
    //Press the spacebar to swap between states.

    public class StateMachineTester : MonoBehaviour
    {
        #region State Machine Fields

        private TesterInput _input { get; set; }
        private StateMachine<StateMachineTester, TesterInput> _factory { get; set; }

        #endregion

        #region Public Fields

        [field:SerializeField]
        public Mesh _cubeMesh { get; set; }
        [field:SerializeField]
        public Mesh _sphereMesh { get; set; }
        [field:SerializeField]
        public Color _cubeColor { get; set; } = Color.red;
        [field:SerializeField]
        public Color _sphereColor { get; set; } = Color.blue;
        [field:SerializeField]
        public float _spinSpeed { get; set; } = 90f;
        [field:SerializeField]
        public float _scaleSpeed { get; set; } = 2f;

        #endregion


        #region Mono

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void Awake()
        {
            _input = new TesterInput();

            //The 'tags' paramters are set dynamically so that we don't have to go back to this script
            //each time we want to change the name of a state.
            _factory = new StateMachine<StateMachineTester, TesterInput>
                (
                    this,
                    _input,
                    //You can also directly use the chld states as generic parameters, since IPool is covariant
                    new Pool<PlayerState>(nameof(SphereState), 1, () => new SphereState()),
                    new Pool<PlayerState>(nameof(CubeState), 1, () => new CubeState()),
                    new Pool<PlayerState>(nameof(RedCubeState), 1, () => new RedCubeState())
                );

            _factory.Start<SphereState>();
        }

        // Update is called once per frame
        void Update()
        {
            _input.Update();
            _factory.Update();
        }

        private void FixedUpdate()
        {
            _factory.FixedUpdate();
        }

        #endregion
    }
}