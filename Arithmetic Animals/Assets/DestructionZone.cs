using UnityEngine;

public class DestructionZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Animal")) // Ensure animals have the "Animal" tag
        {
            Destroy(collision.gameObject); // Destroy the animal that enters the zone
        }
    }
}