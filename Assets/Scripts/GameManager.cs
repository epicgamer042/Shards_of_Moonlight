using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //=====// LEVEL STATE MANAGEMENT //=====//

    [SerializeField] private List<GameObject> levelPrefabs; // List of Level Prefabs, assigned in inspector
    [SerializeField] private GameObject playerPrefab; // Player prefab, assigned in inspector
    [SerializeField] private InGame_UI inGameUI;

    private GameObject currentPlayer;
    private PlayerController playerController;
    private int currentLevelIndex = 0;
    private GameObject currentLevel;

    //=====// GAME DATA MANAGEMENT //=====//

    public static event Action AllShardsCollected;
    public static event Action<int> OnShardChanged;

    private int shardsToCollect = 1;
    private int shardCount = 0;
    public bool winLevel = false;
    public bool winGame = false;

    public float elapsedTime { get; private set; }

    public PlayerScoreData scoreData;

    public List<string> levelTitles = new List<string>();

    //=====// EVENT METHODS //=====//

    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        PopulateLevelTitles();
        StartLevel();
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

    private void Update()
    {
        elapsedTime += Time.deltaTime;
    }


    //=====// LEVEL STATE METHODS //=====//

    private void StartLevel()
    {
        if (GameStartData.StartInTutorial)
        {
            LoadLevel(0);
        }
        else
        {
            LoadLevel(1);
        }
    }

    private void LoadLevel(int index)
    {
        currentLevelIndex = index;

        // Destroy old level if it exists
        if (currentLevel != null)
        {
            Destroy(currentLevel);
        }

        // Instantiate new level
        currentLevel = Instantiate(levelPrefabs[index]);

        // Find PlayerSpawn inside the active level
        Transform spawnPoint = currentLevel.transform.Find("PlayerSpawn");

        // Destroy old player if it exists
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        // Spawn new player and store reference
        currentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        playerController = currentPlayer.GetComponent<PlayerController>();

        // set health, time, and shard count to starting valuess
        ResetLevelTimer();
        ResetShardCount();
        winLevel = false;
        winGame = false;

        shardsToCollect = 7 + (index * 3); //set level count to complete
        
        inGameUI.levelTitleText.text = levelTitles[index];
    }

    public void LoadNextLevel()
    {
        int nextIndex = currentLevelIndex + 1;
        LoadLevel(nextIndex);      
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
    }

    public void PopulateLevelTitles()
    {
        levelTitles = new List<string>()
        {
            "Tutorial",
            "LEVEL 1: NEW MOON",
            "LEVEL 2: WAXING CRESCENT",
            "LEVEL 3: FIRST QUARTER",
            "LEVEL 4: WAXING GIBBOUS",
            "LEVEL 5: FULL MOON"
        };
    }


    //=====// GAME DATA METHODS //=====//

    // Set Game State false on Player Defeated
    private void HandlePlayerDie()
    {
        winLevel = false;
        inGameUI.HandleLevelEnd();
    }

    // Set Game State true on Level Complete
    private void HandleLevelComplete()
    {
        winLevel = true;

        scoreData.levelTimes[currentLevelIndex] = elapsedTime;

        if (currentLevelIndex == 5)
        {
            winGame = true;
        }

        inGameUI.HandleLevelEnd();
    }

    // MOON SHARD COUNTING
    public void HandleCollectShard()
    {
        shardCount++;

        if (shardCount == shardsToCollect)
        {
            AllShardsCollected?.Invoke();
        }
    }

    public int getShardCount()
    {
        return shardCount;
    }

    public int getShardsToCollect()
    {
        return shardsToCollect;
    }

    // Rest Level Timer to 0
    public void ResetLevelTimer()
    {
        elapsedTime = 0f;
    }

    public void ResetShardCount()
    {
        shardCount = 0;
    }

    public void UpdateShardUI()
    {
        OnShardChanged?.Invoke(getShardCount());
    }

    //====// GAME CONTROL //====//

    public void EnablePauseGame()
    {
        Time.timeScale = 0;
        DisablePlayerControls();
        inGameUI.DisableInGameUI();
    }

    public void DisablePauseGame()
    {
        inGameUI.EnableInGameUI();
        EnablePlayerControls();
        Time.timeScale = 1;
    }

    //====// PLAYER CONTROL //====//

    public void EnablePlayerControls()
    {
        playerController.EnablePlayerInput();
    }

    public void DisablePlayerControls()
    {
        playerController.DisablePlayerInput();
    }


}
