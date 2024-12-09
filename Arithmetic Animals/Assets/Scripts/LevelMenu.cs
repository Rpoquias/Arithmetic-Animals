using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private AsyncLoad asyncLoad; // Reference to the AsyncLoad script

    public void OpenLevel(int levelId)
    {
        string levelName = "Level " + levelId;
        asyncLoad.loadLevelBtn(levelName); // Delegate the loading to the AsyncLoad script
    }
}
