using UnityEngine;

public class PinkManController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public bool isFacingRight = true;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public int maxJumps = 2;
    int jumpRemaining;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    public LayerMask groundLayer;
    bool isGrounded;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 20f;
    public float fallMultiplier = 2.5f;

    [Header("Wall Check")]
    public Transform wallCheck;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.1f);
    public LayerMask wallLayer;

    [Header("Wall Movement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;

    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.2f;
    float wallJumpTimer;
    public Vector2 wallJumpForce = new Vector2(10f, 10f);

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GroundCheck();
        Gravity();
        WallSlide();
        HandleWallJumpTimer();

        horizontalMovement = 0f;
        if (Input.GetKey(KeyCode.A)) horizontalMovement = -1f;
        if (Input.GetKey(KeyCode.D)) horizontalMovement = 1f;

        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            Flip();
        }

        // Jump input
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryJump();
        }

        // Animator updates
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
        animator.SetBool("isWallJumping", isWallJumping);
    }

    void TryJump()
    {
        if (wallJumpTimer > 0f)
        {
            WallJumpExecute();
            return;
        }

        if (jumpRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            if (isGrounded)
            {
                animator.SetTrigger("jump");
            }
            else
            {
                animator.SetTrigger("doubleJump");
            }

            jumpRemaining--;
        }
    }

    void WallJumpExecute()
    {
        isWallJumping = true;
        rb.linearVelocity = new Vector2(wallJumpForce.x * wallJumpDirection, wallJumpForce.y);
        wallJumpTimer = 0f;
        animator.SetTrigger("jump");

        if ((wallJumpDirection > 0 && !isFacingRight) || (wallJumpDirection < 0 && isFacingRight))
        {
            Flip();
        }

        Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
    }

    void CancelWallJump()
    {
        isWallJumping = false;
    }

    void HandleWallJumpTimer()
    {
        if (isWallSliding)
        {
            wallJumpDirection = -Mathf.Sign(transform.localScale.x);
            wallJumpTimer = wallJumpTime;
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        if (isGrounded)
        {
            jumpRemaining = maxJumps;
        }
    }

    void WallSlide()
    {
        if (!isGrounded && WallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }

    bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0f, wallLayer);
    }

    void Flip()
    {
        if ((isFacingRight && horizontalMovement < 0) || (!isFacingRight && horizontalMovement > 0))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }
}
