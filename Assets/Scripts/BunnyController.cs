using UnityEngine;

public class BunnyController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 10f;
    public float patrolRange = 8f;
    public float jumpInterval = 2.5f;
    public Transform groundCheck;
    public Transform wallCheck;
    public GameObject feedObject;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool isHit = false;
    private float jumpTimer;
    private Vector2 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpTimer = jumpInterval;
        startPos = transform.position;
    }

    void Update()
    {
        // === Ground Check ===
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(groundCheck.position, Vector2.down * 0.2f, isGrounded ? Color.green : Color.red);

        if (isHit)
        {
            // Rơi thẳng xuống, đứng yên theo phương X
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", !isGrounded && rb.linearVelocity.y > 0.1f);
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("yVelocity", rb.linearVelocity.y);

            if (isGrounded)
            {
                Destroy(gameObject);
            }

            return;
        }

        // === Movement Logic ===
        float dir = isFacingRight ? 1f : -1f;
        Vector2 velocity = rb.linearVelocity;
        velocity.x = dir * moveSpeed;

        // === Jump by Timer ===
        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0f && isGrounded)
        {
            velocity.y = jumpForce;
            jumpTimer = jumpInterval;
        }

        // === Wall Check ===
        Vector2 wallDir = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D wallHit = Physics2D.Raycast(wallCheck.position, wallDir, 0.1f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(wallCheck.position, wallDir * 0.1f, wallHit.collider ? Color.red : Color.green);

        if (wallHit.collider)
        {
            if (isGrounded)
            {
                velocity.y = jumpForce;
                jumpTimer = jumpInterval;
            }
            else
            {
                isFacingRight = !isFacingRight;
            }
        }

        // === Flip Scale ===
        if ((isFacingRight && transform.localScale.x < 0) || (!isFacingRight && transform.localScale.x > 0))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        // === Limit Patrol Range ===
        float distanceFromStart = transform.position.x - startPos.x;
        if (Mathf.Abs(distanceFromStart) > patrolRange)
        {
            isFacingRight = distanceFromStart < 0;
        }

        // === Apply Final Velocity ===
        rb.linearVelocity = velocity;

        // === Animator Sync ===
        float rawSpeed = Mathf.Abs(rb.linearVelocity.x);
        float speed = rawSpeed < 0.05f ? 0f : rawSpeed;
        bool isJumping = !isGrounded && rb.linearVelocity.y > 0.1f;

        animator.SetBool("isRunning", isGrounded && speed > 0f);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public void TakeHit()
    {
        if (isHit) return;

        isHit = true;
        animator.SetTrigger("isHit");
        // Không đẩy lên nữa —> chỉ để trọng lực kéo rơi xuống
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == feedObject)
        {
            TakeHit();
        }
    }
}
