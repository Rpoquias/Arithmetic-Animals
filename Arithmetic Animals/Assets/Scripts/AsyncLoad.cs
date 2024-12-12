    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class AsyncLoad : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Slider loadingSlider;

        public void LoadLevelBtn(string levelToLoad, bool showLoadingScreen = true)
        {
            // If showLoadingScreen is false, skip the loading screen setup
            if (showLoadingScreen)
            {
                ActivateAllChildren(loadingScreen);
                if (loadingSlider != null)
                {
                    loadingSlider.value = 0;
                }
                loadingScreen.SetActive(true); // Show the loading screen
            }
            else
            {
                loadingScreen.SetActive(false); // Hide loading screen if not needed
            }

            StartCoroutine(LoadLevelAsync(levelToLoad, showLoadingScreen));
        }

        private void ActivateAllChildren(GameObject parent)
        {
            if (!parent.activeSelf)
            {
                parent.SetActive(true); // Activate the parent first
            }

            foreach (Transform child in parent.transform)
            {
                if (!child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true); // Activate all child objects
                }
            }
        }

        IEnumerator LoadLevelAsync(string levelToLoad, bool showLoadingScreen)
        {
            // Start asynchronous loading
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

            while (!loadOperation.isDone)
            {
                if (showLoadingScreen && loadingSlider != null)
                {
                    // Update slider value during load
                    float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
                    loadingSlider.value = progressValue;
                }
                yield return null;
            }
        }
    }
