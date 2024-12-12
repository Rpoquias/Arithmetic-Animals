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
        string currentLevelName = SceneManager.GetActiveScene().name; // Get the current level name
        SceneManager.LoadScene(currentLevelName); // Reload the current scene
    }

    // Go to the next level
    // Go to the next level
    public void LoadNextLevel()
    {
        // Unlock the next level
        int currentLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
        PlayerPrefs.SetInt("FocusLevel", currentLevel + 1); // Save the next level to focus
        PlayerPrefs.SetInt("OpenLevelsPanel", 1); // Flag to open the levels panel
        PlayerPrefs.Save();

        // Load the Main Menu Scene
        SceneManager.LoadScene("MainMenu"); // Replace with the actual scene name
    }


}

