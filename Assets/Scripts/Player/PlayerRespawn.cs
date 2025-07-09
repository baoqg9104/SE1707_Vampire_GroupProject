using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 currentCheckpoint;

    private void Start()
    {
        startPosition = transform.position;
        currentCheckpoint = startPosition;
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    public void Respawn()
    {
        transform.position = currentCheckpoint;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Reset linear velocity to prevent being affected by previous forces
        }

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Idle"); // Reset animation to idle state
        }

        EnableController(); // Re-enable player controller
    }

    private void EnableController()
    {
        if (GetComponent<PlayerController>() != null)
            GetComponent<PlayerController>().enabled = true; // Re-enable player controller
        if (GetComponent<PinkManController>() != null)
            GetComponent<PinkManController>().enabled = true; // Re-enable PinkMan controller
    }
}
