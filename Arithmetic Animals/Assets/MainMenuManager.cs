using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject levelsPanel; // Reference to the levels panel GameObject
    public Button[] levelButtons; // Array of level buttons

    void Start()
    {
        // Check if the levels panel should open
        int openLevelsPanel = PlayerPrefs.GetInt("OpenLevelsPanel", 0);

        if (openLevelsPanel == 1)
        {
            levelsPanel.SetActive(true); // Show the levels panel
            FocusNextLevel();           // Focus the next level button
            PlayerPrefs.SetInt("OpenLevelsPanel", 0); // Reset the flag
            PlayerPrefs.Save();
        }
    }

    public void FocusNextLevel()
    {
        int focusLevel = PlayerPrefs.GetInt("FocusLevel", 1); // Get the level to focus on
        if (focusLevel > 0 && focusLevel <= levelButtons.Length)
        {
            levelButtons[focusLevel - 1].Select(); // Highlight the next level button
        }
    }
}
