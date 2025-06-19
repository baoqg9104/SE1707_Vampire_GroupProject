using UnityEngine;

public class CheckpointFlag : MonoBehaviour
{
    private Animator animator;
    private bool isActivated = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("NoFlag");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;
            animator.Play("Out");
        }
    }

    public void SetIdle()
    {
        animator.Play("Idle");
    }
}
