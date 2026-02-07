
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Entity
{

    //=====// DEFINITIONS //=====//

    [Header("Settings")]
    public float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;

    private PlayerControls input;
    private float horizontal;

    public static event Action OnPlayerDie;
    public static event Action<int> OnHealthChanged;

    //=====// EVENT METHODS //=====//

    protected override void Awake()
    {
        base.Awake();
        
        UpdateHealthUI();

        input = GlobalInputManager.Controls;
    }

    protected override void Update()
    {
        base.Update();
        ReadMoveInput();
    }

    private void OnEnable()
    {
        EnablePlayerInput();
        input.UI.Enable();

        input.Gameplay.Attack.performed += TryAttack;
        input.Gameplay.Jump.performed += TryJump;
    }

    private void OnDisable()
    {
        input.Gameplay.Attack.performed -= TryAttack;
        input.Gameplay.Jump.performed -= TryJump;

        DisablePlayerInput();
        input.UI.Disable();
    }


    //=====// INPUT MANAGEMENT //=====//

    private void ReadMoveInput()
    {
        horizontal = input.Gameplay.Move.ReadValue<float>();
    }

    public void EnablePlayerInput()
    {
        input.Gameplay.Enable();
    }

    public void DisablePlayerInput()
    {
        input.Gameplay.Disable();
    }


    //=====// MOVEMENT METHODS //=====//

    private void TryJump(InputAction.CallbackContext ctx)
    {
        if (rb != null && isGrounded && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    protected override void HandleMovement()
    {
        // Ensure Rigidbody exists before applying velocity and if move is enabled
        if (rb != null && canMove)
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    protected override void HandleFlip()
    {
        if (!canFlip)
            return;
            
        float moveInput = horizontal;

        if ((moveInput > 0 && !facingRight) || (moveInput < 0 && facingRight))
            Flip();
    }

    //=====// HEALTH METHODS //=====//

    public void UpdateHealthUI()
    {
        OnHealthChanged?.Invoke(GetCurrentHealth);
    }

    protected override void Die()
    {
        OnPlayerDie?.Invoke(); // this fires the event of player death
        base.Die();
    }



}

