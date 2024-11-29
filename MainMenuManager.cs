using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject firstPanel;
    [SerializeField] private GameObject secondPanel;
    [SerializeField] private Button continueButton;

    [SerializeField] private GameObject mainMenuPanel;

    private void Start()
    {
        mainMenuPanel.SetActive(true);

        firstPanel.SetActive(false);
        secondPanel.SetActive(false);

        bool exists = PlayerPrefs.HasKey("SaveGameExists");
        continueButton.interactable = exists;
    }

    public void OnContinueGameButtonClicked()
    {
        bool exists = PlayerPrefs.HasKey("SaveGameExists");
        continueButton.interactable = exists;

        if (!exists) { return; }
        SceneManager.LoadScene("Game");
    }

    public void OnNewGameButtonPressed()
    {
        PlayerPrefs.DeleteAll();

        //mainMenuPanel.SetActive(false);

        //firstPanel.SetActive(true);
        //secondPanel.SetActive(true);

        SceneManager.LoadScene("Game");
    }

    [ContextMenu("Delete Player Prefs")]
    private void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        bool exists = PlayerPrefs.HasKey("SaveGameExists");
        continueButton.interactable = exists;
    }
}
