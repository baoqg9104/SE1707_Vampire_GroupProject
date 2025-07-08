using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
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
                anim.SetTrigger("die");
                if (GetComponent<PlayerController>() != null)
                    GetComponent<PlayerController>().enabled = false; // Disable player controller on death
                if (GetComponent<PinkManController>() != null)
                    GetComponent<PinkManController>().enabled = false;
                dead = true;

                 
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

    public void Heal(float _healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + _healAmount, 0, startingHealth);
        Debug.Log("Current Health: " + currentHealth);
    }
}


