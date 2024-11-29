using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class ToolSwapper : MonoBehaviour
{
    public SpriteResolver spriteResolver; // Reference to the Sprite Resolver component

    // Method to switch between weapons
    public void SwitchWeapon(string weaponType)
    {
        // Assuming you have the "Sword" and "Axe" categories in the Sprite Library
        if (weaponType == "Sword") {
            spriteResolver.SetCategoryAndLabel("Sword", "_Attack_Frame_1");
        } else if (weaponType == "Axe") {
            spriteResolver.SetCategoryAndLabel("Axe", "_Attack_Frame_1");
        }

        // Resolve the new sprite to the SpriteRenderer
        spriteResolver.ResolveSpriteToSpriteRenderer();
    }
}
