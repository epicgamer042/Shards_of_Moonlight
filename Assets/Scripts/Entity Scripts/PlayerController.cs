
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Entity
{

    //=====// DEFINITIONS //=====//

    [Header("Player Health Bar")]
    [SerializeField] private TextMeshProUGUI healthValue;

    [Header("Input Action References")]
    public InputActionReference move;
    public InputActionReference fire;
    public InputActionReference jump;

    [Header("Settings")]
    public float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;

    private float horizontal;


    //=====// EVENT METHODS //=====//

    protected override void Awake()
    {
        base.Awake();
        UpdateHealthUI();
    }

    protected override void Update()
    {
        base.Update();

        // Check if move action is valid before reading
        if (move != null && move.action != null)
        {
            horizontal = move.action.ReadValue<float>();
        }
    }

    private void FixedUpdate()
    {
        // Ensure Rigidbody exists before applying velocity
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        }
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
                if (callback != null) actionRef.action.started += callback;
                actionRef.action.Enable();
            }
            else
            {
                if (callback != null) actionRef.action.started -= callback;
                actionRef.action.Disable();
            }
        }
    }
    private void OnEnable()
    {
        SetupAction(fire, TryFire, true);
        SetupAction(jump, TryJump, true);
        SetupAction(move, null, true); // move has no event, just enabled
    }

    private void OnDisable()
    {
        SetupAction(fire, TryFire, false);
        SetupAction(jump, TryJump, false);
        SetupAction(move, null, false);
    }


    //=====// FIRE METHOD //=====//

    private void TryFire(InputAction.CallbackContext context)
    {
            Debug.Log("Fired");
    }


    //=====// JUMP METHOD //=====//

    private void TryJump(InputAction.CallbackContext ctx)
    {
        if (rb != null && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    //=====// PLAYER HEALTH UI UPDATE //=====//

    public void UpdateHealthUI()
    {
        if (healthValue != null)
            healthValue.text = GetCurrentHealth.ToString();
    }
}

