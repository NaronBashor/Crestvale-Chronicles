using System.Collections.Generic;
using UnityEngine;

public class PlayerCreatorSelectedPresets : MonoBehaviour
{
    [SerializeField] public int skinSelection;
    [SerializeField] public int hairStyleSelection;
    [SerializeField] public int shoeStyleSelection;
    [SerializeField] public int pantsStyleSelection;
    [SerializeField] public int outerOneStyleSelection;
    [SerializeField] public int outerTwoStyleSelection;

    [SerializeField] public int hairColorSelection;
    [SerializeField] public int shoeColorSelection;
    [SerializeField] public int pantsColorSelection;
    [SerializeField] public int outerOneColorSelection;
    [SerializeField] public int outerTwoColorSelection;

    public void SavePlayerCharacterPresets()
    {
        PlayerPrefs.SetInt("skinSelection", skinSelection);
        PlayerPrefs.SetInt("hairStyleSelection", hairStyleSelection);
        PlayerPrefs.SetInt("shoeStyleSelection", shoeStyleSelection);
        PlayerPrefs.SetInt("pantsStyleSelection", pantsStyleSelection);
        PlayerPrefs.SetInt("outerOneStyleSelection", outerOneStyleSelection);
        PlayerPrefs.SetInt("outerTwoStyleSelection", outerTwoStyleSelection);
        PlayerPrefs.SetInt("hairColorSelection", hairColorSelection);
        PlayerPrefs.SetInt("shoeColorSelection", shoeColorSelection);
        PlayerPrefs.SetInt("pantsColorSelection", pantsColorSelection);
        PlayerPrefs.SetInt("outerOneColorSelection", outerOneColorSelection);
        PlayerPrefs.SetInt("outerTwoColorSelection", outerTwoColorSelection);
    }
}
