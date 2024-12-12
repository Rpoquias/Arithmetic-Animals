using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

   
    private int initialTotal = 0;
    private int persistentTotal = 0;

    // Total value UI
    public TMP_Text questionText;
    public TMP_InputField answerInput;
    public Button submitButton;
    public GameObject questionPanel;

    private int correctAnswer;

    // Rare Animal UI
    public TMP_Text rareQuestionText;
    public TMP_InputField rareAnswerInput;
    public Button rareSubmitButton;
    public GameObject rareQuestionPanel;

    private int rareCorrectAnswer;

    // Main UI Canvas Group (the UI you want to block when the rare question UI is shown)
    public CanvasGroup mainUICanvasGroup;  // Reference to the Canvas Group of the main UI

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

        submitButton.onClick.AddListener(SubmitAnswer);
        rareSubmitButton.onClick.AddListener(SubmitRareAnimalAnswer);

        // Initially hide the panels
        questionPanel.SetActive(false);
        rareQuestionPanel.SetActive(false);
    }

    public void ShowRareAnimalQuestion(string question, int correctAnswer)
    {
        Debug.Log("Found Rare Animal! \r\nSolve the Problem!");

        // Freeze the game
        Time.timeScale = 0;

        // Disable interactions with the main UI (e.g., pause button)
        mainUICanvasGroup.interactable = false;
        mainUICanvasGroup.blocksRaycasts = false;

        // Show the rare animal question panel
        rareQuestionPanel.SetActive(true);

        // Set the question text and correct answer
        rareQuestionText.text = question;
        rareCorrectAnswer = correctAnswer;

        // Clear the input field
        rareAnswerInput.text = "";
    }

    private void SubmitRareAnimalAnswer()
    {
        int playerAnswer;
        if (int.TryParse(rareAnswerInput.text, out playerAnswer))
        {
            if (playerAnswer == rareCorrectAnswer)
            {
                Debug.Log("Correct rare animal answer!");
                // Reward the player or handle success logic
            }
            else
            {
                Debug.Log("Incorrect rare animal answer.");
                // Penalize the player or handle failure logic
            }

            // Hide the rare animal question panel after submission
            rareQuestionPanel.SetActive(false);

            // Unfreeze the game (resume time)
            Time.timeScale = 1;

            // Enable interactions with the main UI again
            mainUICanvasGroup.interactable = true;
            mainUICanvasGroup.blocksRaycasts = true;
        }
        else
        {
            Debug.Log("Invalid input! Please enter a number.");
        }
    }

    private void CalculateInitialTotal()
    {
        initialTotal = 0;
        foreach (Animal animal in FindObjectsOfType<Animal>())
        {
            initialTotal += animal.animalValue;
        }
        Debug.Log($"Initial animal total calculated: {initialTotal}");
    }

    public int GetInitialTotal()
    {
        return initialTotal;
    }

    public void ShowQuestion()
    {
        Debug.Log("Triggering Total Value Question UI");

        // Show the question panel
        questionPanel.SetActive(true);

        // Set the correct answer as the initial total value of animals
        correctAnswer = initialTotal;

        // Set the question text
        questionText.text = "Time's Up!!!\r\nHow many values are there\r\nbased on the animals\r\non the screen?";

        // Clear the input field
        answerInput.text = "";
    }

    private void SubmitAnswer()
    {
        int playerAnswer;
        if (int.TryParse(answerInput.text, out playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                Debug.Log("Correct Answer! You Win!");
                PlayerPrefs.SetString("GameResult", "Win");
            }
            else
            {
                Debug.Log("Incorrect Answer. You Lose!");
                PlayerPrefs.SetString("GameResult", "Defeat");
            }

            questionPanel.SetActive(false);
            StartCoroutine(ShowFeedbackAndLoadScene());
        }
        else
        {
            Debug.Log("Invalid input! Please enter a number.");
        }
    }

    private IEnumerator ShowFeedbackAndLoadScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Win Defeat");
    }

    public int CalculateTotal()
    {
        return initialTotal + persistentTotal;
    }

    public void AddToTotal(int value)
    {
        persistentTotal += value;
        Debug.Log($"Persistent total updated: {persistentTotal}");
    }

    private void Update()
    {
        Debug.Log($"Initial Total: {initialTotal}, Persistent Total (destroyed): {persistentTotal}");
    }
}
