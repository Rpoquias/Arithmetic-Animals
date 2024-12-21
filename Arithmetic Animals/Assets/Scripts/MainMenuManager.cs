using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
        public GameObject usernamePanel;
        public GameObject bG;

        private void Start()
        {
            // This check ensures that the username panel is hidden when we enter the Main Menu
            if (PlayerPrefs.GetInt("HideUsernamePanel", 0) == 1)
            {
                if (usernamePanel != null)
                {
                    usernamePanel.SetActive(false);  // Hide the panel
                }
                else
                {
                    Debug.LogError("UsernamePanel is not assigned!");
                }

                if (bG != null)
                {
                    bG.SetActive(false);
                }

                // Reset the flag after it's processed
                PlayerPrefs.SetInt("HideUsernamePanel", 0);
                PlayerPrefs.Save();  // Save the changes
            }
        }
    


    public void ConfirmQuit()
    {
        Application.Quit();
    }
}
