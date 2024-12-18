using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject defeatPanel;

    void Start()
    {
        // Retrieve the result from PlayerPrefs
        string gameResult = PlayerPrefs.GetString("GameResult", "Defeat"); // Default to "Defeat"

        // Show the appropriate panel
        if (gameResult == "Win")
        {
            ShowWin();
           
        }
        else
        {
            ShowDefeat();
           
        }
    }

    public void ShowWin()
    {
        winPanel.SetActive(true);
        defeatPanel.SetActive(false);
    }

    public void ShowDefeat()
    {
        winPanel.SetActive(false);
        defeatPanel.SetActive(true);
    }

    // Button Functions

    // Go to the main menu
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Replace "MainMenu" with the actual scene name for your main menu
    }

    // Restart the current level
    public void RestartLevel()
    {

        string currentLevelName = PlayerPrefs.GetString("CurrentLevel", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(currentLevelName); // Reload the saved level
    }

    // Go to the next level
    // Go to the next level
    public void LoadNextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        int nextLevel = currentLevel + 1;

        // Unlock the next level
        PlayerPrefs.SetInt("UnlockedLevel", nextLevel);
        PlayerPrefs.SetInt("FocusLevel", nextLevel); // Save the next level to focus
        PlayerPrefs.Save();

        // Generate the next level scene name dynamically
        string nextLevelName = "Level " + nextLevel;

        // Check if the next level exists in Build Settings
        if (Application.CanStreamedLevelBeLoaded(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.LogWarning($"Level '{nextLevelName}' does not exist. Returning to Main Menu.");
            SceneManager.LoadScene("MainMenu"); // Fallback to the main menu
        }
    }

}

