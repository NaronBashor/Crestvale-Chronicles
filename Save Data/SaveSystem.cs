using System.IO;
using Newtonsoft.Json;
using UnityEngine;

// Serializable ColorData class to hold color information
[System.Serializable]
public class ColorData
{
    public float r;
    public float g;
    public float b;
    public float a;

    public ColorData(Color color)
    {
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }

    // Convert ColorData back to Unity Color
    public Color ToColor()
    {
        return new Color(r, g, b, a);
    }
}

// SaveSystem class for saving and loading data using Newtonsoft.Json
public static class SaveSystem
{
    public static void SaveColors(Color[] colors, string part)
    {
        string savePath = Application.persistentDataPath + "/" + part + "colors.json";

        // Convert Color array to ColorData array for serialization
        ColorData[] colorDataArray = new ColorData[colors.Length];
        for (int i = 0; i < colors.Length; i++) {
            colorDataArray[i] = new ColorData(colors[i]);
        }

        // Serialize the ColorData array to JSON using Newtonsoft.Json
        string json = JsonConvert.SerializeObject(colorDataArray, Formatting.Indented);

        // Write the JSON data to a file
        File.WriteAllText(savePath, json);
    }

    public static Color[] LoadColors(string part)
    {
        string savePath = Application.persistentDataPath + "/" + part + "colors.json";

        // Check if the file exists
        if (File.Exists(savePath)) {
            // Read the JSON data from the file
            string json = File.ReadAllText(savePath);

            // Deserialize the JSON back into an array of ColorData
            ColorData[] savedData = JsonConvert.DeserializeObject<ColorData[]>(json);

            // Convert ColorData array back to Color array
            Color[] colors = new Color[savedData.Length];
            for (int i = 0; i < savedData.Length; i++) {
                colors[i] = savedData[i].ToColor();
            }

            return colors;
        } else {
            // If file doesn't exist, return an array with the default color
            //return new Color[] { defaultColor1, defaultColor2, defaultColor3 };
            return null;
        }
    }
}