using UnityEngine;

public class FatBirdController : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask groundLayer;
    public GameObject feedObject;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isHit = false;
    private bool isGrounded = false;
    public float force = 5f; // Lực đẩy lên khi bị hit

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0f; // Không rơi cho đến khi bị hit
    }

    void Update()
    {
        if (isHit)
        {
            isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);

            animator.SetBool("isFalling", !isGrounded);
            animator.SetBool("isGrounded", isGrounded);

            // Rơi thẳng đứng => không trượt ngang
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            // Biến mất khi chạm đất (nếu muốn)
            if (isGrounded)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TakeHit()
    {
        if (isHit) return;

        isHit = true;
        animator.SetTrigger("isHit");
        rb.gravityScale = 1f;

        // Đẩy nhẹ lên trên và đảm bảo rơi thẳng
        rb.linearVelocity = new Vector2(0f, force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == feedObject)
        {
            TakeHit(); // Bị hit: bắt đầu rơi
        }
    }
}
