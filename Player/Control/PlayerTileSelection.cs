using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTileSelection : MonoBehaviour
{
    public List<SeedData> allSeedData;

    public InventoryManager inventoryManager;
    public Tilemap tilemap;               // Reference to the Tilemap
    public Transform player;              // Reference to the player
    public GameObject selectionBox;       // Selection box to highlight tiles
    public float selectionRange = 2f;     // Range within which the player can select a tile

    public TileBase grassTile;            // Grass tile to revert to
    public TileBase dirtTile;             // Dirt tile to replace grass
    public TileBase wetTile;              // Wet tile to replace dirt after watering
    public GameObject seedPrefab;         // Prefab to instantiate when planting seeds
    public float revertTime = 5f;         // Time after which dirt reverts to grass
    public LayerMask obstacleLayer;       // LayerMask to define obstacles like stones, bushes, trees

    private Camera mainCamera;

    // Dictionary to store tile positions and their revert times
    private Dictionary<Vector3Int, float> tileTimers = new Dictionary<Vector3Int, float>();

    void Start()
    {
        mainCamera = Camera.main;
        selectionBox.SetActive(false);    // Hide the selection box initially
    }

    void Update()
    {
        Item selectedItem = inventoryManager.GetSelectedItem(false);

        Vector3Int tilePos = GetTileUnderMouse();

        // If a valid tile position and within range, show the selection box
        if (tilePos != Vector3Int.zero && IsTileWithinRange(tilePos)) {
            selectionBox.SetActive(true);
            selectionBox.transform.position = tilemap.GetCellCenterWorld(tilePos);
        } else {
            selectionBox.SetActive(false); // Hide the selection box if out of range or invalid
        }

        // If no item is selected, return
        if (selectedItem == null) return;

        // Handle tile click based on the selected tool
        if (Input.GetMouseButtonDown(0)) {
            if (selectedItem.name == "Hoe") {
                OnTileHoe(tilePos);  // Hoe the tile
            } else if (selectedItem.name == "WaterCan") {
                OnTileWater(tilePos);  // Water the tile
            } else if (selectedItem.name.Contains("Seed")) {
                OnTileSeed(tilePos);  // Plant the seed
            }
        }

        // Update tile timers for reverting tiles back to grass
        UpdateTileTimers();
    }

    Vector3Int GetTileUnderMouse()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // Ignore z-axis for 2D tilemap

        return tilemap.WorldToCell(mouseWorldPos);
    }

    bool IsTileWithinRange(Vector3Int tilePos)
    {
        Vector3Int playerTilePos = tilemap.WorldToCell(player.position);
        float distance = Vector3Int.Distance(playerTilePos, tilePos);

        return distance <= selectionRange;
    }

    // Change the tile to dirt if it's grass
    void OnTileHoe(Vector3Int tilePos)
    {
        if (!IsTileWithinRange(tilePos)) return;

        TileBase tile = tilemap.GetTile(tilePos);

        // Check if the tile is grass and no obstacles are on the tile
        if (tile != null && tile == grassTile && !IsObstacleOnTile(tilePos)) {
            tilemap.SetTile(tilePos, dirtTile);  // Change the tile to dirt
            tileTimers[tilePos] = revertTime;    // Start the revert timer for this tile
        }
    }

    // Change the tile to wet dirt if it's already dirt
    void OnTileWater(Vector3Int tilePos)
    {
        if (!IsTileWithinRange(tilePos)) return;

        TileBase tile = tilemap.GetTile(tilePos);

        // Check if the tile is dirt and no obstacles are on the tile
        if (tile != null && tile == dirtTile) {
            tilemap.SetTile(tilePos, wetTile);  // Change the tile to wet dirt
            // You can optionally manage timers for the wet tile if needed.
        }
    }

    // Plant a seed if the tile is dirt or wet dirt and remove the timer
    void OnTileSeed(Vector3Int tilePos)
    {
        if (!IsTileWithinRange(tilePos)) return;

        TileBase tile = tilemap.GetTile(tilePos);

        // Check if the tile is dirt or wet dirt and no obstacles are on the tile
        if (tile != null && (tile == dirtTile || tile == wetTile) && !IsObstacleOnTile(tilePos)) {
            // Get the seed type from the selected item
            Item selectedItem = inventoryManager.GetSelectedItem(false);

            // Check if a seed bag is selected
            if (selectedItem != null && selectedItem.name.Contains("Seed")) {
                string seedType = selectedItem.name;  // Assuming the item name matches the seed type
                //Debug.Log("Selected seed type: " + seedType);

                // Find the SeedData associated with the selected seed
                SeedData seedData = FindSeedData(seedType);

                if (seedData != null) {
                    //Debug.Log("Planting: " + seedData.seedName);
                    GetComponent<PlayerController>().UpdateAnimators("SeedThrow", false);

                    // Plant the seed by instantiating the seed prefab at the tile's world position
                    Vector3 worldPos = tilemap.GetCellCenterWorld(tilePos);
                    GameObject seed = Instantiate(seedPrefab, worldPos, Quaternion.identity);

                    // Initialize the seed with the specific type and seed data
                    Seed seedComponent = seed.GetComponent<Seed>();
                    if (seedComponent != null) {
                        seedComponent.InitializeSeed(seedType, seedData);
                    } else {
                        Debug.LogError("Seed prefab is missing the Seed component.");
                    }

                    // Remove the tile from the revert timer dictionary so it doesn't revert to grass
                    if (tileTimers.ContainsKey(tilePos)) {
                        tileTimers.Remove(tilePos);  // Stop the tile from reverting
                        //Debug.Log("Removed tile from revert timer.");
                    }
                } else {
                    Debug.LogError("SeedData not found for: " + seedType);
                }
            } else {
                Debug.LogError("No seed bag selected.");
            }
        } else {
            Debug.Log("Cannot plant seed: Tile is not dirt or wet dirt or an obstacle is present.");
        }
    }

    public SeedData FindSeedData(string seedType)
    {
        foreach (SeedData seedData in allSeedData) {
            if (seedData.seedName == seedType) {
                return seedData;
            }
        }
        Debug.LogWarning("SeedData not found for: " + seedType);
        return null;
    }

    // Method to check if there are any obstacles (stones, bushes, trees) on a tile
    bool IsObstacleOnTile(Vector3Int tilePos)
    {
        // Convert the tile position to world position
        Vector3 worldPos = tilemap.GetCellCenterWorld(tilePos);

        // Check if there's any collider on the obstacle layer within a small radius around the tile
        Collider2D obstacle = Physics2D.OverlapCircle(worldPos, 0.1f, obstacleLayer);
        return obstacle != null;  // Return true if there is an obstacle, false if not
    }

    // Update the timers for all tiles and revert them back to grass if time expires
    void UpdateTileTimers()
    {
        // Create a list of tiles to revert
        List<Vector3Int> tilesToRevert = new List<Vector3Int>();

        // Iterate over a copy of the dictionary's keys to avoid modifying it while iterating
        foreach (var tilePos in new List<Vector3Int>(tileTimers.Keys)) {
            float remainingTime = tileTimers[tilePos] - Time.deltaTime;

            if (remainingTime <= 0) {
                tilesToRevert.Add(tilePos);  // Mark tile for reverting if time has run out
            } else {
                tileTimers[tilePos] = remainingTime;  // Update remaining time for the tile
            }
        }

        // After enumeration, safely modify the dictionary by reverting tiles and removing them
        foreach (var tilePos in tilesToRevert) {
            tilemap.SetTile(tilePos, grassTile);  // Revert the tile back to grass
            tileTimers.Remove(tilePos);           // Remove the tile from the timer dictionary
        }
    }
}
