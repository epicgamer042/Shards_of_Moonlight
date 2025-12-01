
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : Entity
{

    //=====// DEFINITIONS //=====//

    [Header("Input Action References")]
    public InputActionReference attack;
    public InputActionReference jump;
    public InputActionReference move;

    [Header("Settings")]
    public float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;

    private float horizontal;

    [SerializeField] private InputActionAsset inputActions;
    private InputActionMap gameplayMap;

    public static event Action OnPlayerDie;
    public static event Action<int> OnHealthChanged;

    //=====// EVENT METHODS //=====//

    protected override void Awake()
    {
        base.Awake();
        UpdateHealthUI();
        gameplayMap = inputActions.FindActionMap("Gameplay", true);
        gameplayMap.Enable();
    }

    protected override void Update()
    {
        base.Update();
        ReadMoveInput();
    }

    private void OnEnable()
    {
        SetupAction(attack, TryAttack, true);
        SetupAction(jump, TryJump, true);
        SetupAction(move, null, true); // move has no event, just enabled
    }

    private void OnDisable()
    {
        SetupAction(attack, TryAttack, false);
        SetupAction(jump, TryJump, false);
        SetupAction(move, null, false);
    }


    //=====// INPUT ACTION MANAGEMENT //=====//

    private void SetupAction(InputActionReference actionRef, 
                             System.Action<InputAction.CallbackContext> callback, 
                             bool enable)
    {
        if (actionRef != null && actionRef.action != null)
        {
            if (enable)
            {
                if (callback != null) actionRef.action.performed += callback;
                actionRef.action.Enable();
            }
            else
            {
                if (callback != null) actionRef.action.performed -= callback;
                actionRef.action.Disable();
            }
        }
    }

    private void ReadMoveInput()
    {
        // Check if move action is valid before reading
        if (move != null && move.action != null)
        {
            horizontal = move.action.ReadValue<float>();
        }
    }

    public void EnablePlayerInput()
    {
        gameplayMap.Enable();
    }

    public void DisablePlayerInput()
    {
        gameplayMap.Disable();
    }


    //=====// MOVEMENT //=====//

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
        if (move != null && move.action != null && canFlip)
        {
            float moveInput = move.action.ReadValue<float>();

            if ((moveInput > 0 && !facingRight) || (moveInput < 0 && facingRight))
            {
                Flip();
            }
        }
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

