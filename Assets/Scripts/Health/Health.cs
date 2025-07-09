using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    private PlayerRespawn playerRespawn;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        playerRespawn = GetComponent<PlayerRespawn>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            // Debug.Log("Current Health: " + currentHealth);
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!dead && currentHealth == 0)
            {
                Die();
            }
        }
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        TakeDamage((float)0.5);
    //    }


    //}

    public void Die()
    {
        anim.SetTrigger("die");
        if (GetComponent<PlayerController>() != null)
            GetComponent<PlayerController>().enabled = false; // Disable player controller on death
        if (GetComponent<PinkManController>() != null)
            GetComponent<PinkManController>().enabled = false;
        dead = true;

        Invoke("Respawn", 2f);
        // Invoke("EnableController", 2.1f); // Re-enable controller slightly after respawn
    }

    private void Respawn()
    {
        playerRespawn.Respawn();

        // Reset health to starting health
        currentHealth = startingHealth;
        dead = false;
    }

    public void Heal(float _healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + _healAmount, 0, startingHealth);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillZone"))
        {
            Invoke("Respawn", 0.5f);
        }
    }
}


