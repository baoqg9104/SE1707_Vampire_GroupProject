using UnityEngine;

public class AngryPig : MonoBehaviour
{
    public enum State { Idle, Walk, Run, Hit1, Hit2 }
    public State currentState = State.Idle;

    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;
    public float detectionRange = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private Animator animator;
    public int maxHealth = 2;

    private int currentHealth;
    private Rigidbody2D rb;
    private Transform player;
    private bool facingRight = true;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                animator.Play("Idle");
                if (distanceToPlayer < detectionRange)
                {
                    currentState = State.Walk;
                }
                break;
            case State.Walk:
                animator.Play("Walk");
                Move(walkSpeed);
                if (distanceToPlayer >= detectionRange)
                {
                    currentState = State.Idle;
                }
                break;
            case State.Run:
                animator.Play("Run");
                Move(runSpeed);
                break;
            case State.Hit1:
                animator.Play("Hit1");
                break;
            case State.Hit2:
                animator.Play("Hit2");
                break;
        }
    }

    void Move(float speed)
    {
        if (!isGrounded) return;
        rb.linearVelocity = new Vector2((facingRight ? 1 : -1) * speed, rb.linearVelocity.y);
        // Check for wall or edge
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, groundLayer);
        if (!groundInfo.collider)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void OnHitByPlayer()
    {
        if (currentHealth == 2)
        {
            currentState = State.Hit1;
            currentHealth--;
            Invoke(nameof(EnterRunState), 0.5f);
        }
        else if (currentHealth == 1)
        {
            currentState = State.Hit2;
            currentHealth--;
            Invoke(nameof(DestroyPig), 0.5f);
        }
    }

    void EnterRunState()
    {
        currentState = State.Run;
    }

    void DestroyPig()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Kiểm tra nếu player nhảy lên đầu
            if (collision.contacts[0].normal.y > 0.5f)
            {
                OnHitByPlayer();
                // Đẩy player lên
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb) playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 10f);
            }
            else
            {
                // Có thể gây sát thương cho player ở đây nếu muốn
            }
        }
    }
}
