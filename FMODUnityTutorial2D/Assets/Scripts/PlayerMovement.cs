using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private LayerMask jumpableGround;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;
    private enum MovementState {idle, running, jumping, falling };
    private MovementState state = MovementState.idle;

    [HideInInspector]
    public float direction = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");

        Move();
        Jump();
        UpdateAnimation();
        SetDirection();
    }

    void Move() => rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    void Jump()
    {
        if (Input.GetButton("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void UpdateAnimation()
    {
        MovementState state;

        if(direction != 0)
        {
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }
        if (rb.velocity.y > .1)
        {
            state = MovementState.jumping;
        }
        if (rb.velocity.y < -.1)
        {
            state = MovementState.falling;
        }
        animator.SetInteger("state", (int)state);
    }

    void SetDirection()
    {
        if (direction > 0f && spriteRenderer.flipX)
            spriteRenderer.flipX = false;
        else if (direction < 0f && !spriteRenderer.flipX)
            spriteRenderer.flipX = true;
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);

    }
}
