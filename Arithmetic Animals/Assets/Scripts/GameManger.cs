using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton Pattern
    public static GameManager Instance { get; private set; }
    public PlayerProgress playerProgress; // Reference to PlayerProgress
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
        // Ensure countdownManager and UI elements are assigned
        if (countdownManager == null)
        {
            countdownManager = FindObjectOfType<CountDownManager>();
        }

        if (questionPanel == null)
        {
            questionPanel = GameObject.Find("QuestionPanel"); // Adjust to the name of the object in your scene
        }
            

        // Initialize Animal Total
        CalculateInitialTotal();

        // Add Listeners for Input Submission
        submitButton.onClick.AddListener(SubmitAnswer);
        rareSubmitButton.onClick.AddListener(SubmitRareAnimalAnswer);

        // Hide Panels Initially
        questionPanel.SetActive(false);
        rareQuestionPanel.SetActive(false);
        winPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }


    // ----------------------
    // Rare Animal Mechanics
    // ----------------------
    public void ShowRareAnimalQuestion(string question, int correctAnswer)
    {
        Debug.Log("Found Rare Animal! Solve the Problem!");
        PauseGame();

        rareQuestionPanel.SetActive(true);
        rareQuestionText.text = question;
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
                // Unlock the next level (adjust for index offset)
                int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
                int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); // Default to level 1 being unlocked (index 2)
                ShowWinPanel();

                if (currentLevelIndex + 1 > unlockedLevel)
                {
                    PlayerPrefs.SetInt("UnlockedLevel", currentLevelIndex + 1);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                Debug.Log("Incorrect Answer. You Lose!");
                ShowDefeatPanel();
            }

            questionPanel.SetActive(false);
        }
        else
        {
            // Handle this outside the SubmitAnswer if needed, as you stated you already have panels for this.
            Debug.Log("Invalid input! Please enter a number.");
        }
    }



    // ----------------------
    // Win and Defeat Mechanics
    // ----------------------
    private void ShowWinPanel()
    {
        winPanel.SetActive(true);
        SetupWinButtons();

        // Refresh level buttons after winning
        LevelMenu levelMenu = FindObjectOfType<LevelMenu>();
        if (levelMenu != null)
        {
            levelMenu.RefreshLevelButtons();
        }
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
