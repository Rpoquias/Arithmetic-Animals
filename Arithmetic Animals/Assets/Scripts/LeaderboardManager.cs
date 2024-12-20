using UnityEngine;
using TMPro;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public PlayerSlotsManager playerSlotsManager; // Reference to PlayerSlotsManager
    public TMP_Text[] starsLeaderboardTexts; // Text elements for stars leaderboard
    public TMP_Text[] wavesLeaderboardTexts; // Text elements for waves leaderboard

    void Start()
    {
        UpdateLeaderboards(); // Initialize leaderboard on start
    }

    public void UpdateLeaderboards()
    {
        var playerSlots = playerSlotsManager.playerSlots;

        // Sort by stars for stars leaderboard
        var starsSorted = playerSlots
            .OrderByDescending(slot => slot.stars)
            .ThenBy(slot => slot.playerName)
            .ToArray();

        for (int i = 0; i < starsLeaderboardTexts.Length; i++)
        {
            if (i < starsSorted.Length && !string.IsNullOrEmpty(starsSorted[i].playerName))
            {
                starsLeaderboardTexts[i].text = $"{i + 1}. {starsSorted[i].playerName} - Stars: {starsSorted[i].stars}";
            }
            else
            {
                starsLeaderboardTexts[i].text = $"{i + 1}. Empty";
            }
        }

        // Sort by waves for waves leaderboard
        var wavesSorted = playerSlots
            .OrderByDescending(slot => slot.waves)
            .ThenBy(slot => slot.playerName)
            .ToArray();

        for (int i = 0; i < wavesLeaderboardTexts.Length; i++)
        {
            if (i < wavesSorted.Length && !string.IsNullOrEmpty(wavesSorted[i].playerName))
            {
                wavesLeaderboardTexts[i].text = $"{i + 1}. {wavesSorted[i].playerName} - Waves: {wavesSorted[i].waves}";
            }
            else
            {
                wavesLeaderboardTexts[i].text = $"{i + 1}. Empty";
            }
        }
    }


}
