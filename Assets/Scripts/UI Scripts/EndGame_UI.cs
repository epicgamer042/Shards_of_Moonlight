using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalTime;
    [SerializeField] private TextMeshProUGUI finalShardCount;

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
