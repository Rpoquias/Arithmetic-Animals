using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public int animalValue = 0;

    private Camera _cam; // Main camera reference
    private float _buffer = 0.5f; // Buffer to prevent animals from being cut off at screen edges

    private SpriteRenderer sr; // Animal's SpriteRenderer
    private Animator animator;
    private BoxCollider2D animalCollider; // Animal's BoxCollider2D

    public MovementType movementType; // Define the movement behavior
    public float movementSpeed = 2f; // Speed of the animal
    public bool isWalking; // Determines if the animal is walking

    private float randomMovementTimer; // Timer for random movement
    private Vector3 randomDirection;


    void Awake()
    {
        _cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        animalCollider = GetComponent<BoxCollider2D>(); // Get the collider component

        if (sr == null)
        {
            Debug.LogError("SpriteRenderer not found on the Animal object!");
        }

        if (animalCollider == null)
        {
            animalCollider = gameObject.AddComponent<BoxCollider2D>(); // Dynamically add collider if not found
            Debug.Log("BoxCollider2D added to animal!");
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        if (_cam == null)
        {
            Debug.LogError("Main camera not found! Ensure your scene has a Camera tagged as 'MainCamera'.");
            return;
        }

        AdjustAnimalSizeBasedOnScreen();
        InitializeMovement();

            // Assign a default direction if not already set
            if (movementDirection == MovementDirection.Left || movementDirection == MovementDirection.Right)
            {
                Debug.Log($"Initial movementDirection: {movementDirection}");
            }
            else
            {
                movementDirection = MovementDirection.Right; // Default to Right
                Debug.Log("Defaulting movementDirection to Right");
            }
        
    }

    void Update()
    {
        switch (movementType)
        {
            case MovementType.Static:
                PlayIdleAnimation(); // Ensure the static animals stay idle
                break;

            case MovementType.Random:
                PerformRandomMovement();
                break;

            case MovementType.MovePastScreen:
                MovePastScreen();
                break;
        }

        ClampPositionWithinCameraView();
        UpdateSortingOrder();

    }

    private void PlayIdleAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", false); // Ensure "isWalking" is false
            animator.enabled = false;            // Completely disable the Animator for static animals
        }
    }


    private void PerformRandomMovement()
    {
        if (randomMovementTimer <= 0)
        {
            randomDirection = new Vector3(Random.Range(-1f, 1f), 0, 0).normalized;
            randomMovementTimer = Random.Range(2f, 5f); // Random movement duration
        }

        randomMovementTimer -= Time.deltaTime;

        // Flip the sprite based on direction
        if (randomDirection.x > 0 && sr.flipX) // Animal moving right
        {
            sr.flipX = false;  // Flip sprite to the right
        }
        else if (randomDirection.x < 0 && !sr.flipX) // Animal moving left
        {
            sr.flipX = true;  // Flip sprite to the left
        }

        // Move in the random horizontal direction
        transform.Translate(randomDirection * movementSpeed * Time.deltaTime);

        // Check if the animal is actually moving
        if (randomDirection.x != 0)
        {
            // Set walking animation state only if the animal is moving
            if (animator != null)
            {
                animator.SetBool("isWalking", true);
            }
        }
        else
        {
            // Set idle animation state when the animal is not moving
            if (animator != null)
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    void InitializeMovement()
    {
        if (movementType == MovementType.MovePastScreen)
        {
            movementDirection = (Random.value > 0.5f) ? MovementDirection.Right : MovementDirection.Left;
        }
    }

    private void MovePastScreen()
    {
        // Determine the direction vector based on the toggle
        Vector3 direction = (movementDirection == MovementDirection.Right) ? Vector3.right : Vector3.left;

        // Flip the sprite based on the direction
        if (sr != null)
        {
            sr.flipX = (movementDirection == MovementDirection.Left); // Flip the sprite when moving left
        }

        // Move in the selected direction
        transform.Translate(direction * movementSpeed * Time.deltaTime);

        // Ensure the animal's collider is set to trigger when moving past the screen
        if (animalCollider != null)
        {
            animalCollider.isTrigger = true;
        }

        // Set walking animation state
        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }

        // Do not destroy the object here; destruction is handled by destruction zone colliders
    }



    public void SpawnAnimalOnGround(GameObject animalPrefab)
    {
        var (screenMin, screenMax) = GetCameraBounds();
        float spawnBuffer = 1f; // Spawn just outside the screen

        // Determine Y position based on the background collider height
        Vector3 spawnPosition = new Vector3(
            Random.value > 0.5f ? screenMax.x + spawnBuffer : screenMin.x - spawnBuffer, // Left or right of the screen
            transform.position.y, // Use the Y position of the background (ground)
            0
        );

        Instantiate(animalPrefab, spawnPosition, Quaternion.identity);
    }



    private void AdjustAnimalSizeBasedOnScreen()
    {
        if (sr == null) return;

        float cameraHeight = _cam.orthographicSize * 2;
        float cameraWidth = cameraHeight * _cam.aspect;

        float spriteWorldHeight = sr.sprite.rect.height / sr.sprite.pixelsPerUnit;
        float spriteWorldWidth = sr.sprite.rect.width / sr.sprite.pixelsPerUnit;

        float heightScale = cameraHeight / 5f / spriteWorldHeight;
        float widthScale = cameraWidth / 5f / spriteWorldWidth;

        float uniformScale = Mathf.Min(heightScale, widthScale);

        transform.localScale = new Vector3(uniformScale, uniformScale, 1f);
    }

    private void ClampPositionWithinCameraView()
    {
        if (movementType == MovementType.MovePastScreen)
            return; // Skip clamping for MovePastScreen behavior

        Vector3 animalPosition = transform.position;

        var (screenMin, screenMax) = GetCameraBounds();

        animalPosition.x = Mathf.Clamp(animalPosition.x, screenMin.x + _buffer, screenMax.x - _buffer);
        animalPosition.y = Mathf.Clamp(animalPosition.y, screenMin.y + _buffer, screenMax.y - _buffer);

        transform.position = animalPosition;
    }

    private (Vector3 min, Vector3 max) GetCameraBounds()
    {
        float cameraHeight = _cam.orthographicSize * 2;
        float cameraWidth = cameraHeight * _cam.aspect;

        Vector3 cameraCenter = _cam.transform.position;

        Vector3 min = new Vector3(
            cameraCenter.x - cameraWidth / 2,
            cameraCenter.y - cameraHeight / 2,
            0);

        Vector3 max = new Vector3(
            cameraCenter.x + cameraWidth / 2,
            cameraCenter.y + cameraHeight / 2,
            0);

        return (min, max);
    }

    private void UpdateSortingOrder()
    {
        if (sr != null)
        {
            sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }
    }

   

    public enum MovementType
    {
        Static,        // Animal remains stationary
        Random,        // Animal moves randomly
        MovePastScreen // Animal moves continuously in one direction
    }

    public enum MovementDirection
    {
        Left,
        Right
    }

    public MovementDirection movementDirection = MovementDirection.Right;
}

