using UnityEngine; // Imports the Unity engine.
using UnityEngine.UIElements; // Imports UI Toolkit components like UIDocument and Button.
using UnityEngine.SceneManagement; // Imports scene loading functionality.

public class MainMenuUIController : MonoBehaviour // Controls the main menu UI behaviour.
{
    // Reference to the UI Document.
    private UIDocument uiDocument;

    // Button used to start the game.
    private Button playButton;

    // Button used to open the settings/options menu.
    private Button optionsButton;

    // Called automatically when the GameObject becomes active.
    void OnEnable()
    {
        // Get the UIDocument.
        uiDocument = GetComponent<UIDocument>();

        // Get the root UI element containing all child elements.
        var rootVisualElement = uiDocument.rootVisualElement;

        // Find the Play button by name in the UI hierarchy.
        playButton = rootVisualElement.Q<Button>("PlayButton");

        // Find the Options button by name in the UI hierarchy.
        optionsButton = rootVisualElement.Q<Button>("OptionsButton");

        // If Play button exists, attach click event.
        if (playButton != null)
        {
            playButton.clicked += OnPlayButtonClicked;
        }

        // If Options button exists, attach click.
        if (optionsButton != null)
        {
            optionsButton.clicked += OnOptionsButtonClicked;
        }
    }

    // Called automatically when the object is disabled or destroyed.
    void OnDisable()
    {
        // Remove Play button event to prevent memory leaks or duplicate calls.
        if (playButton != null)
        {
            playButton.clicked -= OnPlayButtonClicked;
        }

        // Remove Options button event safely.
        if (optionsButton != null)
        {
            optionsButton.clicked -= OnOptionsButtonClicked;
        }
    }

    // Called when the Play button is clicked.
    void OnPlayButtonClicked()
    {
        // Debug message for testing.
        Debug.Log("Play button clicked!");

        // Load the main gameplay scene.
        SceneManager.LoadScene("GameScene");
    }

    // Called when the Options button is clicked.
    void OnOptionsButtonClicked()
    {
        // Debug message for testing.
        Debug.Log("Options button clicked!");

        // Try to find the SettingsMenuController in the scene.
        SettingsMenuController settingsMenu = FindFirstObjectByType<SettingsMenuController>();

        // If settings menu exists
        if (settingsMenu != null)
        {
            // Open the settings menu.
            settingsMenu.OpenSettings();
        }
        else
        {
            // Warn if no settings menu was found.
            Debug.LogWarning("SettingsMenuController not found!");
        }
    }
}