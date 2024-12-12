using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 1f;

    public void StartTransition(string sceneName)
    {
        StartCoroutine(LoadSceneWithTransition(sceneName));
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        // Trigger the fade-out animation
        transitionAnimator.SetTrigger("FadeOut");

        // Wait for the animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Load the next scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
