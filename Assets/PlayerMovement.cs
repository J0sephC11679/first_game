using UnityEngine;
using Unity.Netcode.Components;
using Unity.Netcode;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private int maxJumps = 2;
    private int jumpsRemaining;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private bool hasDashed;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    void Start()
    {
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        Move();

        if (IsGrounded())
        {
            jumpsRemaining = maxJumps;
            hasDashed = false;
        }
    }

    public void Move()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // PRESS
        if (context.started)
        {
            if (jumpsRemaining > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
                jumpsRemaining--;
            }
        }

        // RELEASE
        if (context.canceled)
        {
            if (rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }


    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash && !hasDashed)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        hasDashed = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float direction = isFacingRight ? 1f : -1f;
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = new Vector2(direction * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
    }
}