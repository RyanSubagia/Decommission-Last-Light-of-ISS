using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float horizontalInput;

    private const int STATE_IDLE = 0;
    private const int STATE_WALK_RIGHT = 1;
    private const int STATE_WALK_LEFT = 2;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        int moveState = STATE_IDLE;

        if (horizontalInput > 0.01f)
        {
            moveState = STATE_WALK_RIGHT;
        }
        else if (horizontalInput < -0.01f)
        {
            moveState = STATE_WALK_LEFT;
        }

        animator.SetInteger("moveState", moveState);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }
}
