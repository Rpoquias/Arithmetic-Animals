using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject levelsPanel; // Reference to the levels panel GameObject
    public Button[] levelButtons; // Array of level buttons
    public GameObject quitConfirmationDialog;
    public GameObject mainPanel; // Reference to the main menu panel

    private const string LastPanelKey = "LastVisitedPanel";

    void Start()
    {
        if (levelsPanel == null || mainPanel == null)
        {
            Debug.LogError("Panel references are missing. Please assign them in the inspector.");
            return;
        }

        string lastPanel = PlayerPrefs.GetString(LastPanelKey, "MainPanel");
        OpenPanel(lastPanel);

        int openLevelsPanel = PlayerPrefs.GetInt("OpenLevelsPanel", 0);
        if (openLevelsPanel == 1)
        {
            levelsPanel.SetActive(true);
            FocusNextLevel();
            PlayerPrefs.SetInt("OpenLevelsPanel", 0); // Reset the flag
        }
    }

    public void OpenPanel(string panelName)
    {
        // Hide all panels
        levelsPanel.SetActive(false);
        mainPanel.SetActive(false);

        // Show the selected panel
        if (panelName == "LevelsPanel") levelsPanel.SetActive(true);
        if (panelName == "MainPanel") mainPanel.SetActive(true);

        PlayerPrefs.SetString(LastPanelKey, panelName);
        PlayerPrefs.Save();
    }
    public void FocusNextLevel()
    {
        int focusLevel = PlayerPrefs.GetInt("FocusLevel", 1); // Get the level to focus on
        if (focusLevel > 0 && focusLevel <= levelButtons.Length)
        {
            levelButtons[focusLevel - 1].Select(); // Highlight the next level button
        }
    }

    public void LoadNextLevel()
    {
        // Get the current level number from PlayerPrefs or set to default (e.g., level 1)
        int currentLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        int nextLevel = currentLevel + 1;

        // Unlock the next level
        PlayerPrefs.SetInt("UnlockedLevel", nextLevel);
        PlayerPrefs.SetInt("FocusLevel", nextLevel); // Save the next level to focus
        PlayerPrefs.Save();

        // Check if the next level exists in Build Settings
        string nextLevelName = "Level " + nextLevel;
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




    public void QuitGame()
    {
        quitConfirmationDialog.SetActive(true);
    }

    public void ConfirmQuit()
    {
        Application.Quit();
        Debug.Log("Game has exited."); // Debug for Unity Editor
    }

    public void CancelQuit()
    {
        quitConfirmationDialog.SetActive(false);
    }
}
