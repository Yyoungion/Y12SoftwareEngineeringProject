using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    private Button playAgainButton;
    private Button quitButton;
    
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        
        playAgainButton = root.Q<Button>("PlayAgainButton");
        quitButton = root.Q<Button>("QuitButton");
        
        playAgainButton?.RegisterCallback<ClickEvent>(evt => PlayAgain());
        quitButton?.RegisterCallback<ClickEvent>(evt => QuitToMenu());
        
        // Hide death screen at start
        HideDeathScreen();
    }
    
    public void ShowDeathScreen()
    {
        root.style.display = DisplayStyle.Flex;
        Time.timeScale = 1f; // Unpause so UI works
        Debug.Log("Death screen shown");
    }
    
    void HideDeathScreen()
    {
        root.style.display = DisplayStyle.None;
    }
    
    void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }
    
    void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}