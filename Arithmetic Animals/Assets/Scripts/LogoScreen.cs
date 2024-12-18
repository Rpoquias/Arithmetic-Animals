using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoScreen : MonoBehaviour
{
    public float displayTime = 3f; // Time to display the logo
    public string mainMenuSceneName = "Main Menu"; // Main Menu scene name
    public SceneTransition sceneTransition; // Reference to SceneTransition script
    public GameObject logoScreen; // Reference to the logo screen game object
    public AsyncLoad asyncLoad; // Reference to AsyncLoad script

    void Start()
    {
        // Check if asyncLoad or sceneTransition is not assigned
        if (asyncLoad == null)
        {
            Debug.LogError("AsyncLoad is not assigned in the LogoScreen script.");
            return;
        }

        if (sceneTransition == null)
        {
            Debug.LogError("SceneTransition is not assigned in the LogoScreen script.");
            return;
        }

        // Start the logo transition coroutine
        StartCoroutine(LogoTransition());
    }

    IEnumerator LogoTransition()
    {
        // Display the logo for the specified time
        Debug.Log("Displaying logo for " + displayTime + " seconds.");
        yield return new WaitForSeconds(displayTime);

        // Hide the logo screen after display
        logoScreen.SetActive(false);

        // Start the scene transition (e.g., fading to black)
        Debug.Log("Starting scene transition to " + mainMenuSceneName);
        sceneTransition.StartTransition(mainMenuSceneName);

        // Wait for the transition duration before loading the scene
        yield return new WaitForSeconds(sceneTransition.transitionTime);

        // Start loading the main menu scene asynchronously
        Debug.Log("Triggering async load for the main menu scene.");
        asyncLoad.LoadLevelBtn(mainMenuSceneName, false); // Skip loading screen here
    }
}
