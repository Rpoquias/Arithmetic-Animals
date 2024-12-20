using UnityEngine;
using TMPro;

public class UsernameManager : MonoBehaviour
{
    public TMP_InputField usernameInputField; // Input field for username
    public TMP_Text mainMenuUsernameText;     // Username display in main menu


    private string username;

    private void Start()
    {
        // Limit the input field to 10 characters
        usernameInputField.characterLimit = 10;
    }

    public void SubmitUsername()
    {
        username = usernameInputField.text.Trim(); // Save username and remove extra spaces

        if (!string.IsNullOrEmpty(username))
        {
            mainMenuUsernameText.text = username; // Update main menu text
        }
        else
        {
            Debug.LogWarning("Username cannot be empty!");
        }
    }

 
}
