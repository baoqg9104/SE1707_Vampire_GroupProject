using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public string onState = "On";
    public string offState = "Off";
    public float fallDelay = 1f;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isFalling = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (animator != null)
            animator.Play(onState);
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!isFalling && other.gameObject.CompareTag("Player") && IsPlayerAbove(other.transform))
        {
            isFalling = true;
            Invoke(nameof(Fall), fallDelay);
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

    void Fall()
    {
        if (animator != null)
            animator.Play(offState);
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}