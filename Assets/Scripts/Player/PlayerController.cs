using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float wallJumpForce = 10f;
    public float wallJumpDirection = 1f;
    public int maxJumps = 2;

    [Header("Checks")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;

    private Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isTouchingWall;
    private int jumpCount;
    private bool isFacingRight = true;
    private bool isHit = false;
    private bool wasGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isHit) return;

        // Ground & wall check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, Vector2.right * (isFacingRight ? 1 : -1), wallCheckDistance, groundLayer);

        // Reset jump count only when just landed
        if (isGrounded && !wasGrounded)
        {
            jumpCount = maxJumps;
        }
        wasGrounded = isGrounded;

        // Horizontal movement
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.A)) moveInput = -1f;
        if (Input.GetKey(KeyCode.D)) moveInput = 1f;

        // Ngăn dính tường: Nếu trên không và chạm tường và đang giữ hướng về phía tường, không cho di chuyển ngang về phía tường
        bool movingTowardsWall = (isTouchingWall && !isGrounded && ((moveInput > 0 && isFacingRight) || (moveInput < 0 && !isFacingRight)));
        if (!movingTowardsWall)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
        // Nếu đang dính tường và giữ hướng về phía tường, giữ nguyên vận tốc ngang (không set lại)

        // Flip
        if (moveInput > 0 && !isFacingRight) Flip();
        if (moveInput < 0 && isFacingRight) Flip();

        // Jump
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpCount = maxJumps - 1;
                animator.Play("Jump");
            }
            else if (jumpCount > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpCount--;
                animator.Play("DoubleJump");
            }
            else if (isTouchingWall)
            {
                rb.linearVelocity = new Vector2(-wallJumpDirection * (isFacingRight ? 1 : -1) * wallJumpForce, jumpForce);
                Flip();
                animator.Play("WallJump");
            }
        }

        // Animation state
        if (isHit)
        {
            animator.Play("Hit");
        }
        else if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            animator.Play("WallJump");
        }
        else if (!isGrounded && rb.linearVelocity.y > 0)
        {
            if (jumpCount == maxJumps - 1)
                animator.Play("Jump");
            else
                animator.Play("DoubleJump");
        }
        else if (!isGrounded && rb.linearVelocity.y < 0)
        {
            animator.Play("Fall");
        }
        else if (isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            animator.Play("Run");
        }
        else if (isGrounded)
        {
            animator.Play("Idle");
        }
    }

    public void Hit()
    {
        isHit = true;
        animator.Play("Hit");
        // Thêm logic bị hit nếu cần
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}