using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int initialTotal = 0; // The total value of all animals at the start of the game
    private int persistentTotal = 0; // The accumulated total value of animals (including destroyed ones)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Calculate the initial total of all animals in the scene at the start
        CalculateInitialTotal();
    }

    // Calculate and store the total value of all animals present at the start of the game
    private void CalculateInitialTotal()
    {
        initialTotal = 0;
        foreach (Animal animal in FindObjectsOfType<Animal>())
        {
            initialTotal += animal.animalValue;
        }
        Debug.Log($"Initial animal total calculated: {initialTotal}");
    }

    // Add an animal's value to the persistent total (e.g., for destroyed animals)
    public void AddToTotal(int value)
    {
        persistentTotal += value;
        Debug.Log($"Persistent total updated: {persistentTotal}");
    }

    // Get the total value of all animals at the start (initial value)
    public int GetInitialTotal()
    {
        return initialTotal;  // Return the pre-calculated total from the start
    }

    // Calculate the total value including both the initial and destroyed animal values
    public int CalculateTotal()
    {
        return initialTotal + persistentTotal;
    }

    private void Update()
    {
        Debug.Log($"Initial Total: {initialTotal}, Persistent Total (destroyed): {persistentTotal}");
    }
}
