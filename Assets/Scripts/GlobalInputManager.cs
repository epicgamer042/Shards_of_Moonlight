using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public enum InputDeviceScheme
{
    KeyboardMouse,
    Gamepad
}


public class GlobalInputManager : MonoBehaviour
{
    public static GlobalInputManager Instance { get; private set; }
    public static PlayerControls Controls { get; private set; }

    public event Action<InputDeviceScheme> OnControlSchemeChanged;

    private InputDeviceScheme lastScheme = InputDeviceScheme.KeyboardMouse;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Controls = new PlayerControls();
        DontDestroyOnLoad(gameObject);

        InputSystem.onEvent += OnInputEvent;

    }

    public void EnableGameplayActionMap() => Controls.Gameplay.Enable();

    public void DisableGameplayActionMap() => Controls.Gameplay.Disable();

    public void EnableUIActionMap() => Controls.UI.Enable();

    public void DisableUIActionMap() => Controls.UI.Disable();

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        // Ignore unwanted traffic
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) 
            return;

        // Listen for device change by input event
        var incomingScheme = GetSchemeFromDevice(device);
        
        // Exit if same scheme
        if (incomingScheme == lastScheme) 
            return;

        // determine what device generated it
        // fire event if control scheme changed
        // debounce stick drift and small mouse movement

        //InputSystem.onEvent fires for every input from every device
        
        if (device is Mouse mouse)
        {
            // Ignore tiny mouse movement (noise), update scheme on large mouse movement
            var delta = mouse.delta.ReadValue();
            if (delta.sqrMagnitude > 0.5f) // tweak threshold
                UpdateScheme(InputDeviceScheme.KeyboardMouse);

            // Update scheme on mouse click
            if (mouse.leftButton.isPressed || mouse.rightButton.isPressed)
                UpdateScheme(InputDeviceScheme.KeyboardMouse);
        }

        if (device is Keyboard keyboard)
        {
            // Update schem on any key press
            if (keyboard.anyKey.isPressed)
            {
                UpdateScheme(InputDeviceScheme.KeyboardMouse);
            }

        }

        if (device is Gamepad gamepad)
        {
            // Ignore stick drift
            Vector2 left = gamepad.leftStick.ReadValue();
            Vector2 right = gamepad.rightStick.ReadValue();

            bool stickMoved = left.sqrMagnitude > 0.2f || right.sqrMagnitude > 0.2f;

            bool buttonPressed = gamepad.allControls.OfType<ButtonControl>().Any(b => b.wasPressedThisFrame); // Using LINQ tool

            if (stickMoved || buttonPressed) 
                UpdateScheme(InputDeviceScheme.Gamepad);
        }

    }

    private InputDeviceScheme GetSchemeFromDevice(InputDevice device)
    {
        if (device is Gamepad) 
            return InputDeviceScheme.Gamepad; 
        
        if (device is Keyboard || device is Mouse) 
            return InputDeviceScheme.KeyboardMouse; 
        
        return lastScheme; // fallback
    }

    private void UpdateScheme(InputDeviceScheme newScheme)
    {
        if (newScheme == lastScheme)
            return;
        lastScheme = newScheme;
        OnControlSchemeChanged?.Invoke(newScheme);


    }

}


