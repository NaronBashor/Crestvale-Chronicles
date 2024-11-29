using DirePixel.Animation;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using static UniversalColorChanger;

public class LoadCreatedPlayerPresets : MonoBehaviour
{
    private List<Color[]> originalPixelData;  // To store the original pixel data for each texture

    private string[] newColors;

    public SpriteRenderer spriteRenderer;
    [SerializeField] private PaperDoll paperDoll;  // PaperDoll object for applying textures/sprites
    [SerializeField] private List<Texture2D> bodyPartSpriteSheet = new List<Texture2D>(); // Sprite sheet for each body part
    public string part;
    public int spriteSheetIndex;

    // Enum for body part names
    public BodyPart bodyPart; // Body part currently being worked on

    // Start is called before the first frame update
    private void Awake()
    {
        ApplySavedTexture(); // Apply saved sprite and colors on load
    }

    private void Start()
    {
        //ChangeAllSpriteSheetPixelsToMatchNewColors();
    }

    // Method to apply saved texture (sprite sheet) based on the current body part
    private void ApplySavedTexture()
    {
        // Use PlayerPrefs to get saved index, defaulting to 0 if not set
        switch (bodyPart) {
            case BodyPart.Body:
                part = "Body";
                spriteSheetIndex = 0;
                paperDoll.SetTexture(bodyPartSpriteSheet[spriteSheetIndex]);
                // Load newColors from SaveSystem (assuming this returns an array of Colors)
                //Color[] loadedColors = SaveSystem.LoadColors(part);

                // Convert each Color to Hex and store in newColors array
                //newColors = new string[loadedColors.Length];
                //for (int i = 0; i < loadedColors.Length; i++) {
                //    newColors[i] = ColorToHex(loadedColors[i]);
                //}
                break;
            case BodyPart.Hair:
                part = "Hair";
                spriteSheetIndex = PlayerPrefs.GetInt("HairSpriteSheetIndex", 0);
                paperDoll.SetTexture(bodyPartSpriteSheet[spriteSheetIndex]);
                // Load newColors from SaveSystem (assuming this returns an array of Colors)
                //loadedColors = SaveSystem.LoadColors(part);

                //// Convert each Color to Hex and store in newColors array
                //newColors = new string[loadedColors.Length];
                //for (int i = 0; i < loadedColors.Length; i++) {
                //    newColors[i] = ColorToHex(loadedColors[i]);
                //}
                break;
            case BodyPart.Outer1:
                part = "Outer1";
                spriteSheetIndex = PlayerPrefs.GetInt("Outer1SpriteSheetIndex", 0);
                paperDoll.SetTexture(bodyPartSpriteSheet[spriteSheetIndex]);
                // Load newColors from SaveSystem (assuming this returns an array of Colors)
                //loadedColors = SaveSystem.LoadColors(part);

                //// Convert each Color to Hex and store in newColors array
                //newColors = new string[loadedColors.Length];
                //for (int i = 0; i < loadedColors.Length; i++) {
                //    newColors[i] = ColorToHex(loadedColors[i]);
                //}
                break;
            case BodyPart.Pants:
                part = "Pants";
                spriteSheetIndex = PlayerPrefs.GetInt("PantsSpriteSheetIndex", 0);
                paperDoll.SetTexture(bodyPartSpriteSheet[spriteSheetIndex]);
                // Load newColors from SaveSystem (assuming this returns an array of Colors)
                //loadedColors = SaveSystem.LoadColors(part);

                //// Convert each Color to Hex and store in newColors array
                //newColors = new string[loadedColors.Length];
                //for (int i = 0; i < loadedColors.Length; i++) {
                //    newColors[i] = ColorToHex(loadedColors[i]);
                //}
                break;
            case BodyPart.Shoes:
                part = "Shoes";
                spriteSheetIndex = PlayerPrefs.GetInt("ShoesSpriteSheetIndex", 0);
                paperDoll.SetTexture(bodyPartSpriteSheet[spriteSheetIndex]);
                // Load newColors from SaveSystem (assuming this returns an array of Colors)
                //loadedColors = SaveSystem.LoadColors(part);

                //// Convert each Color to Hex and store in newColors array
                //newColors = new string[loadedColors.Length];
                //for (int i = 0; i < loadedColors.Length; i++) {
                //    newColors[i] = ColorToHex(loadedColors[i]);
                //}
                break;
        }
    }

