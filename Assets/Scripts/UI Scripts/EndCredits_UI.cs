using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCredits_UI : MonoBehaviour
{

    public void ExitGame()
    {
        //Exit in build application
        Application.Quit();

        //Exit in Unity Editor (UNITY_EDITOR is not defined in a built game)
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void ReturnToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
