using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIInputManager : MonoBehaviour
{
    private enum InputDevice { Mouse, Controller }
    private InputDevice currentDevice;

    private PlayerControls input;
    private GameObject lastSelected;
    private Vector2 lastMousePos;

    private void Awake()
    {
        input = new PlayerControls();
        input.Enable();

        lastSelected = EventSystem.current.firstSelectedGameObject;
        lastMousePos = Mouse.current.position.ReadValue();

        currentDevice = InputDevice.Controller;
    }

    private void Update()
    {
        TrackLastSelected();
        DetectMouseSwitch();
        DetectControllerSwitch();
    }

    private void DetectMouseSwitch()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (mousePos != lastMousePos)
        {
            lastMousePos = mousePos;

            if (currentDevice != InputDevice.Mouse)
            {
                currentDevice = InputDevice.Mouse;

                // IMPORTANT: Do NOT clear selection here.
                // Unity will automatically highlight the hovered button.
            }
        }
    }

    private void DetectControllerSwitch()
    {
        if (input.UI.Navigate.triggered ||
            input.UI.Submit.triggered ||
            input.UI.Cancel.triggered)
        {
            if (currentDevice != InputDevice.Controller)
            {
                currentDevice = InputDevice.Controller;

                // Restore last controller-selected button
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
        }
    }

    private void TrackLastSelected()
    {
        var current = EventSystem.current.currentSelectedGameObject;

        if (current != null &&
            current != lastSelected &&
            current.GetComponent<Selectable>() != null)
        {
            lastSelected = current;
        }
    }
}