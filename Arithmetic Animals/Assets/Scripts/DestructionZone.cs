using UnityEngine;

public class DestructionZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Triggered by: {collision.gameObject.name}");

        if (collision.CompareTag("Animal"))
        {
            Animal animal = collision.GetComponent<Animal>();
            if (animal != null)
            {
                // Add the animal's value to a persistent total in GameManager
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddToTotal(animal.animalValue);
                    Debug.Log($"Animal {animal.gameObject.name} value {animal.animalValue} added to persistent total.");
                }

                // Destroy the animal GameObject
                Destroy(collision.gameObject);
                Debug.Log($"{collision.gameObject.name} destroyed.");
            }
        }
    }
}
