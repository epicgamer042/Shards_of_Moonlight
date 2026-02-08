using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject pauseMenuUI_Panel;
    [SerializeField] private GameObject EndGameMenuUI_Panel;
    [SerializeField] private GameObject pauseMenuFirstSelected;
    [SerializeField] private GameObject EndGameMenuFirstSelected;
    [SerializeField] private EndGame_UI endGameUI;
    [SerializeField] private CanvasGroup inGameUICanvasGroup;
    [SerializeField] private TextMeshProUGUI timerValue;
    [SerializeField] private TextMeshProUGUI shardCountValue;
    [SerializeField] private TextMeshProUGUI playerHealthValue;
    public TextMeshProUGUI levelTitleText;


    //=====// EVENT METHODS //=====//

    private void Update()
    {
        timerValue.text = gameManager.elapsedTime.ToString("F2") + "s";
        shardCountValue.text = gameManager.getShardCount().ToString() + "/" + gameManager.getShardsToCollect().ToString();
    }

    private void OnEnable()
    {
        PlayerController.OnHealthChanged += UpdateHealthText;
        GameManager.OnShardChanged += UpdateShardText;
    }

    private void OnDisable()
    {
        PlayerController.OnHealthChanged -= UpdateHealthText;
        GameManager.OnShardChanged -= UpdateShardText;
    }

    //====// PAUSE MENU MANAGER //====//

    public void EnablePauseMenuUI()
    {
        gameManager.EnablePauseGame();
        pauseMenuUI_Panel.SetActive(true);
        FirstSelectedManager.SetFirstSelected(pauseMenuFirstSelected);
    }

    public void DisablePauseMenuUI()
    {
        pauseMenuUI_Panel.SetActive(false);
        gameManager.DisablePauseGame();
    }

    //====// END GAME MENU MANAGER //====//

    public void HandleLevelEnd()
    {
        gameManager.EnablePauseGame();
        EndGameMenuUI_Panel.SetActive(true);
        endGameUI.HandleMenuState(gameManager.winLevel, gameManager.winGame, timerValue.text, shardCountValue.text);
        FirstSelectedManager.SetFirstSelected(EndGameMenuFirstSelected);
    }

    public void DisableLevelEndUI()
    {
        EndGameMenuUI_Panel.SetActive(false);
        gameManager.DisablePauseGame();
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

    private void UpdateShardText(int shards)
    {
        shardCountValue.text = shards.ToString() + "/" + gameManager.getShardsToCollect().ToString();
    }
}
