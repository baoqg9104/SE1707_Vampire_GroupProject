using UnityEngine;

public class FatBirdController : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isHit = false;
    private bool isGrounded = false;

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
        }
    }

    // Gọi hàm này từ bên ngoài (ví dụ từ enemy, trigger, v.v.) để làm nó rớt
    public void TakeHit()
    {
        if (isHit) return;

        isHit = true;
        animator.SetTrigger("isHit");
        rb.gravityScale = 1f;
    }
}
