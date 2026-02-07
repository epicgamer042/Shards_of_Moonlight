using UnityEngine;

public class GlobalInputManager : MonoBehaviour
{
    public static PlayerControls Controls;

    private void Awake()
    {
        if (Controls == null)
        {
            Controls = new PlayerControls();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}


