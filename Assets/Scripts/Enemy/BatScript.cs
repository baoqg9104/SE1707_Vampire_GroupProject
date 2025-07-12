using UnityEngine;

public class BatScript : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform leftBound;
    public Transform rightBound;
    private Animator animator;
    private Rigidbody2D rb;
    private bool movingRight = true;
    private bool isDead = false;

    public float verticalAmplitude = 0.5f;
    public float verticalFrequency = 2f;
    private float initialY;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        transform.position = rightBound.position;
        movingRight = false;
        animator.Play("Fly");
        initialY = transform.position.y;
    }

    void Update()
    {
        if (isDead) return;

        float moveDirection = movingRight ? 1 : -1;
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

        float verticalOffset = Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        transform.position = new Vector3(transform.position.x, initialY + verticalOffset, transform.position.z);


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
        movingRight = !movingRight;
        FlipSprite();
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

        isDead = true;
        animator.Play("Hit");

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

                Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
                    playerRb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
                }
            }
            else
            {
                var playerScript = other.gameObject.GetComponent<PlayerController>();
                if (playerScript != null)
                {
                    playerScript.Hit();
                }

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
        float playerBottom = player.position.y - player.GetComponent<Collider2D>().bounds.extents.y;
        float enemyTop = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;

        return playerBottom > enemyTop;
    }
}
