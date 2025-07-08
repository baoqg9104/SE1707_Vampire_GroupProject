using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private float jumpForce = 12f;
    private int maxJumps = 2;

    [Header("Checks")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount;
    private bool isHit = false;
    private bool wasGrounded;
    [SerializeField] private double damage;

    private float lastPlayerDirection = 0f;
    private float targetDirection = 0f;
    private bool isChangingDirection = false;



    ///////Score system////////
    //public ScoreManager cm;

    [Header("Target")]
    public Transform player;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps;
        animator = GetComponent<Animator>();

         // Initialize directions
        if (player != null)
        {
            targetDirection = Mathf.Sign(player.position.x - transform.position.x);
            lastPlayerDirection = targetDirection;
        }
        else
        {
            targetDirection = 1f;
            lastPlayerDirection = 1f;
        }
    }

    void Update()
    {
        if (isHit) return;

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Reset jump count only when just landed
        if (isGrounded && !wasGrounded)
        {
            jumpCount = maxJumps;
        }
        wasGrounded = isGrounded;

        // Boss follows the player horizontally
        if (player != null)
        {
            float playerDirection = Mathf.Sign(player.position.x - transform.position.x);

            if (playerDirection != 0 && playerDirection != lastPlayerDirection && !isChangingDirection)
            {
                StartCoroutine(DelayDirectionChange(playerDirection));
            }

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * targetDirection;
            transform.localScale = scale;

            rb.linearVelocity = new Vector2(targetDirection * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        // Jump logic can be added here if you want the boss to jump

        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount = maxJumps - 1;
            animator.Play("Boss");
        }
        else if (jumpCount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount--;
            animator.Play("Boss");
        }
        // Animation state
        //if (isHit)
        //    animator.Play("Hit");
        if (!isGrounded && rb.linearVelocity.y > 0)
        {
            if (jumpCount == maxJumps - 1)
                animator.Play("Boss");
            else
                animator.Play("Boss");
        }
        else if (!isGrounded && rb.linearVelocity.y < 0)
        {
            animator.Play("Boss");
        }
        else if (isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            animator.Play("Boss");
        }
        else if (isGrounded)
        {
            animator.Play("Boss");
        }
    }

    public void Hit()
    {
        if (!isHit)
        {
            isHit = true;
            //animator.Play("Hit");
            StartCoroutine(HitRecover());
            // Call TakeDamage from Health class
            Health health = GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage((float)damage); // Assuming 1 is the damage amount
            }
        }
    }

    private IEnumerator HitRecover()
    {
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = collision.transform;
            moveSpeed = 10f;
        }
        else if (collision.gameObject.CompareTag("SpikeBall") || collision.gameObject.CompareTag("Saw") || collision.gameObject.CompareTag("Thorn") || collision.gameObject.CompareTag("Spike"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 13f);

            Hit();
        }

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
            moveSpeed = 5f; // Reset speed when leaving the platform
        }
    }
    

    private IEnumerator DelayDirectionChange(float newDirection)
    {
        isChangingDirection = true;
        yield return new WaitForSeconds(1f);
        targetDirection = newDirection;
        lastPlayerDirection = newDirection;
        isChangingDirection = false;
    }

     



}
