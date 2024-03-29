//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.0
//     from Assets/Scripts/Player/Input/PlayerController.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerController: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerController"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""b07572ee-5afa-473a-a5b7-d243344cdeb0"",
            ""actions"": [
                {
                    ""name"": ""Position"",
                    ""type"": ""PassThrough"",
                    ""id"": ""350a9a9b-ed56-48bc-849e-31257f89987c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RMB"",
                    ""type"": ""Value"",
                    ""id"": ""71e1e226-be46-4ec4-ae06-92b91e6ac691"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""LMB"",
                    ""type"": ""Button"",
                    ""id"": ""57d71f85-d461-4779-8003-9beb04680533"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""2267271f-b41a-4eaa-a4b0-ef27c29d3116"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Esc"",
                    ""type"": ""Button"",
                    ""id"": ""638e2aab-058a-4691-9391-bbc8879ffe37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""cf1c9145-eca7-425b-8609-119deefbb36b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Stop"",
                    ""type"": ""Button"",
                    ""id"": ""bc3e897f-a790-49ee-982b-73ededc3db21"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Tab"",
                    ""type"": ""Button"",
                    ""id"": ""4e73c70a-53d7-4306-9f52-684ed3195a34"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""6728faac-872b-467f-8dd0-ace82f8b818c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ExpTree"",
                    ""type"": ""Button"",
                    ""id"": ""158e17b8-9da0-4e98-95d0-679aa8fce4fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Q"",
                    ""type"": ""Button"",
                    ""id"": ""452674f3-cb36-420e-ae3c-4fb37b7e97f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""W"",
                    ""type"": ""Button"",
                    ""id"": ""450fa4bd-cb25-442b-acd3-3f91da361dc9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""E"",
                    ""type"": ""Button"",
                    ""id"": ""1b1a95ad-37c9-4a0d-8a76-ce3cadebaf5d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""83ba93e2-b2e9-4ad6-abe1-aded0c56d958"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold,Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LMB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87d88311-654c-4e7e-a933-474e167fc559"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Hold,Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RMB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c49ff0e8-d3d7-4f7c-aacb-b269c2f6455a"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d7842940-856a-4281-8405-2ee9b83c26ce"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Esc"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91f85b94-44d4-4ebc-a4a2-a3be8d997b2f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6aeaa972-158a-4557-b7b1-c04fe8c05457"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Stop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0c5241ff-6720-4913-8e81-cffedaf4df56"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b5e2aa8-5fc7-418c-a4a4-be52f4978886"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fbc0908-4042-43a0-9dc4-d1ed48b07510"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""482a2d0c-bd68-4517-9217-573c73286645"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ExpTree"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""22696805-d128-4555-ba18-d02247bc4ba0"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Q"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a9fa30c-4666-4cc4-8b10-6389e0234e31"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""W"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""635933b4-30b9-4340-b004-d767783f2ea8"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""E"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Position = m_Player.FindAction("Position", throwIfNotFound: true);
        m_Player_RMB = m_Player.FindAction("RMB", throwIfNotFound: true);
        m_Player_LMB = m_Player.FindAction("LMB", throwIfNotFound: true);
        m_Player_Inventory = m_Player.FindAction("Inventory", throwIfNotFound: true);
        m_Player_Esc = m_Player.FindAction("Esc", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_Stop = m_Player.FindAction("Stop", throwIfNotFound: true);
        m_Player_Tab = m_Player.FindAction("Tab", throwIfNotFound: true);
        m_Player_Enter = m_Player.FindAction("Enter", throwIfNotFound: true);
        m_Player_ExpTree = m_Player.FindAction("ExpTree", throwIfNotFound: true);
        m_Player_Q = m_Player.FindAction("Q", throwIfNotFound: true);
        m_Player_W = m_Player.FindAction("W", throwIfNotFound: true);
        m_Player_E = m_Player.FindAction("E", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Position;
    private readonly InputAction m_Player_RMB;
    private readonly InputAction m_Player_LMB;
    private readonly InputAction m_Player_Inventory;
    private readonly InputAction m_Player_Esc;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_Stop;
    private readonly InputAction m_Player_Tab;
    private readonly InputAction m_Player_Enter;
    private readonly InputAction m_Player_ExpTree;
    private readonly InputAction m_Player_Q;
    private readonly InputAction m_Player_W;
    private readonly InputAction m_Player_E;
    public struct PlayerActions
    {
        private @PlayerController m_Wrapper;
        public PlayerActions(@PlayerController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Position => m_Wrapper.m_Player_Position;
        public InputAction @RMB => m_Wrapper.m_Player_RMB;
        public InputAction @LMB => m_Wrapper.m_Player_LMB;
        public InputAction @Inventory => m_Wrapper.m_Player_Inventory;
        public InputAction @Esc => m_Wrapper.m_Player_Esc;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @Stop => m_Wrapper.m_Player_Stop;
        public InputAction @Tab => m_Wrapper.m_Player_Tab;
        public InputAction @Enter => m_Wrapper.m_Player_Enter;
        public InputAction @ExpTree => m_Wrapper.m_Player_ExpTree;
        public InputAction @Q => m_Wrapper.m_Player_Q;
        public InputAction @W => m_Wrapper.m_Player_W;
        public InputAction @E => m_Wrapper.m_Player_E;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Position.started += instance.OnPosition;
            @Position.performed += instance.OnPosition;
            @Position.canceled += instance.OnPosition;
            @RMB.started += instance.OnRMB;
            @RMB.performed += instance.OnRMB;
            @RMB.canceled += instance.OnRMB;
            @LMB.started += instance.OnLMB;
            @LMB.performed += instance.OnLMB;
            @LMB.canceled += instance.OnLMB;
            @Inventory.started += instance.OnInventory;
            @Inventory.performed += instance.OnInventory;
            @Inventory.canceled += instance.OnInventory;
            @Esc.started += instance.OnEsc;
            @Esc.performed += instance.OnEsc;
            @Esc.canceled += instance.OnEsc;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Stop.started += instance.OnStop;
            @Stop.performed += instance.OnStop;
            @Stop.canceled += instance.OnStop;
            @Tab.started += instance.OnTab;
            @Tab.performed += instance.OnTab;
            @Tab.canceled += instance.OnTab;
            @Enter.started += instance.OnEnter;
            @Enter.performed += instance.OnEnter;
            @Enter.canceled += instance.OnEnter;
            @ExpTree.started += instance.OnExpTree;
            @ExpTree.performed += instance.OnExpTree;
            @ExpTree.canceled += instance.OnExpTree;
            @Q.started += instance.OnQ;
            @Q.performed += instance.OnQ;
            @Q.canceled += instance.OnQ;
            @W.started += instance.OnW;
            @W.performed += instance.OnW;
            @W.canceled += instance.OnW;
            @E.started += instance.OnE;
            @E.performed += instance.OnE;
            @E.canceled += instance.OnE;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Position.started -= instance.OnPosition;
            @Position.performed -= instance.OnPosition;
            @Position.canceled -= instance.OnPosition;
            @RMB.started -= instance.OnRMB;
            @RMB.performed -= instance.OnRMB;
            @RMB.canceled -= instance.OnRMB;
            @LMB.started -= instance.OnLMB;
            @LMB.performed -= instance.OnLMB;
            @LMB.canceled -= instance.OnLMB;
            @Inventory.started -= instance.OnInventory;
            @Inventory.performed -= instance.OnInventory;
            @Inventory.canceled -= instance.OnInventory;
            @Esc.started -= instance.OnEsc;
            @Esc.performed -= instance.OnEsc;
            @Esc.canceled -= instance.OnEsc;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Stop.started -= instance.OnStop;
            @Stop.performed -= instance.OnStop;
            @Stop.canceled -= instance.OnStop;
            @Tab.started -= instance.OnTab;
            @Tab.performed -= instance.OnTab;
            @Tab.canceled -= instance.OnTab;
            @Enter.started -= instance.OnEnter;
            @Enter.performed -= instance.OnEnter;
            @Enter.canceled -= instance.OnEnter;
            @ExpTree.started -= instance.OnExpTree;
            @ExpTree.performed -= instance.OnExpTree;
            @ExpTree.canceled -= instance.OnExpTree;
            @Q.started -= instance.OnQ;
            @Q.performed -= instance.OnQ;
            @Q.canceled -= instance.OnQ;
            @W.started -= instance.OnW;
            @W.performed -= instance.OnW;
            @W.canceled -= instance.OnW;
            @E.started -= instance.OnE;
            @E.performed -= instance.OnE;
            @E.canceled -= instance.OnE;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnPosition(InputAction.CallbackContext context);
        void OnRMB(InputAction.CallbackContext context);
        void OnLMB(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnEsc(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnStop(InputAction.CallbackContext context);
        void OnTab(InputAction.CallbackContext context);
        void OnEnter(InputAction.CallbackContext context);
        void OnExpTree(InputAction.CallbackContext context);
        void OnQ(InputAction.CallbackContext context);
        void OnW(InputAction.CallbackContext context);
        void OnE(InputAction.CallbackContext context);
    }
}
