using UnityEngine;

public class FatBird : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform upperBound;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private Animator animator;
    private Rigidbody2D rb;
    private bool movingUp = true;
    private bool isDead = false;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        animator.Play("Fall"); // Start with falling animation
        movingUp = false; // Initially set to falling
    }

    void Update()
    {
        if (isDead) return;

        if (movingUp)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveSpeed);

            if (transform.position.y >= upperBound.position.y)
            {
                movingUp = false; // Switch to falling
                animator.Play("Fall"); // Switch to falling animation
            }
        }
        else
        {
            if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer))
            {
                movingUp = false;
                animator.Play("Idle"); // Switch to idle animation
                Invoke("StartMovingUp", 0.5f); // Wait for 1 second before moving up
            }
        }
    }

    public void TakeHit()
    {
        if (isDead) return;

        isDead = true;
        animator.Play("Hit");

        // Sau khi hit animation xong thì biến mất
        Invoke("Die", 0.5f);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (IsPlayerAbove(other.gameObject.transform))
            {
                TakeHit();

                // Thêm lực đẩy player lên khi giẫm lên enemy
                Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
                    playerRb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
                }
            }
            else
            {
                // Player bị hit nếu không nhảy lên đầu enemy
                var playerScript = other.gameObject.GetComponent<PlayerController>();
                if (playerScript != null)
                {
                    playerScript.Hit();
                }

                // Thêm lực đẩy lùi cho player
                Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    float pushDirection = (other.transform.position.x < transform.position.x) ? -1f : 1f;
                    float pushForce = 10f;
                    playerRb.AddForce(new Vector2(pushDirection * pushForce, 5f), ForceMode2D.Impulse);
                }
            }
        }
    }

    bool IsPlayerAbove(Transform player)
    {
        // Tính toán vị trí tương đối
        float playerBottom = player.position.y - player.GetComponent<Collider2D>().bounds.extents.y;
        float enemyTop = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;

        // Kiểm tra player có đang ở phía trên enemy không
        return playerBottom > enemyTop;
    }

    void StartMovingUp()
    {
        movingUp = true; // Start moving up
    }
}