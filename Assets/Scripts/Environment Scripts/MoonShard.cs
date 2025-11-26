using UnityEngine;
using System;

public class MoonShard : MonoBehaviour
{
    public static event Action OnShardToCollect; // Define Event

    private Rigidbody2D rb; // Identify rigidbody
    private CircleCollider2D triggerCollider; // Identify collider for pickup trigger
    private PolygonCollider2D physicsCollider; // Identify collider for physics

    private bool isMagnetizing = false; // Initialize State as False
    private Transform player; // Define the player transform

    private float pickupDelay = 0.0f;

    private void Awake()
    {
        // Get References at creation
        rb = GetComponent<Rigidbody2D>();
        triggerCollider = GetComponent<CircleCollider2D>();
        physicsCollider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        triggerCollider.enabled = false;
        Invoke(nameof(EnableTriggerCollider), pickupDelay);
    }

    private void Update()
    {
        if (isMagnetizing && player != null)
        {
            // Move toward player
            transform.position = Vector2.MoveTowards(transform.position, player.position, 10f * Time.deltaTime);

            // If close enough, incriment counter (in player), and destroy gameobject
            if (Vector2.Distance(transform.position, player.position) < 0.1f)
            {
                CollectShard();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Define trigger event
    {
        if (collision.CompareTag("Player")) // Checks that Player is the collider trigger
        {
            player = collision.transform; // Gets player position from transform
            DisablePhysics(); // Turns off physics of game object
            isMagnetizing = true; // Enable action to move gameobject to player
            OnShardToCollect?.Invoke(); // this fires the event
        }
    }

    private void DisablePhysics()
    {
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // replaces isKinematic
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (physicsCollider != null)
            physicsCollider.enabled = false; // stop physical collisions

        if (triggerCollider != null)
            triggerCollider.enabled = false; // prevent retriggering
    }

    private void CollectShard()
    {
        Destroy(gameObject); // remove shard from scene
    }

    private void EnableTriggerCollider()
    {
        triggerCollider.enabled = true;
    }

    public void SetPickupDelay(float delay)
    {
        pickupDelay = delay;
    }
}
