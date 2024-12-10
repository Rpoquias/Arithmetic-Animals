using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    // General Properties
    public int animalValue = 0;
    public MovementType movementType;
    public MovementDirection movementDirection = MovementDirection.Right;
    public float movementSpeed = 2f;

    // Component References
    private Camera _cam;
    private SpriteRenderer sr;
    private Animator animator;
    private BoxCollider2D animalCollider;

    // Movement Buffers and Timers
    private float _buffer = 0.5f;
    private float randomMovementTimer;
    private Vector3 randomDirection;

    #region Unity Methods

    private void Awake()
    {
        InitializeComponents();
        InitializeMovementDirection();
    }

    private void Start()
    {
        AdjustAnimalSizeBasedOnScreen();
        InitializeAnimator();

        // Log the animal's value for debugging
        Debug.Log($"Animal {gameObject.name} initialized with value: {animalValue}");
    }

    private void Update()
    {
        HandleMovement();
        ClampPositionWithinCameraView();
        UpdateSortingOrder();
    }

    #endregion

    #region Initialization

    private void InitializeComponents()
    {
        _cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animalCollider = GetComponent<BoxCollider2D>();

        if (sr == null) Debug.LogError("SpriteRenderer not found!");
        if (animalCollider == null)
        {
            animalCollider = gameObject.AddComponent<BoxCollider2D>();
            Debug.Log("BoxCollider2D added dynamically!");
        }
    }

    private void InitializeMovementDirection()
    {
        if (transform.position.x < 0)
            movementDirection = MovementDirection.Right;
        else
            movementDirection = MovementDirection.Left;
    }

    private void InitializeAnimator()
    {
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
            return;
        }
    }

    #endregion

    #region Movement Handlers

    private void HandleMovement()
    {
        switch (movementType)
        {
            case MovementType.Static:
                PlayIdleAnimation();
                break;
            case MovementType.Random:
                PerformRandomMovement();
                break;
            case MovementType.MovePastScreen:
                MovePastScreen();
                break;
        }
    }

    private void PlayIdleAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.enabled = false; // Completely disable for static animals
        }
    }

    private void PerformRandomMovement()
    {
        if (randomMovementTimer <= 0)
        {
            randomDirection = new Vector3(Random.Range(-1f, 1f), 0, 0).normalized;
            randomMovementTimer = Random.Range(2f, 5f);
        }

        randomMovementTimer -= Time.deltaTime;
        transform.Translate(randomDirection * movementSpeed * Time.deltaTime);
        UpdateSpriteDirection(randomDirection.x);
        UpdateWalkingAnimation(randomDirection.x != 0);
    }

    private void MovePastScreen()
    {
        Vector3 direction = (movementDirection == MovementDirection.Right) ? Vector3.right : Vector3.left;
        transform.Translate(direction * movementSpeed * Time.deltaTime);
        UpdateSpriteDirection(movementDirection == MovementDirection.Left ? -1 : 1);
        UpdateWalkingAnimation(true);
        EnsureColliderIsTrigger();
    }

    private void UpdateSpriteDirection(float direction)
    {
        if (sr != null) sr.flipX = (direction < 0);
    }

    private void UpdateWalkingAnimation(bool isWalking)
    {
        if (animator != null) animator.SetBool("isWalking", isWalking);
    }

    private void EnsureColliderIsTrigger()
    {
        if (animalCollider != null) animalCollider.isTrigger = true;
    }

    #endregion

    #region Utility Methods

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
        if (movementType == MovementType.MovePastScreen) return;

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
        if (sr != null) sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }

    #endregion

    #region Enums

    public enum MovementType
    {
        Static,
        Random,
        MovePastScreen
    }

    public enum MovementDirection
    {
        Left,
        Right
    }

    #endregion
}
