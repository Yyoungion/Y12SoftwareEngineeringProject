using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    
    private Label waveLabel;
    private Label goldLabel;
    private Button playAgainButton;
    private Button quitButton;
    
    private bool isDeathScreenOpen = false;
    
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        
        // Find elements
        waveLabel = root.Q<Label>("WaveLabel");
        goldLabel = root.Q<Label>("GoldLabel");
        playAgainButton = root.Q<Button>("PlayAgainButton");
        quitButton = root.Q<Button>("QuitButton");
        
        // Register button clicks
        playAgainButton?.RegisterCallback<ClickEvent>(evt => PlayAgain());
        quitButton?.RegisterCallback<ClickEvent>(evt => QuitToMenu());
        
        // Start hidden
        HideDeathScreen();
    }
    
    public void ShowDeathScreen()
    {
        root.style.display = DisplayStyle.Flex;
        isDeathScreenOpen = true;
        
        // Update stats from managers
        UpdateDeathScreenInfo();
        
        Debug.Log("Death screen shown");
    }
    
    public void HideDeathScreen()
    {
        root.style.display = DisplayStyle.None;
        isDeathScreenOpen = false;
    }
    
    void UpdateDeathScreenInfo()
    {
        // Update wave info
        int currentWave = 0;
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        if (waveManager != null)
        {
            currentWave = waveManager.currentWave;
        }
        
        if (waveLabel != null)
        {
            waveLabel.text = $"Wave Survived: {currentWave}";
        }
        
        // Update gold earned
        int goldEarned = 0;
        if (CurrencyManager.Instance != null)
        {
            goldEarned = CurrencyManager.Instance.GetCurrency();
        }
        
        if (goldLabel != null)
        {
            goldLabel.text = $"Gold Earned: {goldEarned}";
        }
    }
    
    void PlayAgain()
    {
        Debug.Log("Restarting game...");
        
        // Reload the game scene
        SceneManager.LoadScene("GameScene");
    }
    
    void QuitToMenu()
    {
        Debug.Log("Returning to main menu...");
        
        // Make sure time is unpaused
        Time.timeScale = 1f;
        
        // Load main menu
        SceneManager.LoadScene("MainMenu");
    }
    
    void OnDestroy()
    {
        // Unpause if needed
        Time.timeScale = 1f;
    }
}