using UnityEngine;

public class GravityFlipZone : MonoBehaviour
{
    public bool invertGravity = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PinkManController player = other.GetComponent<PinkManController>();
        if (player != null)
        {
            player.SetGravityInverted(invertGravity);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PinkManController player = other.GetComponent<PinkManController>();
        if (player != null)
        {
            // Khi rời khỏi vùng, đảo lại trạng thái
            player.SetGravityInverted(!invertGravity);
        }
    }
}

