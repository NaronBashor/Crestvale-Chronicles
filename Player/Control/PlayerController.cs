using DirePixel.Animation;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public GameObject inventoryPanel;

    public Animator toolAnimator;
    public Animator effectsAnimator;
    public ToolSwapper toolSwapper;

    public FishingLine fishingLine;
    public InventoryManager invManager;
    LineRenderer lineRenderer;

    public Camera cam;

    public int toolIndex;
    public float waitForFishTime = 5f;

    // Variables for player movement
    public float walkSpeed = 5f;  // Walking speed
    public float runSpeed = 8f;   // Running speed
    private float moveSpeed;      // Current movement speed

    private Rigidbody2D rb;
    public Vector2 movement;
    private Vector2 lastMoveDirection;

    // Animator for animations
    public Animator animator;

    // Boolean to check if chopping
    private bool isPerformingAction;

    private bool isWaitingForFish = false;

    public GameObject bobberGameObject;

    [SerializeField] private List<Transform> roomPositions = new List<Transform>();
    [SerializeField] public Vector3 buildingEntrancePosition;
    [SerializeField] public Vector3 camPosition;
    public bool isIndoors;
    public bool isHomeFarm;
    public bool canEnterRoom;
    public bool canExitRoom;
    public int roomIndex;

    private bool isCurrentlySmithing = false;

    private bool toeTapTimer;
    private int frameCounter;
    private float timer;

    private bool invPanelOpen = false;

    // Define camera boundaries (for clamping camera when indoors)
    public Vector2 minCameraPos;  // Minimum X and Y position for the camera
    public Vector2 maxCameraPos;  // Maximum X and Y position for the camera

    // Start is called before the first frame update
    void Start()
    {
        inventoryPanel.SetActive(invPanelOpen);
        toolAnimator.gameObject.SetActive(false);
        effectsAnimator.gameObject.SetActive(false);
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = walkSpeed;
        lastMoveDirection = Vector2.down;
        isPerformingAction = false;
        isWaitingForFish = false;
        lineRenderer.enabled = false;
        toeTapTimer = false;
        isCurrentlySmithing = false;
        bobberGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Only allow movement if the player is not chopping
        if (GameStateManager.Instance.IsPlaying()) {
            Move();
        }

        UpdateAnimations();

        if (toeTapTimer) {
            frameCounter++;
            if (frameCounter >= (120 * 10)) {
                animator.SetBool("toeTap", true);
                timer += Time.deltaTime * 5;  // Increment by 5 frames worth of time

                // Reset the frame counter
                frameCounter = 0;
            }
        } else {
            animator.SetBool("toeTap", false);
        }

        // Handle camera follow
        if (!isIndoors) {
            cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);

            if (isHomeFarm) {
                cam.transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
                Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
                -10f
            );
            }
        }

        // Handle chopping action
        if (Input.GetMouseButtonDown(0) && GameStateManager.Instance.IsPlaying() && !IsPointerOverUI()) {
            StartAction();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (canEnterRoom) {
                isIndoors = !isIndoors;
                EnterRoom(roomIndex);
            }
            if (canExitRoom) {
                isIndoors = !isIndoors;
                ExitRoom(buildingEntrancePosition);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (GameStateManager.Instance.IsInventoryOpen()) {
                HandleInventoryPanel(false);
            } else {
                HandleInventoryPanel(true);
            }
        }

        if (Input.anyKeyDown) {
            if (isCurrentlySmithing) {
                animator.SetBool("isSmithing", false);
                toolAnimator.SetBool("isSmithing", false);
                EndAction();
                isCurrentlySmithing = false;
            }
            if (isWaitingForFish && isPerformingAction) {
                StartCoroutine(ResetFishingState());
            }
            if (toeTapTimer) {
                toeTapTimer = false;
            }
        }
    }

    private void HandleInventoryPanel(bool closeOpen)
    {
        inventoryPanel.SetActive(closeOpen);
        if (closeOpen) {
            GameStateManager.Instance.SetGameState(GameState.Inventory);
        } else {
            GameStateManager.Instance.SetGameState(GameState.Playing);
        }
    }

    // Method to check if the mouse is over a UI element
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void EnterRoom(int index)
    {
        transform.position = roomPositions[roomIndex].transform.position;
        cam.transform.position = camPosition;
    }

    public void ExitRoom(Vector2 doorPos)
    {
        transform.position = buildingEntrancePosition;
    }

    void Move()
    {
        // Check if the player is holding the Shift key to run
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            moveSpeed = runSpeed;  // Running
        } else {
            moveSpeed = walkSpeed;  // Walking
        }

        // Capture input for both axes
        float inputX = Input.GetAxisRaw("Horizontal");   // Left/Right input (-1, 0, 1)
        float inputY = Input.GetAxisRaw("Vertical");     // Up/Down input (-1, 0, 1)

        // Prioritize horizontal movement if both axes are pressed
        if (Mathf.Abs(inputX) > 0.1f) {
            // Horizontal input takes precedence over vertical input
            movement.x = inputX;
            movement.y = 0;
        } else if (Mathf.Abs(inputY) > 0.1f) {
            // No horizontal input, allow vertical movement
            movement.x = 0;
            movement.y = inputY;
        } else {
            // No input
            movement = Vector2.zero;
        }

        // Keep track of the last movement direction
        if (movement.x != 0 || movement.y != 0) {
            lastMoveDirection = movement;
        }

        // Normalize the movement vector to ensure consistent speed
        movement = movement.normalized;
    }

    private void FixedUpdate()
    {
        // Only apply velocity if not chopping
        if (!isPerformingAction && !invPanelOpen) {
            rb.velocity = movement * moveSpeed;
        } else {
            rb.velocity = Vector2.zero;  // Stop movement during chopping
        }
    }

    void UpdateAnimations()
    {
        if (isPerformingAction) return;  // Skip animation updates when performing an action

        if (movement.magnitude > 0.01f && !invPanelOpen) {
            animator.SetBool("isWalking", moveSpeed == walkSpeed);
            animator.SetBool("isRunning", moveSpeed == runSpeed);

            // Update based on movement direction
            animator.SetFloat("xInput", movement.x);
            animator.SetFloat("yInput", movement.y);
        } else {
            // Idle state
            toeTapTimer = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);

            // Set idle direction to the last direction the player was facing
            animator.SetFloat("IdleX", lastMoveDirection.x);
            animator.SetFloat("IdleY", lastMoveDirection.y);
        }
    }


    void StartAction()
    {
        Item selectedItem = invManager.GetSelectedItem(false);
        if (selectedItem != null) {
            // Get the direction from player to mouse
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - transform.position).normalized;

            // Determine the primary direction (up, down, left, right)
            float absX = Mathf.Abs(direction.x);
            float absY = Mathf.Abs(direction.y);

            if (absX > absY) {
                // Horizontal direction (left or right)
                if (direction.x > 0) {
                    lastMoveDirection = Vector2.right;  // Right
                } else {
                    lastMoveDirection = Vector2.left;   // Left
                }
            } else {
                // Vertical direction (up or down)
                if (direction.y > 0) {
                    lastMoveDirection = Vector2.up;     // Up
                } else {
                    lastMoveDirection = Vector2.down;   // Down
                }
            }

            // Update the player's animator to face the correct direction
            UpdatePlayerFacingDirection(lastMoveDirection);

            // Now trigger the appropriate animation based on the selected item
            switch (selectedItem.name) {
                case "Club":
                    UpdateAnimators("Club", true);
                    GameStateManager.Instance.SetGameState(GameState.Action);
                    break;
                case "FishRod":
                    UpdateAnimators("Fish", false);
                    GameStateManager.Instance.SetGameState(GameState.Action);
                    break;
                case "Hoe":
                    UpdateAnimators("Hoe", true);
                    GameStateManager.Instance.SetGameState(GameState.Action);
                    break;
                case "Net":
                    UpdateAnimators("Net", false);
                    GameStateManager.Instance.SetGameState(GameState.Action);
                    break;
                case "Pickaxe":
                    UpdateAnimators("Mine", true);
                    GameStateManager.Instance.SetGameState(GameState.Action);
                    break;
                case "Scythe":
                    UpdateAnimators("Scythe", false);
                    GameStateManager.Instance.SetGameState(GameState.Action);
                    break;
                case "Sword":
                    UpdateAnimators("Sword", true);
                    GameStateManager.Instance.SetGameState(GameState.Action);
                    break;
                case "Tree Axe":
                    UpdateAnimators("Tree", true);
                    GameStateManager.Instance.SetGameState(GameState.Action);
                    break;
                case "WaterCan":
                    UpdateAnimators("Water", false);
                    GameStateManager.Instance.SetGameState(GameState.Action);
                    break;
                case "Weapon Axe":
                    UpdateAnimators("Axe", true);
                    GameStateManager.Instance.SetGameState(GameState.Action);
                    break;
            }
        }
    }

    void UpdatePlayerFacingDirection(Vector2 direction)
    {
        // Update the animator parameters to face the correct direction
        animator.SetFloat("xInput", direction.x);
        animator.SetFloat("yInput", direction.y);
    }



    public void UpdateAnimators(string tool, bool effects)
    {
        isPerformingAction = true;
        rb.velocity = Vector2.zero;

        toolAnimator.gameObject.SetActive(true);
        effectsAnimator.gameObject.SetActive(true);

        // Set the animation trigger for the tool
        animator.SetTrigger(tool);
        toolAnimator.SetTrigger(tool);

        if (effects) {
            effectsAnimator.SetTrigger(tool);
        }

        // Set direction values based on the last move direction (calculated from mouse position)
        toolAnimator.SetFloat("directionX", lastMoveDirection.x);
        toolAnimator.SetFloat("directionY", lastMoveDirection.y);
        effectsAnimator.SetFloat("directionX", lastMoveDirection.x);
        effectsAnimator.SetFloat("directionY", lastMoveDirection.y);
    }


    // Call this method once chopping animation ends (from Animation Event)
    public void EndAction()
    {
        isPerformingAction = false;  // Allow movement again
        GameStateManager.Instance.SetGameState(GameState.Playing);
    }

    public void ChangeFishingBool()
    {
        bobberGameObject.SetActive(true);
        fishingLine.fish = true;
        lineRenderer.enabled = true;
        isWaitingForFish = !isWaitingForFish;
        animator.SetBool("isWaitingForFish", isWaitingForFish);
        toolAnimator.SetBool("isWaitingForFish", isWaitingForFish);
    }

    public IEnumerator SmithingBool()
    {
        yield return new WaitForSeconds(0.5f);
        isCurrentlySmithing = true;
    }

    public void CallThisToResetFishingFromOtherScripts()
    {
        StartCoroutine(ResetFishingState());
    }

    private IEnumerator ResetFishingState()
    {
        bobberGameObject.SetActive(false);
        fishingLine.fish = false;
        lineRenderer.enabled = false;
        lineRenderer.endWidth = 0;
        lineRenderer.startWidth = 0;
        isWaitingForFish = false;
        animator.SetBool("isWaitingForFish", false);
        toolAnimator.SetBool("isWaitingForFish", false);
        FishingLine fishLine = GetComponent<FishingLine>();
        GameStateManager.Instance.SetGameState(GameState.Playing);

        yield return new WaitForSeconds(0.5f);
        isPerformingAction = false;
    }
}
