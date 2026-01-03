using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public class UISelectionRestorer : MonoBehaviour
{
    public GameObject firstSelected;
    private GameObject lastSelected;

    public static bool UsingGamepad { get; private set; }
    public static bool UsingMouseKeyboard { get; private set; }


    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
        lastSelected = firstSelected;
    }

    private void Update()
    {
        var current = EventSystem.current.currentSelectedGameObject;

        // If nothing is selected, restore the last known selection
        if (current == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
        else
        {
            lastSelected = current;
        }
        Debug.Log(UsingGamepad);
    }

    private void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (device is Gamepad)
        {
            UsingGamepad = true;
            UsingMouseKeyboard = false;
        }
        else if (device is Keyboard || device is Mouse)
        {
            UsingMouseKeyboard = true;
            UsingGamepad = false;
        }
    }


}
