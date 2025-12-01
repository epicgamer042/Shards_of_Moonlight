using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    //=====// LEVEL STATE MANAGEMENT //=====//

    [SerializeField] private List<GameObject> levelPrefabs; // List of Level Prefabs, assigned in inspector
    [SerializeField] private GameObject playerPrefab; // Player prefab, assigned in inspector

    private GameObject currentPlayer;
    private PlayerController playerController;
    private int currentLevelIndex = 0;
    private GameObject currentLevel;

    //=====// GAME DATA MANAGEMENT //=====//

    public static event Action AllShardsCollected;

    private int shardCount = 0;
    public bool allSroudsFound = false;
    public bool winLevel = false;

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
        // Destroy old level if it exists
        if (currentLevel != null)
        {
            Destroy(currentLevel);
        }

        // Instantiate new level
        currentLevel = Instantiate(levelPrefabs[index]);

        // Find PlayerSpawn inside the active level
        Transform spawnPoint = levelPrefabs[index].transform.Find("PlayerSpawn");

        // Destroy old player if it exists
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        // Spawn new player and store reference
        currentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        playerController = currentPlayer.GetComponent<PlayerController>();

        // set health, time, and shard count to starting valuess
    }

    private void LoadNextLevel()
    {
        int nextIndex = currentLevelIndex + 1;
        if (nextIndex < levelPrefabs.Count)
        {
            LoadLevel(nextIndex);
        }
            
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
    }

    public void NextLevel()
    {
        LoadNextLevel();
    }


    //=====// GAME DATA METHODS //=====//

    // Set Game State false on Player Defeated
    private void HandlePlayerDie()
    {
        winLevel = false;
    }

    // Set Game State true on Level Complete
    private void HandleLevelComplete()
    {
        winLevel = true;
    }

    // MOON SHARD COUNTING
    public void HandleCollectShard()
    {
        shardCount++;

        if (shardCount == 7)
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
