using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame_UI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI endGameState;
    [SerializeField] private TextMeshProUGUI finalTime;
    [SerializeField] private TextMeshProUGUI finalShardCount;
    [SerializeField] private TextMeshProUGUI endLevelButtonText;
    [SerializeField] private Button endLevelButton;
    [SerializeField] private InGame_UI inGameUI;


    public void HandleMenuState(bool levelstate, bool gamestate, string leveltime, string shardcount)
    {
        ShowEndLevelState(levelstate);
        ShowFinalTime(leveltime);
        ShowFinalShardCount(shardcount);
        HandleButtonState(levelstate, gamestate);
    }

    public void ShowEndLevelState(bool levelstate)
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

    public void TitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void HandleButtonState(bool levelstate, bool gamestate)
    {
        if (gamestate) //game completed
        {
            //set button text to finish -> function to open credits/game summary
            endLevelButtonText.text = "FINISH GAME";
            SetFinishGameButton();
        }
        else if (levelstate) //level completed but not game completed
        {
            if (GameStartData.StartInTutorial) //Tutorial Completed, return to title screen
            {
                endLevelButtonText.text = "TITLE SCREEN";
                SetTitleScreenButton();
            }
            else //set button function and text to next level
            {
                endLevelButtonText.text = "NEXT LEVEL";
                SetNextLevelButton();
            }
                
        }
        else // level ended unsuccessfully
        {
            //restart level
            endLevelButtonText.text = "RESTART LEVEL";
            SetRestartButton();
        }
    }

    private void SetTitleScreenButton()
    {
        endLevelButton.onClick.RemoveAllListeners();
        endLevelButton.onClick.AddListener(TitleScreen);
    }

    private void SetRestartButton()
    {
        endLevelButton.onClick.RemoveAllListeners();
        endLevelButton.onClick.AddListener(ProcessRestartLevel);
    }

    private void SetNextLevelButton()
    {
        endLevelButton.onClick.RemoveAllListeners();
        endLevelButton.onClick.AddListener(ProcessNextLevel);
    }

    private void SetFinishGameButton()
    {
        endLevelButton.onClick.RemoveAllListeners();
        endLevelButton.onClick.AddListener(EndCredits);
    }

    private void ProcessRestartLevel()
    {
        inGameUI.DisableLevelEndUI();
        gameManager.RestartLevel();
    }

    private void ProcessNextLevel()
    {
        inGameUI.DisableLevelEndUI();
        gameManager.LoadNextLevel();
    }
}
