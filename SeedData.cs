using UnityEngine;

[CreateAssetMenu(fileName = "NewSeedData", menuName = "Seeds/SeedData")]
public class SeedData : ScriptableObject
{
    public string seedName;  // The name of the seed (e.g., "Beetroot")
    public Sprite[] growthStages;  // Sprites representing the bottom growth stages of the seed
    public float timeBetweenStages = 10f;  // Time between each growth stage
    public Sprite harvestIcon;
    public int yield;
}
