using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : Entity
{
    private bool playerDetected;

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

}
