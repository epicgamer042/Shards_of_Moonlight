using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnerZone : MonoBehaviour
{
    [SerializeField] private GameObject enemyPreFab; // Define Prefab to Spawn
    [SerializeField] private Transform spawnPoint; // Define Spawn Point
    [SerializeField] private int minEnemies = 2; // Define Enemies to maintain in zone
    [SerializeField] private float refillDelay = 5f; // Time to wait before refilling after player leaves zone

    private List<GameObject> enemiesInZone = new List<GameObject>();
    private bool playerInZone = false;
    private float timeSincePlayerExit = 0f;

    private void Update()
    {
        // Track time since player left
        if (!playerInZone)
        {
            timeSincePlayerExit += Time.deltaTime;

            // Only refill if player has been gone long enough
            if (timeSincePlayerExit >= refillDelay)
            {
                MaintainEnemyCount();
            }
        }
        else
        {
            // Reset timer while player is inside
            timeSincePlayerExit = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInZone.Add(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInZone.Remove(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }

    private void MaintainEnemyCount()
    {
        // Clean up destroyed enemies
        enemiesInZone.RemoveAll(e => e == null);

        while (enemiesInZone.Count < minEnemies)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemiesInZone.Add(newEnemy);

        }
    }
