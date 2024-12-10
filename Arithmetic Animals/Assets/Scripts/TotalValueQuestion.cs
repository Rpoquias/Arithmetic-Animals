using UnityEngine;
using TMPro; // Make sure to import TMP for TextMeshPro
using UnityEngine.UI; // Correct namespace for Button

public class TotalValueQuestion : MonoBehaviour
{
    public GameManager gameManager;  // Reference to the GameManager to get the total value
    public TMP_Text questionText;    // TextMesh Pro component for displaying the question
    public TMP_InputField answerInput;  // TextMesh Pro InputField to capture player input
    public Button submitButton;      // Button to submit the answer
    public GameObject questionPanel; // Panel that holds the question UI

    private int correctAnswer;

    void Start()
    {
        // Hide the panel initially
        questionPanel.SetActive(false);

        // Set up button listener
        submitButton.onClick.AddListener(SubmitAnswer);
    }

    public void ShowQuestion()
    {
        // Get the correct answer (total value of all animals)
        correctAnswer = gameManager.GetInitialTotal();  // Use the initial total of animals

        // Display the question UI
        questionPanel.SetActive(true);

        // Set the question text
        questionText.text = "What is the total value of all the animals?";

        // Clear the input field
        answerInput.text = "";
    }

    public void SubmitAnswer()
    {
        int playerAnswer;
        if (int.TryParse(answerInput.text, out playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                Debug.Log("Correct Answer!");
                // Handle correct answer logic (e.g., reward the player)
            }
            else
            {
                Debug.Log("Incorrect Answer.");
                // Handle incorrect answer logic (e.g., show feedback)
            }
        }
        else
        {
            Debug.Log("Invalid input!");
            // Optionally show a warning if the input is not valid
        }

        // Hide the question UI after submission
        questionPanel.SetActive(false);
    }
}
