using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void EndCredits()
    {
        SceneManager.LoadScene("EndCredits");
    }

    public void ExitGame()
    {
        //Exit in build application
        Application.Quit();

        //Exit in Unity Editor (UNITY_EDITOR is not defined in a built game)
        # if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
