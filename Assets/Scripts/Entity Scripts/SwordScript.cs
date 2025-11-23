using UnityEngine;
using UnityEngine.InputSystem;

public class SwordScript : MonoBehaviour
{
    Animator animator;
    protected PlayerController pc;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    private void TryAttack(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Attack");
    }
}
