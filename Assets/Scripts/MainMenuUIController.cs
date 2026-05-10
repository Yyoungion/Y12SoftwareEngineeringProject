// Imports Unity core engine APIs (MonoBehaviour, Debug, GetComponent, etc.).
using UnityEngine;
// Imports UI Toolkit types such as UIDocument, VisualElement, and Button.
using UnityEngine.UIElements;
// Imports scene-loading APIs for changing scenes.
using UnityEngine.SceneManagement;

// Defines a component that controls main menu UI button behavior.
public class MainMenuUIController : MonoBehaviour
{
    // Stores the UIDocument component attached to this GameObject.
    private UIDocument uiDocument;
    // Stores a reference to the Play button from the UI document.
    private Button playButton;
    // Stores a reference to the Options button from the UI document.
    private Button optionsButton;

    // Called when this component becomes enabled and active.
    void OnEnable()
    {
        // Get the UI Document component
        uiDocument = GetComponent<UIDocument>();
        
        // Get the root visual element
        var rootVisualElement = uiDocument.rootVisualElement;
        
        // Find buttons by their names (from UI Builder)
        playButton = rootVisualElement.Q<Button>("PlayButton");
        optionsButton = rootVisualElement.Q<Button>("OptionsButton");
        
        // Register click events
        if (playButton != null)
        {
            // Subscribe the Play button click to the play handler.
            playButton.clicked += OnPlayButtonClicked;
        }
        
        if (optionsButton != null)
        {
            // Subscribe the Options button click to the options handler.
            optionsButton.clicked += OnOptionsButtonClicked;
        }
    }
    
    // Called when this component becomes disabled or inactive.
    void OnDisable()
    {
        // Unregister events to prevent memory leaks
        if (playButton != null)
        {
            // Unsubscribe the Play button click handler.
            playButton.clicked -= OnPlayButtonClicked;
        }
        
        if (optionsButton != null)
        {
            // Unsubscribe the Options button click handler.
            optionsButton.clicked -= OnOptionsButtonClicked;
        }
    }
    
    // Runs when the Play button is clicked.
    void OnPlayButtonClicked()
    {
        // Prints a message to the Console for debugging.
        Debug.Log("Play button clicked!");
        // Loads the gameplay scene named "GameScene".
        SceneManager.LoadScene("GameScene");
    }
    
    // Runs when the Options button is clicked.
    void OnOptionsButtonClicked()
    {
        // Prints a message to the Console for debugging.
        Debug.Log("Options button clicked!");
        // Add options menu logic here later
    }
}