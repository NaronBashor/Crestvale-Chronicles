using System.Collections;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public GameObject seedCollectiblePrefab;

    public string seedType;  // The type of seed (e.g., "Beetroot", "Cabbage", etc.)
    public SpriteRenderer spriteRenderer;  // Single SpriteRenderer for the plant

    public Transform player; // Reference to the player for Y-position comparison
    public SeedData seedData;  // Reference to the SeedData for this seed

    private int currentStage = 0;  // Current growth stage
    private bool isGrowing = false;  // To ensure the coroutine runs only once
    public bool fullyGrown = false;  // Whether the plant is fully grown

    private bool isPlayerNearby = false;  // To track if the player is near the plant

    private void Start()
    {
        // Start the seed's growth cycle if seed data has been initialized and it's not growing yet
        if (seedData != null && !isGrowing) {
            StartCoroutine(GrowSeed());
        }
        player = GameObject.FindWithTag("Player").transform;  // Get reference to player by tag
    }

    private void Update()
    {
        // Continuously update the sorting order as the player walks around
        UpdateSortingOrder();

        // Check if the player is near the plant and if it's fully grown
        if (fullyGrown && isPlayerNearby && Input.GetMouseButtonDown(0)) {
            Harvest();
        }
    }

    // Coroutine to handle the growth stages of the plant
    private IEnumerator GrowSeed()
    {
        isGrowing = true;  // Set the flag to true so the coroutine doesn't start again

        // Ensure that the number of growth stages is valid for the sprite
        while (currentStage < seedData.growthStages.Length) {
            // Update sprite for the current growth stage
            spriteRenderer.sprite = seedData.growthStages[currentStage];

            // Increment stage at the end of the cycle, before checking for the next cycle
            currentStage++;

            // Wait for the time between stages before moving to the next one
            if (currentStage < seedData.growthStages.Length) {
                yield return new WaitForSeconds(seedData.timeBetweenStages);
            }
        }

        isGrowing = false;  // Reset the flag after fully grown
        fullyGrown = true;  // Mark as fully grown
    }

    // Dynamically adjust the sorting order based on the Y position of the player and the plant
    private void UpdateSortingOrder()
    {
        if (player == null) return;

        // Compare the Y position of the player and the plant's sprite
        float plantY = spriteRenderer.transform.position.y;
        float playerY = player.position.y;

        // If the player is above the plant, render the plant behind the player
        if ((playerY - 0.85f) > plantY) {
            spriteRenderer.sortingOrder = 20;  // Behind the player
        }
        // If the player is below the plant, render the plant in front of the player
        else {
            spriteRenderer.sortingOrder = -20;  // In front of the player
        }
    }

    private void Harvest()
    {
        Debug.Log($"Harvested: {seedType}!");
        int yieldAmount = Random.Range(1, (seedData.yield + 1));

        for (int i = 0; i < yieldAmount; i++) {
            // Instantiate the seed collectible prefab (fruit/vegetable after harvest)
            GameObject collectible = Instantiate(seedCollectiblePrefab, transform.position, Quaternion.identity);

            // Get the correct item and sprite for the harvested seed
            SeedCollectiblePrefab collectibleScript = collectible.GetComponent<SeedCollectiblePrefab>();

            // Find the corresponding Item object from the inventory manager (or any relevant data source)
            Item harvestedItem = FindHarvestedItem();  // You need to implement this based on your item system

            // Initialize the collectible with the seed's data and the corresponding item
            collectibleScript.InitializeCollectible(seedData, harvestedItem);
        }

        // After harvesting, you can remove the plant
        Destroy(gameObject);  // Remove the plant after harvesting
    }


    // Example method to retrieve the harvested item (this depends on your item system)
    private Item FindHarvestedItem()
    {
        // You can either map the seed type to a specific item or retrieve it from a data manager.
        // For example:
        InventoryManager inventoryManager = GameObject.Find("Inventory Manager").GetComponent<InventoryManager>();
        return inventoryManager.FindCropToSpawn(seedType);  // Assuming your InventoryManager has such a method
    }

    // Initialize the seed with its specific type and data
    public void InitializeSeed(string type, SeedData data)
    {
        seedType = type;
        seedData = data;
        currentStage = 0;  // Reset the growth stage

        // Initialize the first growth stage
        spriteRenderer.sprite = seedData.growthStages[currentStage];

        // Start the growth coroutine
        StartCoroutine(GrowSeed());
    }

    // Detect when the player enters the trigger area (i.e., gets close to the plant)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Feet")) {
            isPlayerNearby = true;  // Mark the player as nearby
        }
    }

    // Detect when the player exits the trigger area (i.e., moves away from the plant)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Feet")) {
            isPlayerNearby = false;  // Mark the player as not nearby
        }
    }
}
