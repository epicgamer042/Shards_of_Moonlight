using UnityEngine;

public class Entity : MonoBehaviour
{

    //=====// DEFINITIONS //=====//

    protected Rigidbody2D rb;
    protected Collider2D col;

    [Header("Health")]
    [SerializeField] private int maxHealth = 7;
    [SerializeField] private int currentHealth;
    

    [Header("Collision Details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] protected bool isGrounded;


    //=====// EVENT METHODS //=====//

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        HandleCollision();
    }


    //=====// HEALTH & DAMAGE METHODS //=====//

    public int GetCurrentHealth => currentHealth; //For (player) child access

    private void TakeDamage()
    {
        currentHealth--;

        //PlayDamageFeedback();

        if (this is PlayerController player)
        {
            player.UpdateHealthUI();
        }

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        //anim.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        Destroy(gameObject, 3);
    }


    //=====// GROUND CHECK METHODS //=====//

    protected virtual void HandleCollision() //Does the detection if player is grounded
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos() //Draws visual line for checking if player is grounded
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));

         //if (attackPoint != null)
             //Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
