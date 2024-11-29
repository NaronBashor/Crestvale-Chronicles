using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/item")]
public class Item : ScriptableObject
{
    public string itemName;
    public TileBase tile;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    public int stackSize;
    public SeedData seedData;

    public bool stackable = true;

    public Sprite image;

    public enum ItemType
    {
        Tool,
        Food,
        Block
    }

    public enum ActionType
    {
        Dig,
        Mine,
        Seed,
        Water,
        Attack
    }
}
