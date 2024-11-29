using System.Collections;
using UnityEngine;

public class SeedCollectiblePrefab : MonoBehaviour
{
    public SeedData seedData;  // Reference to SeedData to retrieve the correct sprite and item details
    private SpriteRenderer spriteRenderer;  // SpriteRenderer for displaying the collected food

    private Rigidbody2D rb;
    private Transform player;
    private InventoryManager invManager;

    public float bounceForce = 2f;  // The force applied to make the collectible bounce
    public float bounceTorque = 10f; // The torque applied for rotation (optional)

    public Item seedItem;  // Reference to the Item that will be added to the inventory

    // Movement towards player
    public float playerDetectRange = 5f; // Range within which the logs detect and move towards the player
    public float moveSpeed = 5f;         // Speed at which the logs move towards the player
    public float collectDistance = 0.5f; // Distance from the player at which the logs will be collected/destroyed

    private bool canCollect = false;

    private void Start()
    {
        // Ensure spriteRenderer is initialized
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            Debug.LogError("SpriteRenderer is missing from the SeedCollectiblePrefab!");
            return;  // Exit if the spriteRenderer is not found
        }

        invManager = GameObject.Find("Inventory Manager").GetComponent<InventoryManager>();
        if (invManager == null) {
            Debug.LogError("Inventory Manager not found!");
            return;  // Exit if the Inventory Manager is not found
        }

        player = GameObject.FindWithTag("Player").transform;
        if (player == null) {
            Debug.LogError("Player not found!");
            return;  // Exit if the player is not found
        }

        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Apply a random bounce and wait before allowing collection
        StartCoroutine(ApplyBounce());
    }

    // Method to apply random bounce effect
    private IEnumerator ApplyBounce()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
        rb.AddForce(randomDirection * bounceForce, ForceMode2D.Impulse);

        float randomTorque = Random.Range(-bounceTorque, bounceTorque);
        rb.AddTorque(randomTorque, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);  // Wait 1 second before allowing collection
        canCollect = true;
    }

    private void FixedUpdate()
    {
        if (!canCollect) { return; }

        // Check if the player is within range
        if (Vector2.Distance(transform.position, player.position) <= playerDetectRange) {
            MoveTowardsPlayer();  // Move towards the player
        }

        // Check if close enough to collect
        if (Vector2.Distance(transform.position, player.position) <= collectDistance) {
            CollectItem();
        }
    }

    // Move towards the player
    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    // Collect the item, add to inventory, and destroy the object
    private void CollectItem()
    {
        // Add the item to the player's inventory
        invManager.AddItem(seedItem);

        // Destroy this object
        Destroy(gameObject);
    }

    // Initialize the collectible with the correct sprite and item data
    public void InitializeCollectible(SeedData seedData, Item seedItem)
    {
        // Ensure both seedData and seedItem are passed correctly
        if (seedData == null || seedItem == null) {
            Debug.LogError("SeedData or SeedItem is missing!");
            return;
        }

        this.seedData = seedData;
        this.seedItem = seedItem;

        spriteRenderer = GetComponent<SpriteRenderer>();
        // Ensure spriteRenderer is valid before assigning sprite
        if (spriteRenderer != null) {
            spriteRenderer.sprite = seedData.harvestIcon;  // Use the harvested item icon as the sprite
        } else {
            Debug.LogError("SpriteRenderer is not assigned on the SeedCollectiblePrefab!");
        }
    }
}