    private string ColorToHex(Color color)
    {
        // Convert each RGBA component to a 2-digit hex value
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);
        int a = Mathf.RoundToInt(color.a * 255);  // Optional: Alpha channel (transparency)

        // Combine into a hex string (without alpha if not needed)
        // Use a:X2 to include alpha channel if needed
        return $"#{r:X2}{g:X2}{b:X2}{a:X2}";  // Include alpha in the hex representation
    }

    bool IsNewGame()
    {
        return !PlayerPrefs.HasKey("SaveCharacterExists");
    }

    public void ChangeAllSpriteSheetPixelsToMatchNewColors()
    {
        // Initialize the list to store the original pixel data
        originalPixelData = new List<Color[]>(bodyPartSpriteSheet.Count);

        Color[] targetColorsArray;

        targetColorsArray = initialTargetColors(part);

        if (targetColorsArray == null || newColors == null || targetColorsArray.Length != newColors.Length) {
            //Debug.LogError("Target colors and new colors array mismatch or missing.");
            return;
        }

        // Iterate through all textures in the sprite sheet
        for (int t = 0; t < bodyPartSpriteSheet.Count; t++) {
            Texture2D selectedTexture = bodyPartSpriteSheet[t];

            if (!selectedTexture.isReadable) {
                Debug.LogError($"Texture {selectedTexture.name} is not readable. Check the import settings.");
                continue;
            }

            // Get the original pixels of the texture
            Color[] originalPixels = selectedTexture.GetPixels();
            originalPixelData.Add(originalPixels);  // Store the original pixel data

            // Modify the pixels in place by comparing to target colors
            for (int i = 0; i < originalPixels.Length; i++) {
                Color currentPixel = originalPixels[i];

                // Check if the current pixel matches any target color
                for (int j = 0; j < targetColorsArray.Length; j++) {
                    if (ColorsAreSimilar(currentPixel, targetColorsArray[j])) {
                        // Replace the pixel with the corresponding color from the newColors array
                        originalPixels[i] = new Color(targetColorsArray[j].r, targetColorsArray[j].g, targetColorsArray[j].b, currentPixel.a); // Preserve the alpha value
                        break;
                    }
                }

                // If no match is found, the original pixel is preserved, no change.
            }

            // Set the modified pixels back into the original texture
            selectedTexture.SetPixels(originalPixels);
            selectedTexture.Apply();  // Apply the changes to update the texture in place

            //Debug.Log($"Replaced matched target colors for texture {selectedTexture.name}");
        }
    }

    private bool ColorsAreSimilar(Color color1, Color color2, float tolerance = 0.1f)
    {
        return Mathf.Abs(color1.r - color2.r) < tolerance &&
               Mathf.Abs(color1.g - color2.g) < tolerance &&
               Mathf.Abs(color1.b - color2.b) < tolerance &&
               Mathf.Abs(color1.a - color2.a) < tolerance;
    }

    private Color[] initialTargetColors(string part)
    {
        Color[] targetColors;

        // Check if it's a new game or if saved colors exist
        if (!IsNewGame()) {
            // Load saved colors from SaveSystem if they exist
            Color[] savedColors = SaveSystem.LoadColors(part);
            if (savedColors != null && savedColors.Length > 0) {
                return savedColors;  // Return saved colors if available
            }
        }

        // If no saved colors, fall back to the default initial colors
        switch (part) {
            case "Body":
                targetColors = new Color[3];
                targetColors[0] = HexToColor("#885848");
                targetColors[1] = HexToColor("#D89878");
                targetColors[2] = HexToColor("#F8D8B8");
                return targetColors;
            case "Pants":
            case "Outer1":
            case "Shoes":
                targetColors = new Color[3];
                targetColors[0] = HexToColor("#205040");
                targetColors[1] = HexToColor("#289860");
                targetColors[2] = HexToColor("#58E0A0");
                return targetColors;
            case "Hair":
                targetColors = new Color[4];
                targetColors[0] = HexToColor("#503850");
                targetColors[1] = HexToColor("#686878");
                targetColors[2] = HexToColor("#8898A0");
                targetColors[3] = HexToColor("#C8C8B8");
                return targetColors;
        }
        return null;
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
}