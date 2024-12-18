using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{

    public Button[] buttons;
    [SerializeField] private AsyncLoad asyncLoad; // Reference to the AsyncLoad script
    public GameObject levelButtons;
    public GameObject mainMenuCanvas; // Reference to the main menu canvas
    public Sprite completedIcon;

    void Start()
    {
        PlayerPrefs.SetInt("UnlockedLevel", 1); // Unlock the first level
        PlayerPrefs.Save();
        RefreshLevelButtons();
    }



    private void Awake()
    {
        if (levelButtons == null || asyncLoad == null || mainMenuCanvas == null)
        {
            Debug.LogError("References are missing. Please assign them in the inspector.");
            return;
        }
      
        ButtonsToArray();
        RefreshLevelButtons(); // Update button states
    }


    public void CompleteLevel(int levelId)
    {
        PlayerPrefs.SetInt("Level" + levelId + "Completed", 1);
        PlayerPrefs.Save();
        RefreshLevelButtons();
    }

    private void RefreshLevelButtons()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); // Default to level 1

        for (int i = 0; i < buttons.Length; i++)
        {
            // Unlock the button based on the unlocked level
            buttons[i].interactable = i < unlockedLevel;

            // If the level is completed, change its icon to 'completedIcon'
            bool levelCompleted = PlayerPrefs.GetInt("Level" + (i + 1) + "Completed", 0) == 1;
            if (levelCompleted)
            {
                var image = buttons[i].transform.GetChild(0).GetComponent<Image>();
                if (image != null) image.sprite = completedIcon;
            }
        }
    }
    public void BackToMainMenu()
    {
        gameObject.SetActive(false); // Hide the level menu
        mainMenuCanvas.SetActive(true); // Show the main menu
        PlayerPrefs.SetString("LastVisitedPanel", "MainPanel");
        PlayerPrefs.Save();
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "Level " + levelId; // Generate level name dynamically
        asyncLoad.LoadLevelBtn(levelName);

        string currentLevelName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("CurrentLevel", currentLevelName);
        PlayerPrefs.Save();
        Debug.Log("Triggering Total Value Question UI");

    }

    void ButtonsToArray()
    {
        int childCount = levelButtons.transform.childCount;
        Debug.Log($"Level Buttons Count: {childCount}"); // Debug the number of children
        buttons = new Button[childCount];
        for (int i = 0; i < childCount; i++)
        {
            var button = levelButtons.transform.GetChild(i).GetComponent<Button>();
            if (button != null)
            {
                buttons[i] = button;
                Debug.Log($"Button {i + 1} added: {button.name}");
            }
            else
            {
                Debug.LogError($"Child {i + 1} of levelButtons is not a Button!");
            }
        }
    }

}
