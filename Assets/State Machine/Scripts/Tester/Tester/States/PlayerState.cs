using Project.StateMachine;
using Project.Tester.Inputs;
using UnityEngine;

namespace Project.Tester
{
    public class PlayerState : BaseState<StateMachineTester, TesterInput>
    {

        #region Accessors

        protected Transform T { get { return _t ? _t : _t = Ctx.transform; } set { _t = value; } }
        protected MeshFilter Filter { get { return _filter ? _filter : _filter = Ctx.GetComponent<MeshFilter>(); } set { _filter = value; } }
        protected MeshRenderer Renderer { get { return _renderer ? _renderer : _renderer = Ctx.GetComponent<MeshRenderer>(); } set { _renderer = value; } }
        
        private Transform _t;
        private MeshFilter _filter;
        private MeshRenderer _renderer;

        #endregion

    }
}