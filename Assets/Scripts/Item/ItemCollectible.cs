using UnityEngine;


public class ItemCollectible : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //collision.GetComponent<Health>().Heal(scoreCount); 
            gameObject.SetActive(false); // Disable the collectible item after collection
        }
    }

     
}
