using UnityEngine;
using System.Collections;

public class TreeGrowth : MonoBehaviour
{
    private Transform parent;

    public float shakeDuration = 0.5f;  // Total time to shake the tree
    public float shakeMagnitude = 0.1f; // The magnitude of the shake
    public float elapsed = 0f;

    public SpriteRenderer topRenderer;    // Reference to the SpriteRenderer for the top part
    public SpriteRenderer bottomRenderer; // Reference to the SpriteRenderer for the bottom part
    public BoxCollider2D trunkCollider;   // Reference to the collider for the trunk (bottom part)


    public Sprite[] topGrowthStages;    // Sprites for the top part (canopy)
    public Sprite[] bottomGrowthStages; // Sprites for the bottom part (trunk)
    public float timeBetweenStages = 10f;  // Time in seconds between growth stages

    public Vector2[] trunkColliderSizes = new Vector2[4];   // Collider sizes for each stage
    public Vector2[] trunkColliderOffsets = new Vector2[4]; // Collider offsets for each stage (optional)
    public Vector2[] topPositionOffsets = new Vector2[4];   // Y-offsets for the top part for each growth stage
    private Vector3 originalPosition;      // Store the original position of the tree for shaking

    public GameObject birchLogPrefab;
    public GameObject chestnutLogPrefab;
    public GameObject mapleLogPrefab;

    private int currentStage = 0;  // The current stage of growth

    private int treeHealth = 10;
    private int logsToSpawn;

    private bool fullyGrown;

    public bool isBirch = false;
    public bool isChestnut = false;
    public bool isMaple = false;

    public bool isStarterTree = false;

    void Start()
    {
        // Find ProximityManager if it hasn't been assigned
        if (ProximityManager.Instance == null) {
            Debug.Log("Proximity manager not found.");
        }

        if (ProximityManager.Instance != null) {
            ProximityManager.Instance.objectsToManage.Add(gameObject);
        }

        if (isStarterTree) {
            currentStage = 3;
            UpdateSortingOrder();
            StartCoroutine(GrowTree());
            fullyGrown = true;
            parent = GameObject.Find("Trees").GetComponent<Transform>();
            this.transform.SetParent(parent);
            originalPosition = transform.localPosition;
            return;
        }

        fullyGrown = false;

        parent = GameObject.Find("Trees").GetComponent<Transform>();
        this.transform.SetParent(parent);

        //topRenderer.sortingLayerName = "Collision";

        originalPosition = transform.localPosition;

        // Start the growth process
        UpdateSortingOrder();
        StartCoroutine(GrowTree());
    }

    private IEnumerator GrowTree()
    {
        while (currentStage < topGrowthStages.Length && currentStage < bottomGrowthStages.Length) {
            // Set the sprites for both the top and bottom parts
            topRenderer.sprite = topGrowthStages[currentStage];
            bottomRenderer.sprite = bottomGrowthStages[currentStage];

            // Adjust the position of the top part based on the Y-offset for the current stage
            UpdateTopPosition();

            // Adjust the trunk's collider size and position for the current stage
            UpdateTrunkCollider();

            // Wait for the specified time between stages
            yield return new WaitForSeconds(timeBetweenStages);

            // Move to the next growth stage
            currentStage++;
            //if (currentStage > 0) {
            //    topRenderer.sortingLayerName = "Walk Behind";
            //}

            if (currentStage == 3) { fullyGrown = true; }
        }
    }

    private void UpdateSortingOrder()
    {
        // The closer the tree is to the bottom of the screen (lower Y value), the higher the sorting order
        bottomRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        topRenderer.sortingOrder = bottomRenderer.sortingOrder - 1; // Top should be rendered behind the trunk
    }

    private void UpdateTopPosition()
    {
        if (currentStage < topPositionOffsets.Length) {
            // Set the top part's local position relative to the bottom part using the offset
            topRenderer.transform.localPosition = topPositionOffsets[currentStage];
        }
    }

    private void UpdateTrunkCollider()
    {
        if (currentStage < trunkColliderSizes.Length) {
            // Update the size of the BoxCollider2D for the trunk (bottom part)
            trunkCollider.size = trunkColliderSizes[currentStage];

            // Optionally, update the offset of the BoxCollider2D (in case the trunk grows vertically)
            if (currentStage < trunkColliderOffsets.Length) {
                trunkCollider.offset = trunkColliderOffsets[currentStage];
            }
        }
    }

    [ContextMenu("Shake")]
    // Method to trigger the shake effect when the tree is hit
    public void OnHit()
    {
        TakeDamage(1);
        if (treeHealth <= 0) {
            SpawnInLogs();
        } else {
            StartCoroutine(ShakeTree());
        }
    }

    // Method to handle tree damage
    void TakeDamage(int damage)
    {
        treeHealth -= damage;
    }

    // Coroutine to shake the tree
    private IEnumerator ShakeTree()
    {
        elapsed = 0f;
        while (elapsed < shakeDuration) {
            // Generate random offset for the shake
            float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
            float offsetY = Random.Range(-shakeMagnitude, shakeMagnitude);

            // Apply the offset to the tree's position
            transform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            // Increase elapsed time
            elapsed += Time.deltaTime;

            // Wait for the next frame before continuing the shake
            yield return null;
        }

        // After the shake, reset the tree to its original position
        transform.localPosition = originalPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object hitting the tree is the tree axe
        if (collision.CompareTag("TreeAxe"))  // Ensure the axe has the tag "Axe"
        {
            if (fullyGrown) {
                // Call OnHit when the tree axe hits the tree
                OnHit();
            }
        }
    }

    private void SpawnInLogs()
    {
        // Spawn logs based on the tree type
        for (int i = 0; i < RandomNumberGenerator(); i++) {
            if (isBirch) {
                Instantiate(birchLogPrefab, transform.position, Quaternion.identity);
            } else if (isChestnut) {
                Instantiate(chestnutLogPrefab, transform.position, Quaternion.identity);
            } else {
                Instantiate(mapleLogPrefab, transform.position, Quaternion.identity);
            }
        }

        // Get the index of the current object in the list
        int index = ProximityManager.Instance.objectsToManage.IndexOf(gameObject);

        // If the object is found in the list, remove it by index
        if (index >= 0) {
            ProximityManager.Instance.objectsToManage.RemoveAt(index);
        }

        // Destroy the current GameObject
        Destroy(gameObject);
    }


    private int RandomNumberGenerator()
    {
        logsToSpawn = Random.Range(8, 13);
        return logsToSpawn;
    }
}
