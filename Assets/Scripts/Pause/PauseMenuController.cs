using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

// Controls the pause menu UI
public class PauseMenuController : MonoBehaviour
{
    // References to the UI document and root visual element
    private UIDocument uiDocument;
    private VisualElement root;
    
    // References the pause menu buttons
    private Button resumeButton;
    private Button settingsButton;
    private Button quitButton;
    
    // Tracks whether the pause menu is currently open
    private bool isMenuOpen = false;
    
    // Called before the first frame
    void Start()
    {
        // Get the UI Document attached to this GameObject
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        
        // Find the buttons by their names in the UI Builder
        resumeButton = root.Q<Button>("ResumeButton");
        settingsButton = root.Q<Button>("SettingsButton");
        quitButton = root.Q<Button>("QuitButton");
        
        // Register button click events
        resumeButton?.RegisterCallback<ClickEvent>(evt => ResumeGame());
        settingsButton?.RegisterCallback<ClickEvent>(evt => OpenSettings());
        quitButton?.RegisterCallback<ClickEvent>(evt => QuitToMenu());
        
        // Hide the pause menu when the game starts
        ClosePauseMenu();
    }
    
    // Called once every frame
    void Update()
    {
        // When Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If the menu is already open, resume the game
            if (isMenuOpen)
            {
                ResumeGame();
            }
            // Otherwise, open the pause menu if the game is not already paused
            else if (PauseManager.Instance != null && !PauseManager.Instance.isPaused)
            {
                OpenPauseMenu();
            }
        }
    }
    
    // Opens the pause menu and pauses the game
    public void OpenPauseMenu()
    {
        // Show the pause menu
        root.style.display = DisplayStyle.Flex;
        isMenuOpen = true;
        
        // Pause the game using the PauseManager
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Pause();
        }
        else
        {
            Time.timeScale = 0f;
        }
        
        // Display a message in the Unity Console
        Debug.Log("Pause menu opened");
    }
    
    // Hides the pause menu
    public void ClosePauseMenu()
    {
        // Hide the menu and update its state
        root.style.display = DisplayStyle.None;
        isMenuOpen = false;
    }
    
    // Resumes gameplay
    void ResumeGame()
    {
        // Close the pause menu
        ClosePauseMenu();
        
        // Resume the game using the PauseManager if available
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Resume();
        }
        else
        {
            Time.timeScale = 1f;
        }
        
        // Display a message in the Unity Console
        Debug.Log("Game resumed");
    }
    
    // Opens the settings menu
    void OpenSettings()
    {
        // Close the pause menu
        ClosePauseMenu();
        
        // Find the SettingsMenuController
        SettingsMenuController settingsMenu = FindFirstObjectByType<SettingsMenuController>();
        
        // Open the settings menu8
        if (settingsMenu != null)
        {
            settingsMenu.OpenSettings();
        }
        else
        {
            // Display a warning
            Debug.LogWarning("SettingsMenuController not found!");
        }
    }
    
    // Returns to the main menu
    void QuitToMenu()
    {
        // Ensure the game is resumed
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Resume();
        }
        else
        {
            Time.timeScale = 1f;
        }
        
        // Load the main menu scene
        SceneManager.LoadScene("MainMenuScene");
    }
    
    // Called when this object is destroyed
    void OnDestroy()
    {
        // Ensure the game is never left paused
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Resume();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}