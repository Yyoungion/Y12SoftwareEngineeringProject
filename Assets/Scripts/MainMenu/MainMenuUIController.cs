
using UnityEngine;

using UnityEngine.UIElements;

using UnityEngine.SceneManagement;


public class MainMenuUIController : MonoBehaviour
{
    
    private UIDocument uiDocument;
    
    private Button playButton;
    
    private Button optionsButton;

    
    void OnEnable()
    {
        
        uiDocument = GetComponent<UIDocument>();
        
        
        var rootVisualElement = uiDocument.rootVisualElement;
        
        
        playButton = rootVisualElement.Q<Button>("PlayButton");
        optionsButton = rootVisualElement.Q<Button>("OptionsButton");
        
        
        if (playButton != null)
        {
            
            playButton.clicked += OnPlayButtonClicked;
        }
        
        if (optionsButton != null)
        {
            
            optionsButton.clicked += OnOptionsButtonClicked;
        }
    }
    
    
    void OnDisable()
    {
        
        if (playButton != null)
        {
            
            playButton.clicked -= OnPlayButtonClicked;
        }
        
        if (optionsButton != null)
        {
            
            optionsButton.clicked -= OnOptionsButtonClicked;
        }
    }
    
    
    void OnPlayButtonClicked()
    {
        
        Debug.Log("Play button clicked!");
        
        SceneManager.LoadScene("GameScene");
    }
    
    
    void OnOptionsButtonClicked()
    {
        
        Debug.Log("Options button clicked!");

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
}