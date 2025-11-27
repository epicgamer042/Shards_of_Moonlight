using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI_Panel;
    [SerializeField] private GameObject EndGameMenuUI_Panel;
    [SerializeField] private EndGame_UI endGameUI;
    [SerializeField] private CanvasGroup inGameUICanvasGroup;
    [SerializeField] private TextMeshProUGUI timerValue;
    [SerializeField] private TextMeshProUGUI shardCountValue;
    public PlayerController playerController;

    public static event Action AllShardsCollected;

    private int shardCount = 0;
    public bool allSroudsFound = false;
    private bool winLevel = false;

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
        EndGameZone.OnLevelCompleted += HandleLevelComplete;
        MoonShard.OnShardToCollect += HandleCollectShard;
        PlayerController.OnPlayerDie += HandlePlayerDie;
    }

    private void OnDisable()
    {
        EndGameZone.OnLevelCompleted -= HandleLevelComplete;
        MoonShard.OnShardToCollect -= HandleCollectShard;
        PlayerController.OnPlayerDie -= HandlePlayerDie;
    }

    //====// PAUSE MENU MANAGER //====//

    public void EnablePauseMenuUI()
    {
        Time.timeScale = 0;
        playerController.DisablePlayerInput();
        DisableInGameUI();
        pauseMenuUI_Panel.SetActive(true);
    }

    public void DisablePauseMenuUI()
    {
        pauseMenuUI_Panel.SetActive(false);
        EnableInGameUI();
        playerController.EnablePlayerInput();
        Time.timeScale = 1;
    }

    //====// END GAME MENU MANAGER //====//

    private void HandleLevelEnd()
    {
        Time.timeScale = 0;
        playerController.DisablePlayerInput();
        DisableInGameUI();
        EndGameMenuUI_Panel.SetActive(true);
        endGameUI.ShowFinalTime(timerValue.text);
        endGameUI.ShowFinalShardCount(shardCountValue.text);
        endGameUI.ShowEndGameState(winLevel);
    }

    private void HandlePlayerDie()
    {
        winLevel = false;
        HandleLevelEnd();
    }

    private void HandleLevelComplete()
    {
        winLevel = true;
        HandleLevelEnd();
    }

    //====// IN GAME UI MANAGER //====//

    public void EnableInGameUI()
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
