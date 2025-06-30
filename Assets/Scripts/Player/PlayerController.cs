using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed = 5f;
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


    ///////Score system////////
    public ScoreManager cm;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps;
        animator = GetComponent<Animator>();
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

        // Lấy input di chuyển ngang mượt, luôn cập nhật vận tốc ngang theo input
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (moveInput > 0 ? 1 : -1);
            transform.localScale = scale;
        }
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Jump
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount = maxJumps - 1;
            animator.Play("Jump");
        }
        else if (Input.GetKeyDown(KeyCode.W) && jumpCount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount--;
            animator.Play("DoubleJump");
        }

        // Animation state
        //if (isHit)
        //{
        //    animator.Play("Hit");
        //}
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
        else if (collision.gameObject.CompareTag("SpikeBall") || collision.gameObject.CompareTag("Saw") || collision.gameObject.CompareTag("Thorn"))
        {
            Debug.Log("Player hit by hazard: " + collision.gameObject.name);
            Vector2 pushDir = (transform.position - collision.transform.position).normalized;
            rb.linearVelocity = new Vector2(pushDir.x * 8f, 8f);

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

    void OnTriggerEnter2D(Collider2D items)
    {
        if (items.gameObject.CompareTag("Apple"))
        {
            cm.scoreCount += 1;
        }
        if (items.gameObject.CompareTag("Banana"))
        {
             cm.scoreCount += 2;
        }
        if (items.gameObject.CompareTag("WaterMelon"))
        {
            cm.scoreCount += 3;
        }
        if (items.gameObject.CompareTag("GoldenApple"))
        {
            cm.scoreCount += 5;
        }
    }



}