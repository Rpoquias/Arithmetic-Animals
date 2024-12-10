using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownManager : MonoBehaviour
{
    public TMP_Text countdownText;  // Reference to a TextMeshProUGUI component for the countdown text
    public float countdownTime = 10f;  // The duration of the countdown

    private float currentTime;
    private bool countdownActive = false;

    public TotalValueQuestion totalValueQuestion;  // Reference to the TotalValueQuestion script

    void Start()
    {
        // Initialize currentTime with countdownTime
        currentTime = countdownTime;
        countdownText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(currentTime / 60), Mathf.FloorToInt(currentTime % 60));
    }

    // Update is called once per frame
    void Update()
    {
        if (countdownActive)
        {
            currentTime -= Time.deltaTime;  // Decrease the time

            if (currentTime <= 0)  // Check if countdown has finished
            {
                currentTime = 0;
                countdownActive = false;  // Stop the countdown
                TriggerTotalValueQuestion();  // Trigger the Total Value Question
            }
        }

        // Update the countdown display in the format MM:SS
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Public method to start the countdown
    public void StartCountdown()
    {
        currentTime = countdownTime;  // Reset current time to countdownTime
        countdownActive = true;  // Set the countdown active
    }

    // Method to trigger the Total Value Question UI
    private void TriggerTotalValueQuestion()
    {
        if (totalValueQuestion != null)
        {
            totalValueQuestion.ShowQuestion();  // Show the question UI
        }
    }
}
