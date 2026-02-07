using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class InputBinder_UI : MonoBehaviour
{
    private void Awake()
    {
        var ui = GetComponent<InputSystemUIInputModule>();
        var input = GlobalInputManager.Controls;

        ui.point = new InputActionProperty(input.UI.Point);
        ui.move = new InputActionProperty(input.UI.Navigate);
        ui.submit = new InputActionProperty(input.UI.Submit);
        ui.cancel = new InputActionProperty(input.UI.Cancel);
        ui.leftClick = new InputActionProperty(input.UI.Click);
        ui.rightClick = new InputActionProperty(input.UI.RightClick);
        ui.middleClick = new InputActionProperty(input.UI.MiddleClick);
        ui.scrollWheel = new InputActionProperty(input.UI.ScrollWheel);

    }
}
