// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Tester/Inputs/TesterControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Project.Tester.Inputs
{
    public class @TesterControls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @TesterControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""TesterControls"",
    ""maps"": [
        {
            ""name"": ""Tester"",
            ""id"": ""033074cd-c27e-4584-994f-cc6f792cdd21"",
            ""actions"": [
                {
                    ""name"": ""Swap State"",
                    ""type"": ""Button"",
                    ""id"": ""6e5270db-585d-4452-bb12-e55490b40e6f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""62f3f49d-c1c2-4217-825e-67027f6c1af0"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Swap State"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Tester
            m_Tester = asset.FindActionMap("Tester", throwIfNotFound: true);
            m_Tester_SwapState = m_Tester.FindAction("Swap State", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Tester
        private readonly InputActionMap m_Tester;
        private ITesterActions m_TesterActionsCallbackInterface;
        private readonly InputAction m_Tester_SwapState;
        public struct TesterActions
        {
            private @TesterControls m_Wrapper;
            public TesterActions(@TesterControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @SwapState => m_Wrapper.m_Tester_SwapState;
            public InputActionMap Get() { return m_Wrapper.m_Tester; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(TesterActions set) { return set.Get(); }
            public void SetCallbacks(ITesterActions instance)
            {
                if (m_Wrapper.m_TesterActionsCallbackInterface != null)
                {
                    @SwapState.started -= m_Wrapper.m_TesterActionsCallbackInterface.OnSwapState;
                    @SwapState.performed -= m_Wrapper.m_TesterActionsCallbackInterface.OnSwapState;
                    @SwapState.canceled -= m_Wrapper.m_TesterActionsCallbackInterface.OnSwapState;
                }
                m_Wrapper.m_TesterActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @SwapState.started += instance.OnSwapState;
                    @SwapState.performed += instance.OnSwapState;
                    @SwapState.canceled += instance.OnSwapState;
                }
            }
        }
        public TesterActions @Tester => new TesterActions(this);
        private int m_KeyboardSchemeIndex = -1;
        public InputControlScheme KeyboardScheme
        {
            get
            {
                if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
                return asset.controlSchemes[m_KeyboardSchemeIndex];
            }
        }
        public interface ITesterActions
        {
            void OnSwapState(InputAction.CallbackContext context);
        }
    }
}
