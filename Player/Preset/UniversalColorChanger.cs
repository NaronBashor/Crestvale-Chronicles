using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class ColorSet
{
    public Color[] targetColors;      // Array of target colors to cycle through
    public Color[] replacementColors; // Array of replacement colors that will replace target colors
}

public class UniversalColorChanger : MonoBehaviour
{
    // List of color sets for this specific body part
    public List<ColorSet> colorSet = new List<ColorSet>(); // List of multiple color sets

    // List of sprites (styles) for this specific body part
    public List<Sprite> partStyles = new List<Sprite>(); // List of sprites to represent different styles (e.g., shoe styles)

    public List<Texture2D> spriteSheets = new List<Texture2D>();

    // The current color set and color index
    public int currentColorSetIndex = 0;  // This is the index of the color set
    public int currentColorIndex = 0;     // This is the index of the color within the color set
    public int currentStyleIndex = 0;     // This is the index of the sprite style for the part

    public TextMeshProUGUI currentColorIndexText;
    public TextMeshProUGUI currentStyleIndexText;

    public SpriteRenderer spriteRenderer; // Reference to the sprite renderer for applying the color

    public enum BodyPart { Body, Shoes, Pants, Outer1, Hair };
    public BodyPart bodyPart;

    #region Start Method
    void Start()
    {
        // Set default colors depending on the body part
        if (bodyPart == BodyPart.Shoes || bodyPart == BodyPart.Pants || bodyPart == BodyPart.Outer1) {
            // Default colors for Shoes, Pants, or Outer1
            if (colorSet.Count == 0) {
                ColorSet defaultColorSet = new ColorSet();
                defaultColorSet.targetColors = new Color[3];
                defaultColorSet.targetColors[0] = HexToColor("#205040"); // Dark green
                defaultColorSet.targetColors[1] = HexToColor("#289860"); // Medium green
                defaultColorSet.targetColors[2] = HexToColor("#58E0A0"); // Light green

                // Add the replacement colors
                defaultColorSet.replacementColors = new Color[3];
                defaultColorSet.replacementColors[0] = HexToColor("#205040"); // Grayish green
                defaultColorSet.replacementColors[1] = HexToColor("#289860"); // Light greenish gray
                defaultColorSet.replacementColors[2] = HexToColor("#58E0A0"); // Light beige

                colorSet.Add(defaultColorSet);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet2 = new ColorSet();
                defaultColorSet2.targetColors = new Color[0]; // No target colors
                defaultColorSet2.replacementColors = new Color[3];
                defaultColorSet2.replacementColors[0] = HexToColor("#454995"); // Dark blue-purple
                defaultColorSet2.replacementColors[1] = HexToColor("#5371B0"); // Medium blue
                defaultColorSet2.replacementColors[2] = HexToColor("#79A4D1"); // Light blue

                colorSet.Add(defaultColorSet2);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet3 = new ColorSet();
                defaultColorSet3.targetColors = new Color[0]; // No target colors
                defaultColorSet3.replacementColors = new Color[3];
                defaultColorSet3.replacementColors[0] = HexToColor("#15809D"); // Dark blue-purple
                defaultColorSet3.replacementColors[1] = HexToColor("#40C0C1"); // Medium blue
                defaultColorSet3.replacementColors[2] = HexToColor("#A6E4D5"); // Light blue

                colorSet.Add(defaultColorSet3);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet4 = new ColorSet();
                defaultColorSet4.targetColors = new Color[0]; // No target colors
                defaultColorSet4.replacementColors = new Color[3];
                defaultColorSet4.replacementColors[0] = HexToColor("#628226"); // Dark blue-purple
                defaultColorSet4.replacementColors[1] = HexToColor("#A3B743"); // Medium blue
                defaultColorSet4.replacementColors[2] = HexToColor("#D8D790"); // Light blue

                colorSet.Add(defaultColorSet4);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet5 = new ColorSet();
                defaultColorSet5.targetColors = new Color[0]; // No target colors
                defaultColorSet5.replacementColors = new Color[3];
                defaultColorSet5.replacementColors[0] = HexToColor("#786850"); // Dark blue-purple
                defaultColorSet5.replacementColors[1] = HexToColor("#B8A080"); // Medium blue
                defaultColorSet5.replacementColors[2] = HexToColor("#E8D0B0"); // Light blue

                colorSet.Add(defaultColorSet5);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet6 = new ColorSet();
                defaultColorSet6.targetColors = new Color[0]; // No target colors
                defaultColorSet6.replacementColors = new Color[3];
                defaultColorSet6.replacementColors[0] = HexToColor("#945816"); // Dark blue-purple
                defaultColorSet6.replacementColors[1] = HexToColor("#CD9217"); // Medium blue
                defaultColorSet6.replacementColors[2] = HexToColor("#D7C448"); // Light blue

                colorSet.Add(defaultColorSet6);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet7 = new ColorSet();
                defaultColorSet7.targetColors = new Color[0]; // No target colors
                defaultColorSet7.replacementColors = new Color[3];
                defaultColorSet7.replacementColors[0] = HexToColor("#89152C"); // Dark blue-purple
                defaultColorSet7.replacementColors[1] = HexToColor("#CF3939"); // Medium blue
                defaultColorSet7.replacementColors[2] = HexToColor("#FF7869"); // Light blue

                colorSet.Add(defaultColorSet7);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet8 = new ColorSet();
                defaultColorSet8.targetColors = new Color[0]; // No target colors
                defaultColorSet8.replacementColors = new Color[3];
                defaultColorSet8.replacementColors[0] = HexToColor("#8D5ABC"); // Dark blue-purple
                defaultColorSet8.replacementColors[1] = HexToColor("#D381E4"); // Medium blue
                defaultColorSet8.replacementColors[2] = HexToColor("#F4BDE6"); // Light blue

                colorSet.Add(defaultColorSet8);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet9 = new ColorSet();
                defaultColorSet9.targetColors = new Color[0]; // No target colors
                defaultColorSet9.replacementColors = new Color[3];
                defaultColorSet9.replacementColors[0] = HexToColor("#4356A7"); // Dark blue-purple
                defaultColorSet9.replacementColors[1] = HexToColor("#9B7AD8"); // Medium blue
                defaultColorSet9.replacementColors[2] = HexToColor("#EAAAE9"); // Light blue

                colorSet.Add(defaultColorSet9);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet10 = new ColorSet();
                defaultColorSet10.targetColors = new Color[0]; // No target colors
                defaultColorSet10.replacementColors = new Color[3];
                defaultColorSet10.replacementColors[0] = HexToColor("#15809D"); // Dark blue-purple
                defaultColorSet10.replacementColors[1] = HexToColor("#40C0C1"); // Medium blue
                defaultColorSet10.replacementColors[2] = HexToColor("#A6E4D5"); // Light blue

                colorSet.Add(defaultColorSet10);
            }
            currentColorIndex = 0; // Set the index to 0
        } else if (bodyPart == BodyPart.Hair) {
            // Default colors for Hair
            if (colorSet.Count == 0) {
                ColorSet defaultColorSet = new ColorSet();
                defaultColorSet.targetColors = new Color[4]; // Hair has 4 default colors
                defaultColorSet.targetColors[0] = HexToColor("#503850"); // Dark purple-gray
                defaultColorSet.targetColors[1] = HexToColor("#686878"); // Gray
                defaultColorSet.targetColors[2] = HexToColor("#8898A0"); // Light blue-gray
                defaultColorSet.targetColors[3] = HexToColor("#C8C8B8"); // Light beige

                colorSet.Add(defaultColorSet);

                // Add the replacement colors
                defaultColorSet.replacementColors = new Color[4];
                defaultColorSet.replacementColors[0] = HexToColor("#503850"); // Grayish green
                defaultColorSet.replacementColors[1] = HexToColor("#686878");
                defaultColorSet.replacementColors[2] = HexToColor("#8898A0"); // Light greenish gray
                defaultColorSet.replacementColors[3] = HexToColor("#C8C8B8"); // Light beige

                colorSet.Add(defaultColorSet);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet2 = new ColorSet();
                defaultColorSet2.targetColors = new Color[0]; // No target colors
                defaultColorSet2.replacementColors = new Color[4];
                defaultColorSet2.replacementColors[0] = HexToColor("#454995"); // Dark blue-purple
                defaultColorSet2.replacementColors[1] = HexToColor("#3B4280");
                defaultColorSet2.replacementColors[2] = HexToColor("#5371B0"); // Medium blue
                defaultColorSet2.replacementColors[3] = HexToColor("#79A4D1"); // Light blue

                colorSet.Add(defaultColorSet2);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet3 = new ColorSet();
                defaultColorSet3.targetColors = new Color[0]; // No target colors
                defaultColorSet3.replacementColors = new Color[4];
                defaultColorSet3.replacementColors[0] = HexToColor("#15809D"); // Dark blue-purple
                defaultColorSet3.replacementColors[1] = HexToColor("#1A9DB2");
                defaultColorSet3.replacementColors[2] = HexToColor("#40C0C1"); // Medium blue
                defaultColorSet3.replacementColors[3] = HexToColor("#A6E4D5"); // Light blue

                colorSet.Add(defaultColorSet3);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet4 = new ColorSet();
                defaultColorSet4.targetColors = new Color[0]; // No target colors
                defaultColorSet4.replacementColors = new Color[4];
                defaultColorSet4.replacementColors[0] = HexToColor("#628226"); // Dark blue-purple
                defaultColorSet4.replacementColors[1] = HexToColor("#6E8F3A");
                defaultColorSet4.replacementColors[2] = HexToColor("#A3B743"); // Medium blue
                defaultColorSet4.replacementColors[3] = HexToColor("#D8D790"); // Light blue

                colorSet.Add(defaultColorSet4);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet5 = new ColorSet();
                defaultColorSet5.targetColors = new Color[0]; // No target colors
                defaultColorSet5.replacementColors = new Color[4];
                defaultColorSet5.replacementColors[0] = HexToColor("#786850"); // Dark blue-purple
                defaultColorSet5.replacementColors[1] = HexToColor("#8B7464");
                defaultColorSet5.replacementColors[2] = HexToColor("#B8A080"); // Medium blue
                defaultColorSet5.replacementColors[3] = HexToColor("#E8D0B0"); // Light blue

                colorSet.Add(defaultColorSet5);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet6 = new ColorSet();
                defaultColorSet6.targetColors = new Color[0]; // No target colors
                defaultColorSet6.replacementColors = new Color[4];
                defaultColorSet6.replacementColors[0] = HexToColor("#945816"); // Dark blue-purple
                defaultColorSet6.replacementColors[1] = HexToColor("#A0633A");
                defaultColorSet6.replacementColors[2] = HexToColor("#CD9217"); // Medium blue
                defaultColorSet6.replacementColors[3] = HexToColor("#D7C448"); // Light blue

                colorSet.Add(defaultColorSet6);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet7 = new ColorSet();
                defaultColorSet7.targetColors = new Color[0]; // No target colors
                defaultColorSet7.replacementColors = new Color[4];
                defaultColorSet7.replacementColors[0] = HexToColor("#89152C"); // Dark blue-purple
                defaultColorSet7.replacementColors[1] = HexToColor("#992438");
                defaultColorSet7.replacementColors[2] = HexToColor("#CF3939"); // Medium blue
                defaultColorSet7.replacementColors[3] = HexToColor("#FF7869"); // Light blue

                colorSet.Add(defaultColorSet7);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet8 = new ColorSet();
                defaultColorSet8.targetColors = new Color[0]; // No target colors
                defaultColorSet8.replacementColors = new Color[4];
                defaultColorSet8.replacementColors[0] = HexToColor("#8D5ABC"); // Dark blue-purple
                defaultColorSet8.replacementColors[1] = HexToColor("#9D66C4");
                defaultColorSet8.replacementColors[2] = HexToColor("#D381E4"); // Medium blue
                defaultColorSet8.replacementColors[3] = HexToColor("#F4BDE6"); // Light blue

                colorSet.Add(defaultColorSet8);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet9 = new ColorSet();
                defaultColorSet9.targetColors = new Color[0]; // No target colors
                defaultColorSet9.replacementColors = new Color[4];
                defaultColorSet9.replacementColors[0] = HexToColor("#4356A7"); // Dark blue-purple
                defaultColorSet9.replacementColors[1] = HexToColor("#4D6AB9");
                defaultColorSet9.replacementColors[2] = HexToColor("#9B7AD8"); // Medium blue
                defaultColorSet9.replacementColors[3] = HexToColor("#EAAAE9"); // Light blue

                colorSet.Add(defaultColorSet9);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet10 = new ColorSet();
                defaultColorSet10.targetColors = new Color[0]; // No target colors
                defaultColorSet10.replacementColors = new Color[4];
                defaultColorSet10.replacementColors[0] = HexToColor("#15809D"); // Dark blue-purple
                defaultColorSet10.replacementColors[1] = HexToColor("#2A97A6");
                defaultColorSet10.replacementColors[2] = HexToColor("#40C0C1"); // Medium blue
                defaultColorSet10.replacementColors[3] = HexToColor("#A6E4D5"); // Light blue

                colorSet.Add(defaultColorSet10);

                // Second ColorSet with no target colors but replacement colors
                ColorSet defaultColorSet11 = new ColorSet();
                defaultColorSet11.targetColors = new Color[0]; // No target colors
                defaultColorSet11.replacementColors = new Color[4];
                defaultColorSet11.replacementColors[0] = HexToColor("#65390F");
                defaultColorSet11.replacementColors[1] = HexToColor("#B17326");
                defaultColorSet11.replacementColors[2] = HexToColor("#DEAD2D");
                defaultColorSet11.replacementColors[3] = HexToColor("#F4E082");

                colorSet.Add(defaultColorSet11);
            }
            currentColorIndex = 0; // Set the index to 0
        } else if (bodyPart == BodyPart.Body) {
            if (colorSet.Count == 0) {
                // First ColorSet
                ColorSet defaultColorSet = new ColorSet();
                defaultColorSet.targetColors = new Color[3];
                defaultColorSet.targetColors[0] = HexToColor("#885848");
                defaultColorSet.targetColors[1] = HexToColor("#D89878");
                defaultColorSet.targetColors[2] = HexToColor("#F8D8B8");

                // Add the replacement colors
                defaultColorSet.replacementColors = new Color[3];
                defaultColorSet.replacementColors[0] = HexToColor("#885848");
                defaultColorSet.replacementColors[1] = HexToColor("#D89878");
                defaultColorSet.replacementColors[2] = HexToColor("#F8D8B8");

                colorSet.Add(defaultColorSet);

                // Second ColorSet
                ColorSet defaultColorSet2 = new ColorSet();
                defaultColorSet2.targetColors = new Color[0];
                defaultColorSet2.replacementColors = new Color[3];
                defaultColorSet2.replacementColors[0] = HexToColor("#805058");
                defaultColorSet2.replacementColors[1] = HexToColor("#D08878");
                defaultColorSet2.replacementColors[2] = HexToColor("#F8C8B0");

                colorSet.Add(defaultColorSet2);

                //// Third ColorSet
                ColorSet defaultColorSet3 = new ColorSet();
                defaultColorSet3.targetColors = new Color[0];
                defaultColorSet3.replacementColors = new Color[3];
                defaultColorSet3.replacementColors[0] = HexToColor("#8A5050");
                defaultColorSet3.replacementColors[1] = HexToColor("#D07870");
                defaultColorSet3.replacementColors[2] = HexToColor("#F8C0A8");

                colorSet.Add(defaultColorSet3);

                //// Fourth ColorSet
                ColorSet defaultColorSet4 = new ColorSet();
                defaultColorSet4.targetColors = new Color[0];
                defaultColorSet4.replacementColors = new Color[3];
                defaultColorSet4.replacementColors[0] = HexToColor("#9B4326");
                defaultColorSet4.replacementColors[1] = HexToColor("#D08070");
                defaultColorSet4.replacementColors[2] = HexToColor("#F8C0B0");

                colorSet.Add(defaultColorSet4);

                //// Fifth ColorSet
                ColorSet defaultColorSet5 = new ColorSet();
                defaultColorSet5.targetColors = new Color[0];
                defaultColorSet5.replacementColors = new Color[3];
                defaultColorSet5.replacementColors[0] = HexToColor("#8C5139");
                defaultColorSet5.replacementColors[1] = HexToColor("#D68A63");
                defaultColorSet5.replacementColors[2] = HexToColor("#FFD39C");

                colorSet.Add(defaultColorSet5);

                //// Sixth ColorSet
                ColorSet defaultColorSet6 = new ColorSet();
                defaultColorSet6.targetColors = new Color[0];
                defaultColorSet6.replacementColors = new Color[3];
                defaultColorSet6.replacementColors[0] = HexToColor("#785030");
                defaultColorSet6.replacementColors[1] = HexToColor("#C88058");
                defaultColorSet6.replacementColors[2] = HexToColor("#F0C090");

                colorSet.Add(defaultColorSet6);

                //// Seventh ColorSet
                ColorSet defaultColorSet7 = new ColorSet();
                defaultColorSet7.targetColors = new Color[0];
                defaultColorSet7.replacementColors = new Color[3];
                defaultColorSet7.replacementColors[0] = HexToColor("#784020");
                defaultColorSet7.replacementColors[1] = HexToColor("#C08050");
                defaultColorSet7.replacementColors[2] = HexToColor("#F8B880");

                colorSet.Add(defaultColorSet7);

                //// Eighth ColorSet
                ColorSet defaultColorSet8 = new ColorSet();
                defaultColorSet8.targetColors = new Color[0];
                defaultColorSet8.replacementColors = new Color[3];
                defaultColorSet8.replacementColors[0] = HexToColor("#8C4131");
                defaultColorSet8.replacementColors[1] = HexToColor("#DE864B");
                defaultColorSet8.replacementColors[2] = HexToColor("#FFC784");

                colorSet.Add(defaultColorSet8);

                //// Ninth ColorSet
                ColorSet defaultColorSet9 = new ColorSet();
                defaultColorSet9.targetColors = new Color[0];
                defaultColorSet9.replacementColors = new Color[3];
                defaultColorSet9.replacementColors[0] = HexToColor("#804828");
                defaultColorSet9.replacementColors[1] = HexToColor("#C88028");
                defaultColorSet9.replacementColors[2] = HexToColor("#F0C080");

                colorSet.Add(defaultColorSet9);

                // Potential future body colors

                //// Tenth ColorSet
                //ColorSet defaultColorSet10 = new ColorSet();
                //defaultColorSet10.targetColors = new Color[0];
                //defaultColorSet10.replacementColors = new Color[3];
                //defaultColorSet10.replacementColors[0] = HexToColor("#181818");
                //defaultColorSet10.replacementColors[1] = HexToColor("#634A29");
                //defaultColorSet10.replacementColors[2] = HexToColor("#AD8452");

                //colorSet.Add(defaultColorSet10);

                //// Eleventh ColorSet
                //ColorSet defaultColorSet11 = new ColorSet();
                //defaultColorSet11.targetColors = new Color[0];
                //defaultColorSet11.replacementColors = new Color[5];
                //defaultColorSet11.replacementColors[0] = HexToColor("#181818");
                //defaultColorSet11.replacementColors[1] = HexToColor("#58331B");
                //defaultColorSet11.replacementColors[2] = HexToColor("#A56D51");
                //defaultColorSet11.replacementColors[3] = HexToColor("#D5AA72");
                //defaultColorSet11.replacementColors[4] = HexToColor("#F8F0E0");

                //colorSet.Add(defaultColorSet11);

                //// Twelfth ColorSet
                //ColorSet defaultColorSet12 = new ColorSet();
                //defaultColorSet12.targetColors = new Color[0];
                //defaultColorSet12.replacementColors = new Color[5];
                //defaultColorSet12.replacementColors[0] = HexToColor("#181818");
                //defaultColorSet12.replacementColors[1] = HexToColor("#6B3112");
                //defaultColorSet12.replacementColors[2] = HexToColor("#A36039");
                //defaultColorSet12.replacementColors[3] = HexToColor("#DEA05B");
                //defaultColorSet12.replacementColors[4] = HexToColor("#F8F0E0");

                //colorSet.Add(defaultColorSet12);

                //// Thirteenth ColorSet
                //ColorSet defaultColorSet13 = new ColorSet();
                //defaultColorSet13.targetColors = new Color[0];
                //defaultColorSet13.replacementColors = new Color[5];
                //defaultColorSet13.replacementColors[0] = HexToColor("#181818");
                //defaultColorSet13.replacementColors[1] = HexToColor("#633416");
                //defaultColorSet13.replacementColors[2] = HexToColor("#B56131");
                //defaultColorSet13.replacementColors[3] = HexToColor("#F7A273");
                //defaultColorSet13.replacementColors[4] = HexToColor("#F8F0E0");

                //colorSet.Add(defaultColorSet13);

                //// Fourteenth ColorSet
                //ColorSet defaultColorSet14 = new ColorSet();
                //defaultColorSet14.targetColors = new Color[0];
                //defaultColorSet14.replacementColors = new Color[5];
                //defaultColorSet14.replacementColors[0] = HexToColor("#181818");
                //defaultColorSet14.replacementColors[1] = HexToColor("#772A1B");
                //defaultColorSet14.replacementColors[2] = HexToColor("#AB5544");
                //defaultColorSet14.replacementColors[3] = HexToColor("#E09060");
                //defaultColorSet14.replacementColors[4] = HexToColor("#F8F0E0");

                //colorSet.Add(defaultColorSet14);

                //// Fifteenth ColorSet
                //ColorSet defaultColorSet15 = new ColorSet();
                //defaultColorSet15.targetColors = new Color[0];
                //defaultColorSet15.replacementColors = new Color[5];
                //defaultColorSet15.replacementColors[0] = HexToColor("#181818");
                //defaultColorSet15.replacementColors[1] = HexToColor("#422921");
                //defaultColorSet15.replacementColors[2] = HexToColor("#735A42");
                //defaultColorSet15.replacementColors[3] = HexToColor("#A5845A");
                //defaultColorSet15.replacementColors[4] = HexToColor("#F8F0E0");

                //colorSet.Add(defaultColorSet15);

                //// Sixteenth ColorSet
                //ColorSet defaultColorSet16 = new ColorSet();
                //defaultColorSet16.targetColors = new Color[0];
                //defaultColorSet16.replacementColors = new Color[5];
                //defaultColorSet16.replacementColors[0] = HexToColor("#181818");
                //defaultColorSet16.replacementColors[1] = HexToColor("#4B2D26");
                //defaultColorSet16.replacementColors[2] = HexToColor("#7C5834");
                //defaultColorSet16.replacementColors[3] = HexToColor("#B28246");
                //defaultColorSet16.replacementColors[4] = HexToColor("#F8F0E0");

                //colorSet.Add(defaultColorSet16);

                //// Seventeenth ColorSet
                //ColorSet defaultColorSet17 = new ColorSet();
                //defaultColorSet17.targetColors = new Color[0];
                //defaultColorSet17.replacementColors = new Color[5];
                //defaultColorSet17.replacementColors[0] = HexToColor("#181818");
                //defaultColorSet17.replacementColors[1] = HexToColor("#502828");
                //defaultColorSet17.replacementColors[2] = HexToColor("#774A2F");
                //defaultColorSet17.replacementColors[3] = HexToColor("#B07040");
                //defaultColorSet17.replacementColors[4] = HexToColor("#F8F0E0");

                //colorSet.Add(defaultColorSet17);

                //// Eighteenth ColorSet
                //ColorSet defaultColorSet18 = new ColorSet();
                //defaultColorSet18.targetColors = new Color[0];
                //defaultColorSet18.replacementColors = new Color[5];
                //defaultColorSet18.replacementColors[0] = HexToColor("#181818");
                //defaultColorSet18.replacementColors[1] = HexToColor("#885848");
                //defaultColorSet18.replacementColors[2] = HexToColor("#D89878");
                //defaultColorSet18.replacementColors[3] = HexToColor("#F8D8B8");
                //defaultColorSet18.replacementColors[4] = HexToColor("#F8F0E0");

                //colorSet.Add(defaultColorSet18);
            }

            currentColorIndex = 0; // Set the index to 0
        }

        // Apply the first color from the first color set on start
        if (colorSet.Count > 0) {
            // Set the current color set and color index to 0 to apply the first color
            currentColorSetIndex = 0;
            currentColorIndex = 0;

            // Apply the first color from the first color set
            ApplyColor(currentColorIndex);

            // Update the UI to reflect the current color and style
            UpdateUIText();

            //Debug.Log("First color applied on start.");
        } else {
            Debug.LogError("No color sets available to apply.");
        }

        // Ensure we have at least one style (sprite) to apply
        if (partStyles.Count > 0) {
            ApplyStyle(0); // Apply the first style as default
            currentStyleIndex = 0; // Set the index to 0
        } else {
            Debug.LogError("No styles assigned for this part!");
        }

        // Update the UI text to reflect the current color and style indices
        UpdateUIText();

        //if (bodyPart == BodyPart.Body) { return; }
        ApplyColorChanges();

        OnNewColorButtonPressed();
        OnNewColorSetButtonPressed();
    }
    #endregion

