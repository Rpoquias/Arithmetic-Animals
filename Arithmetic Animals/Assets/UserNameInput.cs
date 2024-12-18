using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInputManager : MonoBehaviour
{
    [SerializeField] private InputField usernameInputField;
    [SerializeField] private Button confirmButton;

    void Start()
    {
        confirmButton.onClick.AddListener(ConfirmUsername);
    }

    private void ConfirmUsername()
    {
        string username = usernameInputField.text.Trim();

        if (string.IsNullOrEmpty(username))
        {
            Debug.LogWarning("Username cannot be empty!");
            return;
        }

        PlayerPrefs.SetString("Username", username); // Save the username
        PlayerPrefs.Save();
        Debug.Log($"Username saved: {username}");

        // Navigate to Main Menu
        SceneManager.LoadScene("MainMenu");
    }
}
