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
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddToTotal(animal.animalValue);
                    Debug.Log($"Animal {animal.gameObject.name} added its value to persistent total: {animal.animalValue}");
                }

                Destroy(collision.gameObject);
                Debug.Log($"{collision.gameObject.name} destroyed.");
            }
        }
    }
}
