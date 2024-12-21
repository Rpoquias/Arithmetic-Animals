using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton Pattern
    public static GameManager Instance { get; private set; }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }

    // Managers
    public CountDownManager countdownManager;

    // Animal Total Calculation
    private int initialTotal = 0;
    private int persistentTotal = 0;

    // Question UI (Time's Up Panel)
    [Header("Question Panel")]
    public TMP_Text questionText;
    public TMP_InputField answerInput;
    public Button submitButton;
    public GameObject questionPanel;
    private int correctAnswer;

    // Rare Animal UI
    [Header("Rare Animal Panel")]
    public TMP_Text rareQuestionText;
    public TMP_InputField rareAnswerInput;
    public Button rareSubmitButton;
    public GameObject rareQuestionPanel;
    private int rareCorrectAnswer;

    // Win and Defeat Panels
    [Header("Win and Defeat Panels")]
    public GameObject winPanel;
    public GameObject defeatPanel;
    public Button homeButtonWin;
    public Button restartButtonWin;
    public Button nextLevelButtonWin;
    public Button homeButtonDefeat;
    public Button restartButtonDefeat;

    private void Start()
    {
        // Set up button listeners
        homeButtonWin.onClick.AddListener(() => GoToMainMenu());
        homeButtonDefeat.onClick.AddListener(() => GoToMainMenu());

        // Ensure countdownManager is assigned
        if (countdownManager == null)
        {
            countdownManager = FindObjectOfType<CountDownManager>();
            if (countdownManager == null)
            {
                Debug.LogWarning("CountdownManager not found in the scene.");
            }
        }

        // Initialize animal total
        CalculateInitialTotal();

        // Add listeners for submitting answers
        submitButton.onClick.AddListener(SubmitAnswer);
        rareSubmitButton.onClick.AddListener(SubmitRareAnimalAnswer);

        // Hide panels initially
        questionPanel.SetActive(false);
        rareQuestionPanel.SetActive(false);
        winPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }

    // ----------------------
    // Rare Animal Mechanics
    // ----------------------
    public void ShowRareAnimalQuestion(int correctAnswer)
    {
        PauseGame();

        rareQuestionPanel.SetActive(true);
        rareQuestionText.text = "Rare Animal Found!\r\nSolve The Arithmetic Problem!\r\n 1 + 2 + 1 = ?";
        rareCorrectAnswer = correctAnswer;
        rareAnswerInput.text = "";
    }

    private void SubmitRareAnimalAnswer()
    {
        if (int.TryParse(rareAnswerInput.text, out int playerAnswer))
        {
            if (playerAnswer == rareCorrectAnswer)
            {
                Debug.Log("Correct rare animal answer!");
                // Reward the player
            }
            else
            {
                Debug.Log("Incorrect rare animal answer.");
                // Penalize the player
            }
            rareQuestionPanel.SetActive(false);
            ResumeGame();
        }
        else
        {
            Debug.Log("Invalid input! Please enter a number.");
        }
    }

    // ----------------------
    // Game Question Mechanics
    // ----------------------
    public void ShowQuestion()
    {
        questionPanel.SetActive(true);
        correctAnswer = initialTotal;
        questionText.text = "Time's Up!!!\r\nHow many values are there\r\nbased on the animals\r\non the screen?";
        answerInput.text = "";
    }

    private void SubmitAnswer()
    {
        if (int.TryParse(answerInput.text, out int playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                // Unlock the next level
                int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
                int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
                ShowWinPanel();

                if (currentLevelIndex + 1 > unlockedLevel)
                {
                    PlayerPrefs.SetInt("UnlockedLevel", currentLevelIndex + 1);
                    PlayerPrefs.Save();
                }

                // Reward 3 stars for correct answer
                PlayerProgress playerProgress = FindObjectOfType<PlayerProgress>();
                if (playerProgress != null)
                {
                    playerProgress.AddStars(3);  // Add 3 stars to the player's progress
                    PlayerDataManager.Instance.SaveGame();  // Save the game after updating stars
                }

            }
            else
            {
                ShowDefeatPanel(); // Show defeat panel if the answer is incorrect
            }

            questionPanel.SetActive(false); // Hide the question panel after the answer
        }
        else
        {
            Debug.Log("Invalid input! Please enter a number."); // Handle invalid input
        }
    }





    // ----------------------
    // Go to Main Menu
    // ----------------------
    private void GoToMainMenu()
    {
        PlayerPrefs.SetInt("HideUsernamePanel", 1);
        PlayerPrefs.Save(); // Save changes
        SceneManager.LoadScene("Main Menu");
    }

    // ----------------------
    // Win and Defeat Mechanics
    // ----------------------
    private void ShowWinPanel()
    {
        winPanel.SetActive(true);
        SetupWinButtons();
    }

    private void ShowDefeatPanel()
    {
        defeatPanel.SetActive(true);
        SetupDefeatButtons();
    }

    private void SetupWinButtons()
    {
        homeButtonWin.onClick.AddListener(() => LoadScene("Main Menu"));
        restartButtonWin.onClick.AddListener(() => RestartLevel());
        nextLevelButtonWin.onClick.AddListener(() => LoadNextLevel());
    }

    private void SetupDefeatButtons()
    {
        homeButtonDefeat.onClick.AddListener(() => LoadScene("Main Menu"));
        restartButtonDefeat.onClick.AddListener(() => RestartLevel());
    }

    // ----------------------
    // Level and Scene Management
    // ----------------------
    private void LoadScene(string sceneName)
    {
        Time.timeScale = 1; // Ensure game time is running
        SceneManager.LoadScene(sceneName);
    }

    private void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextLevel()
    {
        Time.timeScale = 1;
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentLevelIndex + 1 > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevelIndex + 1);
            PlayerPrefs.Save();
        }
        SceneManager.LoadScene(currentLevelIndex + 1);
    }

    // ----------------------
    // Game Management Utilities
    // ----------------------

    public void AddToPersistentTotal(int value)
    {
        persistentTotal += value;
        Debug.Log($"Persistent total updated: {persistentTotal}");
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

    private void PauseGame()
    {
        if (countdownManager != null)
            countdownManager.PauseCountdown();
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        if (countdownManager != null)
            countdownManager.ResumeCountdown();
        Time.timeScale = 1;
    }
}
