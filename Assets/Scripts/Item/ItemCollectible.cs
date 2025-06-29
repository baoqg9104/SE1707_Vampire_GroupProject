using UnityEngine;


public class ItemCollectible : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private int healthCount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().Heal(healthCount); 
            gameObject.SetActive(false); // Disable the collectible item after collection
        }
    }

     
}
