using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEditor.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private float doubleTapTime = 0.25f;
    private float lastTapTimeRight;
    private float lastTapTimeLeft;

    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private int maxJumps = 2;
    private int jumpsRemaining;

    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.4f;
    private bool hasDashed;

    private bool isCrouching;
    private Vector2 originalOffset;
    private Vector2 originalSize;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private float crouchHeight;
    [SerializeField] private BoxCollider2D col;

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite crouchSprite;

    void Start()
    {
        jumpsRemaining = maxJumps;

        tr.emitting = false;

        originalSize = col.size;
        originalOffset = col.offset;
    }

    void Update()
    {
        if (IsGrounded())
        {
            jumpsRemaining = maxJumps;
            hasDashed = false;
            return;
        }

        if (!isDashing && rb.gravityScale == 0f)
        {
            rb.gravityScale = 1f;
        }

        Flip();
    }


    public void OnMove(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();

        if (context.started)
        {
            if (moveInput.x > 0)
            {
                if (Time.time - lastTapTimeRight < doubleTapTime && !isDashing && !hasDashed)
                {
                    StartCoroutine(Dash(1));
                }
                lastTapTimeRight = Time.time;
            }

            if (moveInput.x < 0)
            {
                if (Time.time - lastTapTimeLeft < doubleTapTime && !isDashing && !hasDashed)
                {
                    StartCoroutine(Dash(-1));
                }
                lastTapTimeLeft = Time.time;
            }
        }
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
        
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        if (isCrouching)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && moveInput.x < 0f || !isFacingRight && moveInput.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash(int direction)
    {
        isDashing = true;
        hasDashed = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        tr.emitting = true;
        rb.linearVelocity = new Vector2(direction * dashingPower, 0f);

        yield return new WaitForSeconds(dashingTime);

        rb.gravityScale = originalGravity;
        tr.emitting =  false;
        isDashing = false;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartCrouch();
        }
        else if (context.canceled)
        {
            StopCrouch();
        }
    }

    void StartCrouch()
    {
        if (isCrouching || !IsGrounded()) return;

        isCrouching = true;

        sr.sprite = crouchSprite;

        float newHeight = originalSize.y * 0.5f;
        col.size = new Vector2(originalSize.x, newHeight);

        float offsetDifference = (originalSize.y - newHeight) / 2f;
        col.offset = new Vector2(originalOffset.x, originalOffset.y - offsetDifference);
    }

    void StopCrouch()
    {
        isCrouching = false;

        sr.sprite = idleSprite;

        col.size = originalSize;
        col.offset = originalOffset;
    }
}