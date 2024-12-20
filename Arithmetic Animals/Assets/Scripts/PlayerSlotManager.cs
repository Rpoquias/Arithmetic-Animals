using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSlotsManager : MonoBehaviour
{
    public PlayerSlot[] playerSlots = new PlayerSlot[5]; // Array for 5 player slots
    public TMP_Text[] slotTexts; // Text elements for displaying slot information
    public Button[] slotButtons; // Buttons corresponding to each slot
    public TMP_InputField usernameInputField; // Input field for new names
    public TMP_Text mainMenuUsernameText;     // Main menu username display

    private int currentSlotIndex = -1; // Currently selected slot
    public Color defaultColor = Color.white; // Default button color
    public Color selectedColor = Color.green; // Highlighted button color
    public LeaderboardManager leaderboardManager;
    void Start()
    {
        // Initialize the default slot if empty
        if (string.IsNullOrEmpty(playerSlots[0].playerName))
        {
            currentSlotIndex = 0;
            playerSlots[0].playerName = "Default";
            playerSlots[0].stars = 0;
            playerSlots[0].waves = 0;
        }

        HighlightButton(0); // Highlight the first slot
        UpdateSlotTexts();
    }

    public void SelectSlot(int slotIndex)
    {
        currentSlotIndex = slotIndex;
        usernameInputField.text = playerSlots[slotIndex].playerName;
        HighlightButton(slotIndex); // Update button highlight
    }

    public void SaveNameToSlot()
    {
        if (currentSlotIndex < 0) return;

        string newName = usernameInputField.text.Trim();
        if (!string.IsNullOrEmpty(newName))
        {
            playerSlots[currentSlotIndex].playerName = newName;
            UpdateSlotTexts();

            // Update main menu username if it's the first slot
            if (currentSlotIndex == 0)
            {
                mainMenuUsernameText.text = $"{newName}";
            }
        }
        else
        {
            Debug.LogWarning("Name cannot be empty!");
        }

        if (leaderboardManager != null)
        {
            leaderboardManager.UpdateLeaderboards();
        }

    }

    public void UpdateSlotTexts()
    {
        for (int i = 0; i < playerSlots.Length; i++)
        {
            slotTexts[i].text = string.IsNullOrEmpty(playerSlots[i].playerName)
                ? "Empty Slot"
                : $"{playerSlots[i].playerName} (Stars: {playerSlots[i].stars}, Waves: {playerSlots[i].waves})";
        }
    }

    private void HighlightButton(int slotIndex)
    {
        // Reset all button colors to default
        for (int i = 0; i < slotButtons.Length; i++)
        {
            var colors = slotButtons[i].colors;
            colors.normalColor = defaultColor;
            slotButtons[i].colors = colors;
        }

        // Highlight the selected button
        var selectedColors = slotButtons[slotIndex].colors;
        selectedColors.normalColor = selectedColor;
        slotButtons[slotIndex].colors = selectedColors;
    }

    [System.Serializable]
    public class PlayerSlot
    {
        public string playerName;
        public int stars;
        public int waves;
    }


}
