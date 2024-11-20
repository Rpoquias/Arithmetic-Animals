using UnityEngine;

public class GameManager : MonoBehaviour
{
    // References to all the animals in the scene
    public GameObject[] animals; // This will store all the animal GameObjects
    public int totalValue = 0;   // To hold the total value

    void Start()
    {
        animals = GameObject.FindGameObjectsWithTag("Animal"); // Ensure animals have the "Animal" tag
    }

    // Update the total value based on the animal values
    void Update()
    {
        CalculateTotalValue();
    }

    // Method to calculate the total value based on the values in AnimalValue scripts
    void CalculateTotalValue()
    {
        totalValue = 0; // Reset the total value each time

        // Loop through each animal in the animals array
        foreach (GameObject animal in animals)
        {
            // Get the AnimalValue script attached to each animal
            AnimalValue animalValueScript = animal.GetComponent<AnimalValue>();

            if (animalValueScript != null)
            {
                // Add the animal's value to the total value
                totalValue += animalValueScript.animalValue;
            }
        }

        // Output the total value for debugging
        Debug.Log("Total Value of Animals: " + totalValue);
    }
}