using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    private PlayerControls input;

    private void Awake()
    {
        input = GlobalInputManager.Controls;
    }

    private void OnEnable()
    {
        input.UI.Enable();
        input.Gameplay.Disable();
    }

    private void OnDisable()
    {
        input.UI.Disable();
        input.Gameplay.Disable();
    }

    public void StartTutorial()
    {
        GameStartData.StartInTutorial = true;
        SceneManager.LoadScene("Game");
    }
    
    public void StartGame()
    {
        GameStartData.StartInTutorial = false;
        SceneManager.LoadScene("Game");
    }

    public void EndCredits()
    {
        SceneManager.LoadScene("EndCredits");
    }

    public void ExitGame()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            Application.ExternalCall("location.reload"); // Refresh web build on exit (deprecated but should still work)
        #elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //Exit from Unity Editor
        #else
            Application.Quit(); // Exit from standalone
        #endif
    }
}
