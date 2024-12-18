        using UnityEngine;
        using UnityEngine.SceneManagement;  // For loading scenes
        using UnityEngine.UI; // For UI components (Button)

        public class PauseMenu : MonoBehaviour
        {
            public GameObject pauseMenuPanel;  // Reference to the pause menu panel
            public Button resumeButton;        // Reference to the resume button
            public Button restartButton;       // Reference to the restart button
            public Button backToHomeButton;    // Reference to the back to home button
            public Button pauseButton;         // Reference to the pause button that triggers the pause menu
            public CountDownManager countdownManager;  // Reference to the CountDownManager
    public GameManager gameManager; // Reference to the GameManager

    private bool isPaused = false;     // To track if the game is paused

            void Start()
            {
    
                // Set up button listeners
                resumeButton.onClick.AddListener(ResumeGame);
                restartButton.onClick.AddListener(RestartGame);
                backToHomeButton.onClick.AddListener(BackToHome);
                pauseButton.onClick.AddListener(TogglePauseMenu);  // This will trigger the pause menu
            }

    public void PauseCountdown()
    {
        isPaused = true; // Add a flag if necessary
    }
    public void PauseGame()
    {
        if (gameManager != null && gameManager.rareQuestionPanel.activeSelf)
        {
            // Don't allow pausing while a rare question is active
            Debug.Log("Cannot pause while answering a rare question.");
            return;
        }

        Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);
        isPaused = true;

        if (countdownManager != null)
        {
            countdownManager.PauseCountdown();
        }
    }

    // Toggle the pause menu on and off
    public void TogglePauseMenu()
            {
                if (isPaused)
                {
                    ResumeGame();  // Resume the game if it's currently paused
                }
                else
                {
                    PauseGame();   // Pause the game if it's not already paused
                }
            }

            // Pauses the game by setting time scale to 0 and showing the pause menu
            public void PauseGame()
            {
                Time.timeScale = 0;  // Stop time (pause the game)
                pauseMenuPanel.SetActive(true);  // Show the pause menu
                isPaused = true;


                if (countdownManager != null)
                {
                    countdownManager.PauseCountdown();  // Pause the countdown as well
                }
            }

            // Resumes the game by setting time scale to 1 and hiding the pause menu
            public void ResumeGame()
            {
                Time.timeScale = 1;  // Resume time (unpause the game)
                pauseMenuPanel.SetActive(false);  // Hide the pause menu
                isPaused = false;

                if (countdownManager != null)
                {
                    countdownManager.ResumeCountdown();  // Resume the countdown if it was paused
                }
            }

            // Restart the current scene
            private void RestartGame()
            {
                Time.timeScale = 1;  // Ensure the game is running
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
            }

            // Load the main menu (back to home)
            private void BackToHome()
            {
                Time.timeScale = 1;  // Ensure the game is running
                SceneManager.LoadScene("Main Menu");  // Load the main menu scene (replace with your actual main menu scene name)
            }
        }
