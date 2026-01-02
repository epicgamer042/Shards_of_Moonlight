using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCredits_UI : MonoBehaviour
{
    public PlayerScoreData scoreData;

    [SerializeField] private TextMeshProUGUI level1TimeValue;
    [SerializeField] private TextMeshProUGUI level2TimeValue;
    [SerializeField] private TextMeshProUGUI level3TimeValue;
    [SerializeField] private TextMeshProUGUI level4TimeValue;
    [SerializeField] private TextMeshProUGUI level5TimeValue;
    [SerializeField] private TextMeshProUGUI totalTimeValue;

    private void Start()
    {
        HandleGameTimes();
    }

    public void HandleGameTimes()
    {
        level1TimeValue.text = scoreData.levelTimes[1].ToString("F2") + "s";
        level2TimeValue.text = scoreData.levelTimes[2].ToString("F2") + "s";
        level3TimeValue.text = scoreData.levelTimes[3].ToString("F2") + "s";
        level4TimeValue.text = scoreData.levelTimes[4].ToString("F2") + "s";
        level5TimeValue.text = scoreData.levelTimes[5].ToString("F2") + "s";
        totalTimeValue.text = countTotal();
    }

    public string countTotal()
    {
        float total = 0f;
        for (int i = 1; i <= 5; i++)
        {
            total += scoreData.levelTimes[i];
        }
        return total.ToString("F2") + "s";
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

    public void ReturnToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
