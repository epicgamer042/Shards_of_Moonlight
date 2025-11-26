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

    //=====// EVENT METHODS //=====//

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
        // Draw line to check for ground layer (wall) around enemy ankles
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance, 0));
    }

    //=====// ANIMATION METHODS //=====//

    protected override void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        //anim.SetFloat("yVelocity", rb.linearVelocity.y);
        //anim.SetBool("isGrounded", isGrounded);
    }

}
