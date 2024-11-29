using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Inventory,
    Action,
    Dialogue,
    Paused,
    GameOver
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState currentGameState { get; private set; }

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep this across scenes
        } else {
            Destroy(gameObject);  // Ensure only one instance exists
        }
    }

    private void Start()
    {
        SetGameState(GameState.Playing); // Update later to main menu if we start from there, for testing we are starting in game
    }

    public void SetGameState(GameState newGameState)
    {
        if (currentGameState != newGameState) {
            currentGameState = newGameState;
            HandleGameStateChange();
        }
    }

    private void HandleGameStateChange()
    {
        switch (currentGameState) {
            case GameState.MainMenu:
                // Handle main menu logic
                Time.timeScale = 0f;  // Stop time if needed
                break;

            case GameState.Playing:
                // Start/resume gameplay
                Time.timeScale = 1f;  // Ensure time is running
                break;

            case GameState.Inventory:
                // Start/resume gameplay
                Time.timeScale = 1f;  // Ensure time is running
                break;

            case GameState.Action:
                // Start/resume gameplay
                Time.timeScale = 1f;  // Ensure time is running
                break;

            case GameState.Dialogue:
                // Start/resume gameplay
                Time.timeScale = 1f;  // Ensure time is running
                break;

            case GameState.Paused:
                // Pause game logic
                Time.timeScale = 0f;  // Stop time for pause
                break;

            case GameState.GameOver:
                // Game over logic
                Time.timeScale = 0f;  // Stop time if game is over
                break;
        }
    }

    // Optional: Adding additional helper methods for convenience
    public bool IsPlaying()
    {
        return currentGameState == GameState.Playing;
    }

    public bool IsInventoryOpen()
    {
        return currentGameState == GameState.Inventory;
    }

    public bool IsDialogue()
    {
        return currentGameState == GameState.Dialogue;
    }

    public bool IsPaused()
    {
        return currentGameState == GameState.Paused;
    }
}
