using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> levelPrefabs; // List of Level Prefabs, assigned in inspector
    [SerializeField] private GameObject playerPrefab; // Player prefab, assigned in inspector
    private GameObject currentPlayer;

    private void LoadLevel(int index)
    {
        // Turn off all levels
        foreach (GameObject level in levelPrefabs)
        {
            level.SetActive(false);
        }

        // Enable the desired level
        levelPrefabs[index].SetActive(true);

        // Find PlayerSpawn inside the active level
        Transform spawnPoint = levelPrefabs[index].transform.Find("PlayerSpawn");

        // Destroy old player if it exists
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        // Spawn new player and store reference
        currentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
