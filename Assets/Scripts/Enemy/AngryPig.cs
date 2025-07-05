using Unity.VisualScripting;
using UnityEngine;

public class AngryPig : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float idleTimeMin = 1f;
    public float idleTimeMax = 3f;
    public Transform leftBound;
    public Transform rightBound;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody2D rb;

    private bool movingRight = true;
    private bool isIdle = false;
    private bool isHitOnce = false;
    private bool isDead = false;
    private float idleTimer = 0f;
    private float currentSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (rightBound != null)
        {
            transform.position = rightBound.position;

        }
        movingRight = false;
        currentSpeed = walkSpeed;
        animator.Play("Walk");
    }

    void Update()
    {
        if (isDead) return;

        if (isHitOnce)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        if (isIdle)
        {
            // Xử lý thời gian idle
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0f)
            {
                isIdle = false;
                animator.Play(isHitOnce ? "Run" : "Walk");
            }
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Dừng di chuyển khi idle
            return;
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on the AngryPig object.");
            return;
        }

        // if boudary is null, do not move
        if (leftBound == null || rightBound == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Di chuyển
        float moveDirection = movingRight ? 1 : -1;
        rb.linearVelocity = new Vector2(moveDirection * currentSpeed, rb.linearVelocity.y);

        // Kiểm tra biên
        if (movingRight && transform.position.x >= rightBound.position.x)
        {
            ReachedBound();
        }
        else if (!movingRight && transform.position.x <= leftBound.position.x)
        {
            ReachedBound();
        }
    }

    void ReachedBound()
    {
        if (isHitOnce)
        {
            // Nếu đã bị đánh thì không idle, quay đầu ngay
            movingRight = !movingRight;
            FlipSprite();
        }
        else
        {
            // Chưa bị đánh thì idle một lúc
            isIdle = true;
            idleTimer = Random.Range(idleTimeMin, idleTimeMax);
            animator.Play("Idle");

            // Sau khi idle xong mới quay đầu
            movingRight = !movingRight;
            FlipSprite();
        }
    }

    void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeHit()
    {
        if (isDead) return;

        if (!isHitOnce)
        {
            // Bị đánh lần đầu
            isHitOnce = true;
            currentSpeed = runSpeed;
            animator.Play("Hit1");
            // Sau khi hit animation xong thì chạy
            Invoke("PlayRunAfterHit", 0.5f);
        }
        else
        {
            // Bị đánh lần 2 - chết
            isDead = true;
            animator.Play("Hit2");
            // Sau khi hit animation xong thì biến mất
            Invoke("Die", 0.5f);
        }
    }

    void PlayRunAfterHit()
    {
        if (!isDead)
        {
            animator.Play("Run");
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    // Gọi khi player tấn công enemy
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
                var playerScript = other.gameObject.GetComponent<PlayerController>(); // Đổi PlayerController thành script player của bạn
                if (playerScript != null)
                {
                    playerScript.Hit();
                }

                // Thêm lực đẩy lùi cho player
                Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    float pushDirection = (other.transform.position.x < transform.position.x) ? -1f : 1f;
                    float pushForce = 10f; // Có thể điều chỉnh lực này
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
}