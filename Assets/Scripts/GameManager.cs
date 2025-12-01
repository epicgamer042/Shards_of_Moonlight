using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    private int shardsToCollect = 1;
    private int shardCount = 0;
    public bool winLevel = false;
    public bool winGame = false;

    public float elapsedTime { get; private set; }


    //=====// EVENT METHODS //=====//

    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        LoadLevel(0);
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

        shardsToCollect = 1 + (index * 10); //set level count to complete
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

    // Rest Level Timer to 0
    public void ResetLevelTimer()
    {
        elapsedTime = 0f;
    }

    public void ResetShardCount()
    {
        shardCount = 0;
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
