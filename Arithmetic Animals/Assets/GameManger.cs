
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Calculate the total value of all animals in the scene
    public int CalculateTotal()
    {
        int total = 0;
        foreach (Animal animal in FindObjectsOfType<Animal>())
        {
            total += animal.animalValue;
        }
        return total;
    }

   
}
