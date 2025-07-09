using UnityEngine;
using System.Collections;

public class RadishController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float pauseDuration = 1.5f;
    [SerializeField] private GameObject feedObject; // Đối tượng gây hit

    private Vector3 startPosition;
    private bool movingRight = true;
    private bool isPaused = false;
    private bool isHit = false;

    private Animator animator;

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isHit || isPaused) return;

        HandleMovement();
    }

    void HandleMovement()
    {
        float leftBound = startPosition.x - distance;
        float rightBound = startPosition.x + distance;

        animator.Play("Run");

        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound)
                StartCoroutine(PauseAndFlip(false));
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound)
                StartCoroutine(PauseAndFlip(true));
        }
    }

    IEnumerator PauseAndFlip(bool newDirection)
    {
        isPaused = true;

        if (movingRight != newDirection)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }

        movingRight = newDirection;

        animator.Play("Idle1");
        yield return new WaitForSeconds(0.5f);

        animator.Play("Idle2");
        yield return new WaitForSeconds(0.5f);

        animator.Play("Leafs");
        yield return new WaitForSeconds(3f);

        animator.Play("Idle1");
        yield return new WaitForSeconds(pauseDuration);

        isPaused = false;
    }

    public void TakeHit()
    {
        if (isHit) return;

        isHit = true;
        animator.Play("Hit");
        StartCoroutine(DestroyAfterHit());
    }

    IEnumerator DestroyAfterHit()
    {
        yield return new WaitForSeconds(1.0f); // Chờ cho animation Hit kết thúc
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == feedObject)
        {
            TakeHit();
        }
    }
}
