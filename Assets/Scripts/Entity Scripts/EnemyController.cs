using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : Entity
{
    private bool playerDetected;

    [Header("Movement Details")]
    [SerializeField] protected float moveSpeed = 3.5f;

    [Header("Collision Details")]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] protected bool wallDetected;

    private float attackCooldown = 1f; // seconds
    private float lastAttackTime;

    public Transform shardParent; 
    public GameObject moonShardPrefab;


    //=====// EVENT METHODS //=====//

    protected override void Awake()
    {
        base.Awake();
        shardParent = GameObject.FindGameObjectWithTag("ShardContainer").transform;
    }

    protected override void Update()
    {
        base.Update();

        TryAttack();
    }


    //=====// ATTACK METHODS //=====//

    private void TryAttack()
    {
        if (playerDetected && Time.time >= lastAttackTime + attackCooldown)
        {
            anim.SetTrigger("attack");
            lastAttackTime = Time.time;
        }
    }


    //=====// MOVEMENT METHODS //=====//

    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
        wallDetected = Physics2D.Raycast(transform.position, transform.right, wallCheckDistance, whatIsGround);
    }

    protected override void HandleMovement()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(facingDir * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    protected override void HandleFlip()
    {
        if (canFlip && wallDetected)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        // Draw line to check for ground layer (wall) for enemy to turn around
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance, 0));
    }

    //=====// ANIMATION METHODS //=====//

    protected override void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        //anim.SetFloat("yVelocity", rb.linearVelocity.y);
        //anim.SetBool("isGrounded", isGrounded);
    }


    //====// ON DEATH METHODS //====//

    protected override void Die()
    {
        SpawnShardsOnDie();
        base.Die();
    }

    private void SpawnShardsOnDie()
    {
        for (int i = 0; i < Random.Range(0, 3); i++)
        {
            GameObject shard = Instantiate(moonShardPrefab, transform.position, Quaternion.identity, shardParent);

            MoonShard shardScript = shard.GetComponent<MoonShard>();
            Rigidbody2D shardRb = shard.GetComponent<Rigidbody2D>();

            shardScript.SetPickupDelay(0.5f);
            float xVel = Random.Range(-2f, 2f);
            float yVel = Random.Range(2f, 5f);
            shardRb.linearVelocity = new Vector2(xVel, yVel);
        }
    }
}
