using UnityEngine;
using UnityEngine.EventSystems;

public static class FirstSelectedManager
{
    public static void SetFirstSelected(GameObject button)
    {
        if (button == null)
            return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }
}
