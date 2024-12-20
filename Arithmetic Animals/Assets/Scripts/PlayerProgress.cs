using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerProgress
{
    public int starsEarned;
    public int wavesCompleted;
    public int levelsUnlocked;

    public PlayerProgress(int stars = 0, int waves = 0, int levels = 1)
    {
        starsEarned = stars;
        wavesCompleted = waves;
        levelsUnlocked = levels;
    }

    public void AddStars(int stars)
    {
        starsEarned += stars;
    }

    public void CompleteWave()
    {
        wavesCompleted++;
    }

    public void UnlockLevel(int level)
    {
        if (level > levelsUnlocked)
        {
            levelsUnlocked = level;
        }
    }
}
