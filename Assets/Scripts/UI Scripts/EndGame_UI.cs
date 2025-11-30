using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endGameState;
    [SerializeField] private TextMeshProUGUI finalTime;
    [SerializeField] private TextMeshProUGUI finalShardCount;


    public void ShowEndGameState(bool levelstate)
    {
        if (levelstate)
        {
            endGameState.text = "LEVEL COMPLETED";
            endGameState.color = Color.green;
        }
        else
        {
            endGameState.text = "DEFEATED";
            endGameState.color = Color.red;
        }
        
    }
    public void ShowFinalTime(string leveltime)
    {
        finalTime.text = leveltime;
    }
    public void ShowFinalShardCount(string levelcount)
    {
        finalShardCount.text = levelcount;
    }

    public void EndCredits()
    {
        SceneManager.LoadScene("EndCredits");
    }
}
