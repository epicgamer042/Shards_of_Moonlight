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
    [SerializeField] private TextMeshProUGUI playerHealthValue;


    //=====// EVENT METHODS //=====//

    private void Update()
    {
        timerValue.text = gameManager.elapsedTime.ToString("F2") + "s";
        shardCountValue.text = gameManager.getShardCount().ToString();
    }

    private void OnEnable()
    {
        PlayerController.OnHealthChanged += UpdateHealthText;
    }

    private void OnDisable()
    {
        PlayerController.OnHealthChanged -= UpdateHealthText;
    }

    //====// PAUSE MENU MANAGER //====//

    public void EnablePauseMenuUI()
    {
        Time.timeScale = 0;
        gameManager.DisablePlayerControls();
        DisableInGameUI();
        pauseMenuUI_Panel.SetActive(true);
    }

    public void DisablePauseMenuUI()
    {
        pauseMenuUI_Panel.SetActive(false);
        EnableInGameUI();
        gameManager.EnablePlayerControls();
        Time.timeScale = 1;
    }

    //====// END GAME MENU MANAGER //====//

    public void HandleLevelEnd()
    {
        Time.timeScale = 0;
        gameManager.DisablePlayerControls();
        DisableInGameUI();
        EndGameMenuUI_Panel.SetActive(true);
        endGameUI.HandleMenuState(gameManager.winLevel, gameManager.winGame, timerValue.text, shardCountValue.text);
    }

    public void DisableLevelEndUI()
    {
        EndGameMenuUI_Panel.SetActive(false);
        EnableInGameUI();
        gameManager.EnablePlayerControls();
        Time.timeScale = 1;
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

    private void UpdateHealthText(int health)
    {
        playerHealthValue.text = health.ToString();
    }


}
