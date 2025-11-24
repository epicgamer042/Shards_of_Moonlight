using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InGame_UI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI_Panel;
    [SerializeField] private GameObject EndGameMenuUI_Panel;
    [SerializeField] private EndGame_UI endGameUI;
    [SerializeField] private CanvasGroup inGameUICanvasGroup;
    [SerializeField] private TextMeshProUGUI timerValue;
    [SerializeField] private TextMeshProUGUI shardCountValue;

    public static event Action AllShardsCollected;

    private int shardCount = 0;
    public bool allSroudsFound = false;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        timerValue.text = Time.timeSinceLevelLoad.ToString("F2") + "s";
        shardCountValue.text = shardCount.ToString();
    }

    private void OnEnable()
    {
        EndGameZone.OnLevelCompleted += HandleLevelCompleted;
        MoonShard.OnShardToCollect += HandleCollectShard;
    }

    private void OnDisable()
    {
        EndGameZone.OnLevelCompleted -= HandleLevelCompleted;
        MoonShard.OnShardToCollect -= HandleCollectShard;
    }

    //====// PAUSE MENU MANAGER //====//

    public void EnablePauseMenuUI()
    {
        pauseMenuUI_Panel.SetActive(true);
        Time.timeScale = 0;
        DisableInGameUI();
    }

    public void DisablePauseMenuUI()
    {
        pauseMenuUI_Panel.SetActive(false);
        Time.timeScale = 1;
        EanableInGameUI();
    }

    //====// END GAME MENU MANAGER //====//

    public void EnableEndGameMenuUI()
    {
        EndGameMenuUI_Panel.SetActive(true);
        Time.timeScale = 0;
        DisableInGameUI();
    }

    public void DisableEndGameMenuUI()
    {
        EndGameMenuUI_Panel.SetActive(false);
        Time.timeScale = 1;
        EanableInGameUI();
    }

    private void HandleLevelCompleted()
    {
        EnableEndGameMenuUI();
        endGameUI.ShowFinalTime(timerValue.text);
        endGameUI.ShowFinalShardCount(shardCountValue.text);
    }


    //====// IN GAME UI MANAGER //====//

    public void EanableInGameUI()
    {
        inGameUICanvasGroup.interactable = true;
        inGameUICanvasGroup.blocksRaycasts = true;
    }

    public void DisableInGameUI()
    {
        inGameUICanvasGroup.interactable = false;
        inGameUICanvasGroup.blocksRaycasts = false;
    }

    //=====// MOON SHARD COUNTING //=====//

    public void HandleCollectShard()
    {
        shardCount++;

        if (shardCount == 7)
        {
            AllShardsCollected?.Invoke();
        }
    }

}