    private void ApplyColorChanges()
    {
        if (spriteRenderer != null && colorSet.Count > 0) {
            // Get the original texture from the sprite and create a new texture with the same dimensions
            Texture2D originalTexture = spriteRenderer.sprite.texture;
            Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height);

            // Copy the original texture's pixels
            Color[] originalPixels = originalTexture.GetPixels();
            Color[] newPixels = new Color[originalPixels.Length];

            // Iterate through the pixels and apply the color changes
            for (int i = 0; i < originalPixels.Length; i++) {
                Color originalPixelColor = originalPixels[i];

                // Check if the pixel matches any target color, and if so, replace it
                bool colorReplaced = false;
                for (int j = 0; j < colorSet[currentColorSetIndex].targetColors.Length; j++) {
                    if (ColorsAreSimilar(originalPixelColor, colorSet[currentColorSetIndex].targetColors[j])) {
                        newPixels[i] = colorSet[currentColorSetIndex].replacementColors[j];
                        colorReplaced = true;
                        break;
                    }
                }

                // If no replacement was found, keep the original color
                if (!colorReplaced) {
                    newPixels[i] = originalPixelColor;
                }
            }

            // Set the new pixels and apply the new texture
            newTexture.SetPixels(newPixels);
            newTexture.Apply();

            // Disable filtering and texture wrapping for pixel-perfect results
            newTexture.filterMode = FilterMode.Point;
            newTexture.wrapMode = TextureWrapMode.Clamp;

            // Create a new sprite, using the same rect, pivot, and pixels-per-unit as the original sprite
            Sprite originalSprite = spriteRenderer.sprite;
            Vector2 pivot = new Vector2(originalSprite.pivot.x / originalSprite.rect.width,
                                        originalSprite.pivot.y / originalSprite.rect.height);

            Sprite newSprite = Sprite.Create(
                newTexture,
                originalSprite.rect,              // Use the original sprite's rect
                pivot,                            // Use recalculated pivot based on original sprite
                originalSprite.pixelsPerUnit      // Use the original sprite's pixels per unit
            );

            // Apply the new sprite to the SpriteRenderer
            spriteRenderer.sprite = newSprite;

            //Debug.Log($"Applied color set {currentColorSetIndex} to sprite.");
        } else {
            Debug.LogError("SpriteRenderer or colorSet is missing.");
        }
    }


    // Update the next color set's target colors with the replacement colors of the current set
    private void UpdateNextColorSetTargetColors()
    {
        // Find the next color set index, wrapping around to 0 if necessary
        int nextSetIndex = (currentColorSetIndex + 1) % colorSet.Count;

        //Debug.Log($"Updating target colors for set {nextSetIndex} from set {currentColorSetIndex}'s replacement colors.");

        // Check if the next set's targetColors array is initialized
        if (colorSet[nextSetIndex].targetColors == null || colorSet[nextSetIndex].targetColors.Length != colorSet[currentColorSetIndex].replacementColors.Length) {
            // Initialize targetColors with the same size as the replacementColors
            colorSet[nextSetIndex].targetColors = new Color[colorSet[currentColorSetIndex].replacementColors.Length];
        }

        // Update the next color set's target colors to be the current set's replacement colors
        for (int i = 0; i < colorSet[currentColorSetIndex].replacementColors.Length; i++) {
            if (i < colorSet[nextSetIndex].targetColors.Length) {
                colorSet[nextSetIndex].targetColors[i] = colorSet[currentColorSetIndex].replacementColors[i];
            }
        }

        //Debug.Log($"Updated color set {nextSetIndex} target colors from set {currentColorSetIndex}'s replacement colors.");
    }


    // Helper function to check if two colors are approximately the same
    private bool ColorsAreSimilar(Color color1, Color color2)
    {
        float threshold = 0.1f; // You can adjust this threshold for sensitivity
        return Mathf.Abs(color1.r - color2.r) < threshold &&
               Mathf.Abs(color1.g - color2.g) < threshold &&
               Mathf.Abs(color1.b - color2.b) < threshold &&
               Mathf.Abs(color1.a - color2.a) < threshold;
    }

    // Function to convert Hex string to Unity Color
    private Color HexToColor(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color)) {
            return color;
        } else {
            Debug.LogError("Invalid Hex color: " + hex);
            return Color.white; // Fallback to white if parsing fails
        }
    }

    // Function to be called when a new color is selected
    public void OnNewColorButtonPressed()
    {
        if (colorSet.Count > 0 && colorSet[currentColorSetIndex].targetColors.Length > 0) {
            // Cycle to the next color in the current color set
            currentColorIndex = (currentColorIndex + 1) % colorSet[currentColorSetIndex].targetColors.Length;
            ApplyColor(currentColorIndex); // Apply the new color
            UpdateUIText(); // Update UI text to reflect the new selection
        } else {
            Debug.LogError("No colors to cycle through!");
        }
    }

    // Function to be called when a new color set is selected
    public void OnNewColorSetButtonPressed()
    {
        if (colorSet.Count > 0) {
            // Apply the current color set (for body parts, only color cycling is needed)
            ApplyColorChanges();

            UpdateNextColorSetTargetColors();

            // Increment the color set index and cycle back to the first if necessary
            currentColorSetIndex = (currentColorSetIndex + 1) % colorSet.Count;

            //Debug.Log($"Switched to color set {currentColorSetIndex}");

            // Reset color index to 0 when switching to a new set
            currentColorIndex = 0;

            // Apply the color changes for the next color set
            ApplyColor(currentColorIndex); // Apply the first color of the new color set

            // Update the UI to reflect the new color set and index
            UpdateUIText();
        } else {
            Debug.LogError("No color sets to cycle through!");
        }
    }


    public void OnNewStyleButtonPressed()
    {
        if (bodyPart == BodyPart.Body) {
            //Debug.Log("Body part does not support styles.");
            return;
        }

        if (partStyles.Count > 0) {
            // Cycle to the next style in the list
            currentStyleIndex = (currentStyleIndex + 1) % partStyles.Count;
            switch (bodyPart) {
                case BodyPart.Body:
                    PlayerPrefs.SetInt("BodySpriteSheetIndex", currentStyleIndex);
                    break;
                case BodyPart.Hair:
                    PlayerPrefs.SetInt("HairSpriteSheetIndex", currentStyleIndex);
                    break;
                case BodyPart.Outer1:
                    PlayerPrefs.SetInt("Outer1SpriteSheetIndex", currentStyleIndex);
                    break;
                case BodyPart.Pants:
                    PlayerPrefs.SetInt("PantsSpriteSheetIndex", currentStyleIndex);
                    break;
                case BodyPart.Shoes:
                    PlayerPrefs.SetInt("ShoesSpriteSheetIndex", currentStyleIndex);
                    break;
            }

            // Apply the new style (sprite)
            ApplyStyle(currentStyleIndex);

            // Reset the color index and apply the first color of the current color set
            currentColorIndex = 0;  // Reset color index to the first color
            currentColorSetIndex = 0; // Optionally, reset the color set index as well if needed
            ApplyColorChanges(); // Apply color changes based on the current color set and index

            // Update the UI to reflect the reset color and style indices
            UpdateUIText();

            //Debug.Log("Style changed and colors reset.");
        } else {
            Debug.LogError("No styles assigned for this part!");
        }
    }



    // Apply the color based on the index from the current color set's targetColors array
    private void ApplyColor(int colorIndex)
    {
        ApplyColorChanges();
        ShowCurrentColors();
    }

    // Apply the style (sprite) based on the current style index
    private void ApplyStyle(int styleIndex)
    {
        if (styleIndex >= 0 && styleIndex < partStyles.Count) {
            if (spriteRenderer != null) {
                spriteRenderer.sprite = partStyles[styleIndex]; // Apply the selected sprite style
            } else {
                Debug.LogError("No SpriteRenderer found on this GameObject.");
            }
        } else {
            Debug.LogError("Invalid style index.");
        }
    }

    // Update the UI text to display the current indices
    private void UpdateUIText()
    {
        currentColorIndexText.text = currentColorIndex.ToString(); // Update color index display
        currentColorIndexText.text = currentColorSetIndex.ToString();

        // Only update the style index for non-body parts
        if (bodyPart != BodyPart.Body) {
            currentStyleIndexText.text = currentStyleIndex.ToString(); // Update style index display
        }
    }

    public void ShowCurrentColors()
    {
        //Color[] colors = new Color[colorSet[currentColorSetIndex].replacementColors.Length];
        //for (int i = 0; i < colorSet[currentColorSetIndex].replacementColors.Length; i++) {
        //    colors[i] = colorSet[currentColorSetIndex].replacementColors[i];
        //}
        //SaveSystem.SaveColors(colors, bodyPart.ToString());
    }
}