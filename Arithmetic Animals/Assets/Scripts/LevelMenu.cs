    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class LevelMenu : MonoBehaviour
    {

        public Button[] buttons;
        [SerializeField] private AsyncLoad asyncLoad; // Reference to the AsyncLoad script
        public GameObject levelButtons;
        public GameObject mainMenuCanvas; // Reference to the main menu canvas

        private void Awake()
        {
            ButtonsToArray();
            RefreshLevelButtons(); // Update button states
        }
    void Start()
    {
        // Only set UnlockedLevel if it's not already set (to avoid overwriting)
        if (!PlayerPrefs.HasKey("UnlockedLevel"))
        {
            PlayerPrefs.SetInt("UnlockedLevel", 1); // Set level 1 as unlocked (index 1)
            PlayerPrefs.Save();
        }
        RefreshLevelButtons(); // Update button states
    }
    public void CompleteLevel(int levelId)
    {
        PlayerPrefs.SetInt("Level" + levelId + "Completed", 1);
        PlayerPrefs.Save();
        RefreshLevelButtons(); // Update button states to reflect the completed level
    }


    public void RefreshLevelButtons()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); // Default to Level 1 being unlocked (index 1)

        for (int i = 0; i < buttons.Length; i++)
        {
            // Buttons are based on index 1, so we check if the level index + 1 is <= unlocked level
            buttons[i].interactable = (i + 1 <= unlockedLevel); // i + 1 to match the index in PlayerPrefs
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
