using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 7f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float horizontalInput;
    private bool isGrounded;
    private bool jumpRequested;
    private int facingDirection = 1; 

    private const int STATE_IDLE = 0;
    private const int STATE_WALK_RIGHT = 1;
    private const int STATE_WALK_LEFT = 2;
    private const int STATE_JUMP_RIGHT = 3;
    private const int STATE_JUMP_LEFT = 4;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0.01f)
        {
            facingDirection = 1;
        }
        else if (horizontalInput < -0.01f)
        {
            facingDirection = -1;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }

        int moveState = STATE_IDLE;

        if (!isGrounded)
        {
            if (facingDirection == 1)
            {
                moveState = STATE_JUMP_RIGHT;
            }
            else
            {
                moveState = STATE_JUMP_LEFT;
            }
        }
        else
        {
            if (horizontalInput > 0.01f)
            {
                moveState = STATE_WALK_RIGHT;
            }
            else if (horizontalInput < -0.01f)
            {
                moveState = STATE_WALK_LEFT;
            }
        }

        animator.SetInteger("moveState", moveState);
    }

    private void FixedUpdate()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        Vector2 velocity = rb.velocity;
        velocity.x = horizontalInput * moveSpeed;

        if (jumpRequested)
        {
            velocity.y = jumpForce;
            jumpRequested = false;
        }

        rb.velocity = velocity;
    }
}
