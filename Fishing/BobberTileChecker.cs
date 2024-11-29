using UnityEngine;
using UnityEngine.Tilemaps;

public class BobberTileChecker : MonoBehaviour
{
    public PlayerController playerController;
    public Tilemap tilemap1;  // Reference to the first Tilemap
    public Tilemap tilemap2;  // Reference to the second Tilemap
    public string targetSpriteName = "spring forest_554";  // The name of the sprite you're checking for

    void Update()
    {
        if (tilemap1 == null || tilemap2 == null) {
            //Debug.LogError("One or both Tilemaps are not assigned!");
            return;
        }

        // Check if the bobber is over the target tile in either tilemap
        if (IsOverTargetTile(tilemap1) || IsOverTargetTile(tilemap2)) {
            //Debug.Log("Bobber is over the tile with the sprite: " + targetSpriteName);
        } else {
            playerController.CallThisToResetFishingFromOtherScripts();
        }
    }

    private bool IsOverTargetTile(Tilemap tilemap)
    {
        // Get the position of the Bobber in world space
        Vector3 worldPosition = transform.position;

        // Force the z-position to 0 to align with the tilemap's plane
        worldPosition.z = 0;

        // Convert the world position to cell position (tilemap grid position)
        Vector3Int tilePosition = tilemap.WorldToCell(worldPosition);
        //Debug.Log("Checking Tile Position on " + tilemap.name + ": " + tilePosition);

        // Get the tile at the calculated position
        TileBase tile = tilemap.GetTile(tilePosition);

        if (tile != null) {
            //Debug.Log("Tile found at position on " + tilemap.name + ": " + tilePosition);

            // Use Tilemap.GetSprite() to directly get the sprite at the tile position
            Sprite tileSprite = tilemap.GetSprite(tilePosition);

            if (tileSprite != null) {
                //Debug.Log("Tile Sprite on " + tilemap.name + ": " + tileSprite.name);
                if (tileSprite.name == targetSpriteName) {
                    return true;
                }
            } else {
                //Debug.Log("Tile on " + tilemap.name + " has no sprite.");
            }
        } else {
            //Debug.Log("No tile found at position on " + tilemap.name + ": " + tilePosition);
        }

        return false;
    }
}
