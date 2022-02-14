// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Keyboard"",
            ""id"": ""30b5275d-a05f-4b2c-8178-824c01a3afc7"",
            ""actions"": [
                {
                    ""name"": ""Horizontal"",
                    ""type"": ""Button"",
                    ""id"": ""3ad0905f-d533-4a00-abba-82ff655ae724"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Vertical"",
                    ""type"": ""Button"",
                    ""id"": ""712ec8ba-2f3b-4512-a5a1-650f4aec53ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""113db9a9-6f49-4d72-8f9a-4c024236c998"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftMouseIsDown"",
                    ""type"": ""Button"",
                    ""id"": ""09630958-c2d8-4a1c-be8b-838e1a06ca57"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Value"",
                    ""id"": ""6949a7ed-f39c-4cf8-80a6-824aa2ace667"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""8b53363c-5a98-4858-9cfe-70c0345c5361"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""cfd0ca0f-f6fd-4632-bc24-83d60c259f34"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Value"",
                    ""id"": ""57d08320-c367-4a03-93e9-6f774c561a53"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""39cc9a09-1557-41b0-8f9a-225d91649183"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""209ef3b7-12cf-4361-81e8-15559275d293"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""20b754a5-c354-4e4f-aa18-e32271aea828"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""235ab2e8-c3a9-4e62-9bb3-19024546f7b5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftMouseIsDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Keyboard
        m_Keyboard = asset.FindActionMap("Keyboard", throwIfNotFound: true);
        m_Keyboard_Horizontal = m_Keyboard.FindAction("Horizontal", throwIfNotFound: true);
        m_Keyboard_Vertical = m_Keyboard.FindAction("Vertical", throwIfNotFound: true);
        m_Keyboard_Crouch = m_Keyboard.FindAction("Crouch", throwIfNotFound: true);
        m_Keyboard_LeftMouseIsDown = m_Keyboard.FindAction("LeftMouseIsDown", throwIfNotFound: true);
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

    // Keyboard
    private readonly InputActionMap m_Keyboard;
    private IKeyboardActions m_KeyboardActionsCallbackInterface;
    private readonly InputAction m_Keyboard_Horizontal;
    private readonly InputAction m_Keyboard_Vertical;
    private readonly InputAction m_Keyboard_Crouch;
    private readonly InputAction m_Keyboard_LeftMouseIsDown;
    public struct KeyboardActions
    {
        private @PlayerInput m_Wrapper;
        public KeyboardActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Horizontal => m_Wrapper.m_Keyboard_Horizontal;
        public InputAction @Vertical => m_Wrapper.m_Keyboard_Vertical;
        public InputAction @Crouch => m_Wrapper.m_Keyboard_Crouch;
        public InputAction @LeftMouseIsDown => m_Wrapper.m_Keyboard_LeftMouseIsDown;
        public InputActionMap Get() { return m_Wrapper.m_Keyboard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyboardActions set) { return set.Get(); }
        public void SetCallbacks(IKeyboardActions instance)
        {
            if (m_Wrapper.m_KeyboardActionsCallbackInterface != null)
            {
                @Horizontal.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnHorizontal;
                @Horizontal.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnHorizontal;
                @Horizontal.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnHorizontal;
                @Vertical.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnVertical;
                @Vertical.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnVertical;
                @Vertical.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnVertical;
                @Crouch.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnCrouch;
                @LeftMouseIsDown.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnLeftMouseIsDown;
                @LeftMouseIsDown.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnLeftMouseIsDown;
                @LeftMouseIsDown.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnLeftMouseIsDown;
            }
            m_Wrapper.m_KeyboardActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Horizontal.started += instance.OnHorizontal;
                @Horizontal.performed += instance.OnHorizontal;
                @Horizontal.canceled += instance.OnHorizontal;
                @Vertical.started += instance.OnVertical;
                @Vertical.performed += instance.OnVertical;
                @Vertical.canceled += instance.OnVertical;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @LeftMouseIsDown.started += instance.OnLeftMouseIsDown;
                @LeftMouseIsDown.performed += instance.OnLeftMouseIsDown;
                @LeftMouseIsDown.canceled += instance.OnLeftMouseIsDown;
            }
        }
    }
    public KeyboardActions @Keyboard => new KeyboardActions(this);
    public interface IKeyboardActions
    {
        void OnHorizontal(InputAction.CallbackContext context);
        void OnVertical(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnLeftMouseIsDown(InputAction.CallbackContext context);
    }
}
