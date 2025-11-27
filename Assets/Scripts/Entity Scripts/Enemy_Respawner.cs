using UnityEngine;

public class Enemy_Respawner : MonoBehaviour
{
    // Prefab to spawn
    [SerializeField] private GameObject enemyPreFab;
    
    // List of respawn points
    [SerializeField] private Transform[] respawnPoints;
    
    // Spawn cooldown timer
    [SerializeField] private float cooldown = 2f;
    [Space]
    [SerializeField] private float cooldownDecreaseRate = 0.05f;
    [SerializeField] private float cooldownCap = 0.7f;
    private float timer;
    
    // Define player transform to point to for direction check
    private Transform player;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = cooldown;
            CreateNewEnemy();

            cooldown = Mathf.Max(cooldownCap, cooldown - cooldownDecreaseRate);
        }
    }

    private void CreateNewEnemy()
    {
        int respawnPointIndex = Random.Range(0, respawnPoints.Length);
        Vector3 spawnPoint = respawnPoints[respawnPointIndex].position;

        GameObject newEnemy = Instantiate(enemyPreFab, spawnPoint, Quaternion.identity);

        bool createdOnTheRight = newEnemy.transform.position.x > player.transform.position.x;

        if (createdOnTheRight)
        {
            newEnemy.GetComponent<EnemyController>().Flip();
        }
    }
}
