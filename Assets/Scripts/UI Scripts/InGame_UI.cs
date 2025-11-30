using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject pauseMenuUI_Panel;
    [SerializeField] private GameObject EndGameMenuUI_Panel;
    [SerializeField] private EndGame_UI endGameUI;
    [SerializeField] private CanvasGroup inGameUICanvasGroup;
    [SerializeField] private TextMeshProUGUI timerValue;
    [SerializeField] private TextMeshProUGUI shardCountValue;
    public PlayerController playerController;


    //=====// EVENT METHODS //=====//

    private void Update()
    {
        timerValue.text = gameManager.elapsedTime.ToString("F2") + "s";
        shardCountValue.text = gameManager.getShardCount().ToString();
    }

    private void OnEnable()
    {
        EndGameZone.OnLevelCompleted += HandleLevelComplete;
        PlayerController.OnPlayerDie += HandlePlayerDie;
    }

    private void OnDisable()
    {
        EndGameZone.OnLevelCompleted -= HandleLevelComplete;
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
        endGameUI.ShowEndGameState(gameManager.winLevel);
    }

    private void HandlePlayerDie()
    {
        HandleLevelEnd();
    }

    private void HandleLevelComplete()
    {
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

}
