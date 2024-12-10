using UnityEngine;
/*
public class PlayerInteraction : MonoBehaviour
{
    public GameObject rareAnimalUI;  // The UI panel to show
    public UnityEngine.UI.Text rareAnimalQuestionText;  // The Text to show the question

    // Update is called once per frame
    void Update()
    {
        // Check for touch input (or mouse input for testing on a PC)
        if (Input.touchCount > 0)
        {
            // Get touch position
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;

            // Raycast from touch position to see what is hit
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPosition), Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("RareAnimal"))
            {
                // If we hit a rare animal, show the UI and the question
                ShowRareAnimalUI(hit.collider.GetComponent<RareAnimal>());
            }
        }
        else if (Input.GetMouseButtonDown(0)) // For mouse input (e.g., in the editor or PC)
        {
            Vector2 mousePosition = Input.mousePosition;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("RareAnimal"))
            {
                // If we hit a rare animal, show the UI and the question
                ShowRareAnimalUI(hit.collider.GetComponent<RareAnimal>());
            }
        }
    }

    // Show the rare animal UI and set the question text
    private void ShowRareAnimalUI(RareAnimal rareAnimal)
    {
        if (rareAnimalUI != null && rareAnimalQuestionText != null)
        {
            // Activate the UI
            rareAnimalUI.SetActive(true);

            // Set the question for the rare animal (you can set this up in the RareAnimal script)
            rareAnimalQuestionText.text = rareAnimal.GetQuestion();
        }
    }
}
*/