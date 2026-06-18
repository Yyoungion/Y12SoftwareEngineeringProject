using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    
    private Button resumeButton;
    private Button settingsButton;
    private Button quitButton;
    
    private bool isMenuOpen = false;
    
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        
        // Find buttons
        resumeButton = root.Q<Button>("ResumeButton");
        settingsButton = root.Q<Button>("SettingsButton");
        quitButton = root.Q<Button>("QuitButton");
        
        // Register clicks
        resumeButton?.RegisterCallback<ClickEvent>(evt => ResumeGame());
        settingsButton?.RegisterCallback<ClickEvent>(evt => OpenSettings());
        quitButton?.RegisterCallback<ClickEvent>(evt => QuitToMenu());
        
        // Start hidden
        ClosePauseMenu();
    }
    
    void Update()
    {
        // Toggle pause menu with ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuOpen)
            {
                ResumeGame();
            }
            else if (PauseManager.Instance != null && !PauseManager.Instance.isPaused)
            {
                OpenPauseMenu();
            }
        }
    }
    
    public void OpenPauseMenu()
    {
        root.style.display = DisplayStyle.Flex;
        isMenuOpen = true;
        
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Pause();
        }
        else
        {
            Time.timeScale = 0f;
        }
        
        Debug.Log("Pause menu opened");
    }
    
    public void ClosePauseMenu()
    {
        root.style.display = DisplayStyle.None;
        isMenuOpen = false;
    }
    
    void ResumeGame()
    {
        ClosePauseMenu();
        
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Resume();
        }
        else
        {
            Time.timeScale = 1f;
        }
        
        Debug.Log("Game resumed");
    }
    
    void OpenSettings()
    {
        // Close pause menu
        ClosePauseMenu();
        
        // Find and open settings menu
        SettingsMenuController settingsMenu = FindFirstObjectByType<SettingsMenuController>();
        if (settingsMenu != null)
        {
            settingsMenu.OpenSettings();
        }
        else
        {
            Debug.LogWarning("SettingsMenuController not found!");
        }
    }
    
    void QuitToMenu()
    {
        // Unpause before loading scene
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Resume();
        }
        else
        {
            Time.timeScale = 1f;
        }
        
        // Load main menu
        SceneManager.LoadScene("MainMenuScene");
    }
    
    void OnDestroy()
    {
        // Make sure game unpauses
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