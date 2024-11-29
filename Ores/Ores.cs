using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ores", menuName = "New Ores/Ore")]
public class Ores : ScriptableObject
{
    public int health;
    public Sprite oreSprite;
    public Sprite collectibleSprite;
    public int minDropAmount;
    public int maxDropAmount;
}
