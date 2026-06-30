using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

// Controls the death screen UI and displays the player's game results
public class DeathScreenController : MonoBehaviour
{
    // References the UI document and root visual element
    private UIDocument uiDocument;
    private VisualElement root;
    
    // References labels that display player statistics
    private Label playerNameLabel;
    private Label waveLabel;
    private Label goldLabel;
    private Label highScoreLabel;
    private Label newHighScoreLabel;
    
    // References to the death screen buttons
    private Button playAgainButton;
    private Button quitButton;
    
    // Called before the first frame
    void Start()
    {
        // Get the UI Document attached to this GameObject
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        
        // Find UI elements by their names in the UI Builder
        playerNameLabel = root.Q<Label>("PlayerNameLabel");
        waveLabel = root.Q<Label>("WaveLabel");
        goldLabel = root.Q<Label>("GoldLabel");
        highScoreLabel = root.Q<Label>("HighScoreLabel");
        newHighScoreLabel = root.Q<Label>("NewHighScoreLabel");
        playAgainButton = root.Q<Button>("PlayAgainButton");
        quitButton = root.Q<Button>("QuitButton");
        
        // Register button click events
        playAgainButton?.RegisterCallback<ClickEvent>(evt => PlayAgain());
        quitButton?.RegisterCallback<ClickEvent>(evt => QuitToMenu());
        
        // Hide the death screen when the game starts
        HideDeathScreen();
    }
    
    // Displays the death screen and updates the player's statistics
    public void ShowDeathScreen(int waveReached, int goldEarned)
    {
        // Ensure a player is logged in before displaying results
        if (!PlayerManager.Instance.IsLoggedIn)
        {
            Debug.LogError("Player not logged in!");
            return;
        }
        
        // Display the player's username
        playerNameLabel.text = $"Player: {PlayerManager.Instance.CurrentPlayerName}";
        
        // Display the player's game results
        waveLabel.text = $"Wave Reached: {waveReached}";
        goldLabel.text = $"Gold Earned: {goldEarned}";
        
        // Store the player's previous high score
        int previousHighScore = PlayerManager.Instance.GetCurrentPlayerHighScore();
        
        // Update the player's saved statistics
        PlayerManager.Instance.UpdateGameStats(waveReached, goldEarned);
        
        // Get the updated high score and determine if a new record was achieved
        int newHighScore = PlayerManager.Instance.GetCurrentPlayerHighScore();
        bool isNewHighScore = (newHighScore > previousHighScore);
        
        // Update the UI depending on whether a new high score was achieved
        if (isNewHighScore)
        {
            highScoreLabel.text = $"NEW HIGH SCORE: {newHighScore}";
            highScoreLabel.style.color = new StyleColor(new Color(255, 215, 0)); // Gold colour
            newHighScoreLabel.text = "PERSONAL BEST!";
            newHighScoreLabel.style.display = DisplayStyle.Flex;
        }
        else
        {
            highScoreLabel.text = $"Best Wave: {newHighScore}";
            highScoreLabel.style.color = new StyleColor(new Color(100, 200, 255)); // Light blue colour
            newHighScoreLabel.style.display = DisplayStyle.None;
        }
        
        // Show the death screen
        root.style.display = DisplayStyle.Flex;
        Time.timeScale = 1f;
        
        // Output the game results to Unity Console
        Debug.Log($"Death Screen - Wave: {waveReached}, Gold: {goldEarned}, High Score: {newHighScore}");
    }
    
    // Hides the death screen
    void HideDeathScreen()
    {
        root.style.display = DisplayStyle.None;
    }
    
    // Restarts the game
    void PlayAgain()
    {
        // Ensure normal game speed before reloading the scene
        Time.timeScale = 1f;
        
        // Load the game scene
        SceneManager.LoadScene("GameScene");
    }
    
    // Returns the player to the login screen
    void QuitToMenu()
    {
        // Ensure normal game speed
        Time.timeScale = 1f;
        
        // Log out the current player
        PlayerManager.Instance.Logout();
        
        // Load the login scene
        SceneManager.LoadScene("LoginScene");
    }
}