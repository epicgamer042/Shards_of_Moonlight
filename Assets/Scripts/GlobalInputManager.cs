using System;
using UnityEngine;
using UnityEngine.InputSystem;
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
        // Listen for device change by input event
        // determine what device generated it
        // fire event if control scheme changed
        // debounce stick drift and small mouse movement

        //InputSystem.onEvent fires for every input from every device

        
        if (device is Mouse)
        {

        }

        if (device is Keyboard)
        {

        }

        if (device is Gamepad)
        {

        }

    }

    private void UpdateScheme(InputDeviceScheme newScheme)
    {
        if (newScheme == lastScheme)
            return;
        lastScheme = newScheme;
        OnControlSchemeChanged?.Invoke(newScheme);


    }

}


