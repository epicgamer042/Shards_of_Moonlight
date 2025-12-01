using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_UI : MonoBehaviour
{
    [SerializeField] private InGame_UI inGameUI;
    [SerializeField] private GameManager gameManager;

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
    
    public void ReturnToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void RestartCurrentScene()
    {
        inGameUI.DisablePauseMenuUI();
        gameManager.RestartLevel();
    }
}
