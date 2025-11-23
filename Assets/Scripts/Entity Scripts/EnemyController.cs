using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : Entity
{
    private bool playerDetected;

    [Header("Movement Details")]
    [SerializeField] protected float moveSpeed = 3.5f;

    private float attackCooldown = 1f; // seconds
    private float lastAttackTime;

    //=====// EVENT METHODS //=====//

    protected override void Update()
    {
        base.Update();

        TryAttack();
    }

    //=====// ATTACK METHOD //=====//

    private void TryAttack()
    {
        if (playerDetected && Time.time >= lastAttackTime + attackCooldown)
        {
            DamageTargets();
            lastAttackTime = Time.time;
        }
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
    }

    protected override void HandleMovement()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(facingDir * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    protected override void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        //anim.SetFloat("yVelocity", rb.linearVelocity.y);
        //anim.SetBool("isGrounded", isGrounded);
    }

}
