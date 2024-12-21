using System.Collections;
using TMPro;
using UnityEngine;

public class CountDownManager : MonoBehaviour
{
    public bool isPreGameCountdownActive = false;
    public TMP_Text preGameCountdownText;  // Reference to the "Get Ready" text
    public TMP_Text countdownText;  // Reference to the gameplay countdown text
    public GameObject preGameUI;  // Parent object that contains both the "Get Ready" text and gameplay countdown text

    public float preGameCountdownTime = 3f;  // Time for the "Get Ready" countdown (3 seconds)
    public float gameCountdownTime = 10f;  // Time for the gameplay countdown

    private float currentTime;  // Current time for countdown
    private bool countdownActive = false;  // Flag to track if countdown is active
    private bool isPreGameCountdown = true;  // Flag to track if it's pre-game countdown
    private bool isGameCountdown = false;  // Flag to track if it's gameplay countdown

    // Reference to the GameManager (which handles the Total Value Question)
    public GameManager gameManager;

    void Start()
    {
        // Ensure that Time.timeScale is set to 1 when starting
        Time.timeScale = 1;
        // Start the "Get Ready" countdown
        StartPreGameCountdown();
    }


    // Update is called once per frame

    void Update()
    {
        if (countdownActive)
        {
     

            if (countdownActive)
            {
                // Decrease the current time using unscaled delta time
                currentTime -= Time.unscaledDeltaTime;

                if (currentTime <= 0)
                {
                    currentTime = 0;
                    countdownActive = false;

                    // Continue with the rest of the countdown logic
                }
            }

            if (currentTime <= 0)  // Check if countdown has finished
            {
                currentTime = 0;
                countdownActive = false;  // Stop the countdown

                if (isPreGameCountdown)  // If it's the "Get Ready" countdown
                {
                    isPreGameCountdown = false;  // Switch to gameplay countdown

                    // Hide the PreGame UI
                    preGameUI.SetActive(false);

                    // Start the gameplay countdown
                    StartGameCountdown();

                    Time.timeScale = 1;  // Ensure time is running normally after pre-game
                }
                else if (isGameCountdown)  // If it's the gameplay countdown
                {
                    TriggerTotalValueQuestion();  // Trigger the Total Value Question UI
                }
            }

            // Update the countdown text based on the current countdown type
            if (isPreGameCountdown)
            {
                UpdatePreGameCountdownText();
            }
            else if (isGameCountdown)
            {
                UpdateGameCountdownText();
            }
        }
    }
    // Updates the pregame countdown display based on the current time
    private void UpdatePreGameCountdownText()
    {
        int seconds = Mathf.FloorToInt(currentTime);
        preGameCountdownText.text = string.Format("Get Ready: {0:00}(s)", seconds);
    }

    // Updates the gameplay countdown display based on the current time
    private void UpdateGameCountdownText()
    {
        int seconds = Mathf.FloorToInt(currentTime);
        countdownText.text = string.Format("{0:00}(s) remaining", seconds);  // Simple seconds countdown format
    }

    private void SetAnimalsMovement(bool isAllowed)
    {
        Animal[] allAnimals = FindObjectsOfType<Animal>();
        foreach (var animal in allAnimals)
        {
            animal.isMovementAllowed = isAllowed;
        }
    }

    // Public method to start the pre-game countdown
    public void StartPreGameCountdown()
    {
        // Debug log to track countdown start time
   

        preGameUI.SetActive(true);  // Show PreGame UI
        countdownText.gameObject.SetActive(true);  // Show countdown text
        currentTime = preGameCountdownTime;  // Set countdown time
        countdownActive = true;  // Begin countdown
        isPreGameCountdown = true;  // Mark this as the pregame countdown

        // Disable animal movement during countdown
        SetAnimalsMovement(false);
    }


    // Public method to start the gameplay countdown
    public void StartGameCountdown()
    {
        SetAnimalsMovement(true);
        countdownText.gameObject.SetActive(true);  // Activate gameplay countdown text
        currentTime = gameCountdownTime;  // Set countdown time to gameplay countdown time
        countdownActive = true;  // Begin gameplay countdown
        isGameCountdown = true;  // Mark this as the gameplay countdown
    }

    // Function to trigger the total value question (after the game countdown ends)
    private void TriggerTotalValueQuestion()
    {
        // Trigger the Total Value Question logic here
        // For example, enable the UI for the total value question and allow user input
        gameManager.ShowQuestion();
    }

    // Pause the countdown
    public void PauseCountdown()
    {
        countdownActive = false;  // Stops the countdown
    }

    // Resume the countdown
    public void ResumeCountdown()
    {
        countdownActive = true;  // Resumes the countdown
    }
}
