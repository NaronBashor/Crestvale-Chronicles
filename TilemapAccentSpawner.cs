using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapAccentSpawner : MonoBehaviour
{
    public Tilemap mainTilemap;     // The main tilemap that holds the "spring forest" tiles
    public Tilemap accentTilemap;   // The tilemap where accent tiles will be placed

    public TileBase[] accentTiles;  // List of accent tiles (assign in Inspector)

    // List of sprite names we're looking for
    private string[] targetSprites = { "spring forest_0", "spring forest_63", "spring forest_95", "spring forest_127", "spring forest_159" , "spring tree wall_10"};

    // Probability to spawn an accent tile
    [Range(0f, 1f)]
    public float spawnChance = 0.3f; // 30% chance to spawn an accent tile

    void Start()
    {
        // Iterate over the tilemap and find matching tiles
        FindAndPlaceAccentTiles();
    }

    void FindAndPlaceAccentTiles()
    {
        // Get the bounds of the tilemap
        BoundsInt bounds = mainTilemap.cellBounds;

        // Iterate over all the positions in the bounds of the tilemap
        for (int x = bounds.xMin; x < bounds.xMax; x++) {
            for (int y = bounds.yMin; y < bounds.yMax; y++) {
                Vector3Int tilePos = new Vector3Int(x, y, 0);

                // Get the tile at the current position
                TileBase currentTile = mainTilemap.GetTile(tilePos);

                if (currentTile != null) {
                    // Get the sprite of the current tile
                    Sprite tileSprite = mainTilemap.GetSprite(tilePos);

                    // Check if the tile's sprite name is in the target list
                    if (tileSprite != null && IsTargetSprite(tileSprite.name)) {
                        // Chance to spawn an accent tile
                        if (Random.value <= spawnChance) {
                            // Pick a random accent tile from the list
                            TileBase randomAccentTile = GetRandomAccentTile();

                            // Place the accent tile on the accent tilemap at the same position
                            if (randomAccentTile != null) {
                                accentTilemap.SetTile(tilePos, randomAccentTile);
                            }
                        }
                    }
                }
            }
        }
    }

    bool IsTargetSprite(string spriteName)
    {
        // Check if the sprite name is in the list of target sprite names
        foreach (string targetSpriteName in targetSprites) {
            if (spriteName == targetSpriteName) {
                return true;
            }
        }
        return false;
    }

    TileBase GetRandomAccentTile()
    {
        // Return a random tile from the accentTiles array
        if (accentTiles.Length > 0) {
            int randomIndex = Random.Range(0, accentTiles.Length);
            return accentTiles[randomIndex];
        }
        return null;
    }
}
