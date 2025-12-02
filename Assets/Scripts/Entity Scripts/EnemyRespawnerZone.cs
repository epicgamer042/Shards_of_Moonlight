using System.Collections;
using UnityEngine;

//=======================================================//
// Current configuration of spawning enemies expects a   //
// fixed "room" with walls on both sides with the enemy  //
// collider set to wall to wall. This makes sure there   //
// are always 2 enemies in the dictated collider zone.   //
// It refills only when the player is out of the zone.   //
//=======================================================//

public class EnemyRespawnerZone : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // Define Prefab to Spawn
    [SerializeField] private Transform spawnPoint;   // Define Spawn Point
    [SerializeField] private int minEnemies = 2;     // Define Number of Enemies to maintain in zone
    [SerializeField] private float refillDelay = 3f; // Time to wait before refilling after player leaves zone

    [SerializeField] private int currentEnemyCount = 0;
    private bool playerInZone = false;
    private Coroutine spawnRoutine; // Reference to make sure only 1 coroutine for enemy spawn cycle is running at a time

    private void Awake()
    {
        StartCoroutine(HandleEnemySpawn()); // Populate zone on startup
    }

    private void OnTriggerEnter2D(Collider2D other) // Use to keep track of Enemmies when they spawn, and if player enters before leave timer
    {
        if (other.CompareTag("Enemy"))
        {
            currentEnemyCount++;
        }
        else if (other.CompareTag("Player")) // If Player Enters zone
        {
            playerInZone = true; // Mark as entered
            
            if (spawnRoutine != null) // If coroutine reference exists
            {
                StopCoroutine(spawnRoutine); // End spawn cycle coroutine
                spawnRoutine = null; // Set reference to null
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) // Use to detect when the player leaves the zone
    {
        if (other.CompareTag("Enemy"))
        {
            currentEnemyCount--;
        }
        else if (other.CompareTag("Player")) // When Player exits zone
        {
            playerInZone = false; // Mark has exited
            if (spawnRoutine == null && gameObject.activeInHierarchy) // Confirm spawnroutine is null and object is active
                spawnRoutine = StartCoroutine(HandleEnemySpawn()); // Begin zone spawn cycle
        }
    }

    private IEnumerator HandleEnemySpawn()
    {
        while (currentEnemyCount < minEnemies)
        {
            yield return new WaitForSeconds(refillDelay); // Spawn Delay

            if (currentEnemyCount == minEnemies || playerInZone) // Check that conditions are still true for spawn after wait
            {
                spawnRoutine = null;
                yield break;
            }
            
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation); // Spawn Enemy

            newEnemy.transform.SetParent(transform); // Make sure Enemies are under level prefab, not scene so they get destroyed with level load

            yield return new WaitForSeconds(0.5f); // Delay to process Enemy count in OnTriggerEnter before continuiing loop
        }
        
        // Clear reference when finished
        spawnRoutine = null;
    }

}
