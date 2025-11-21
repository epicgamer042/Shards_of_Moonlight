using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalTime;

    public void ShowFinalTime(string leveltime)
    {
        finalTime.text = leveltime;
    }

    public void EndCredits()
    {
        SceneManager.LoadScene("EndCredits");
    }
}
