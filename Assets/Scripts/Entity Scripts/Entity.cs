using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Entity : MonoBehaviour
{

    //=====// DEFINITIONS //=====//

    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected SpriteRenderer sr;

    [Header("Health")]
    [SerializeField] private int maxHealth = 7;
    [SerializeField] private int currentHealth;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration = 0.1f;
    private Coroutine damageFeedbackCoroutine;

    [Header("Attack details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;

    [Header("Collision Details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] protected bool isGrounded;

    // Facing Direction Details
    protected int facingDir = 1;
    protected bool canMove = true;
    protected bool facingRight = true;


    //=====// EVENT METHODS //=====//

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        HandleCollision();
        HandleAnimations();
        HandleFlip();
    }


    //=====// HEALTH & DAMAGE METHODS //=====//

    public int GetCurrentHealth => currentHealth; //For (player) child access

    protected virtual void TryAttack(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            anim.SetTrigger("attack");
            Debug.Log("TEST ATTACK");
        }
    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);

        foreach (Collider2D enemy in enemyColliders)
        {
            Entity entityTarget = enemy.GetComponent<Entity>();
            entityTarget.TakeDamage();
        }
    }

    private void TakeDamage()
    {
        currentHealth--;

        PlayDamageFeedback();

        if (this is PlayerController player)
        {
            player.UpdateHealthUI();
        }

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        anim.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        Destroy(gameObject, 3);
    }

    private void PlayDamageFeedback()
    {
        if (damageFeedbackCoroutine != null)
            StopCoroutine(DamageFeedbackCoroutine());

        StartCoroutine(DamageFeedbackCoroutine());
    }

    private IEnumerator DamageFeedbackCoroutine()
    {
        Material originalMat = sr.material;
        Color originalColor = sr.color;

        sr.color = Color.white;          // force neutral color
        sr.material = damageMaterial;

        yield return new WaitForSeconds(damageFeedbackDuration);

        sr.material = originalMat;
        sr.color = originalColor;        // restore original tint
    }

    //=====// GROUND CHECK METHODS //=====//

    protected virtual void HandleCollision() //Does the detection if player is grounded
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos() //Draws visual line for checking if player is grounded
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));

        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    //=====// GROUND CHECK METHODS //=====//

    protected virtual void HandleFlip()
    {
        if (rb.linearVelocity.x > 0 && facingRight == false)
            Flip();
        else if (rb.linearVelocity.x < 0 && facingRight == true)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        //facingDir = facingDir * -1;
    }


    //=====// ANIMATION METHODS //=====//

    protected virtual void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }


    //=====// MOVEMENT //=====//

    public virtual void EnableMovement(bool enable)
    {
        canMove = enable;
    }

    protected virtual void HandleMovement()
    {
        // Handled in child classes
    }

}
