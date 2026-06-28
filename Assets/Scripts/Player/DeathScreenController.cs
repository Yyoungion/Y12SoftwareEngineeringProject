using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    
    private Label playerNameLabel;
    private Label waveLabel;
    private Label goldLabel;
    private Label highScoreLabel;
    private Label newHighScoreLabel;
    private Button playAgainButton;
    private Button quitButton;
    
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        
        playerNameLabel = root.Q<Label>("PlayerNameLabel");
        waveLabel = root.Q<Label>("WaveLabel");
        goldLabel = root.Q<Label>("GoldLabel");
        highScoreLabel = root.Q<Label>("HighScoreLabel");
        newHighScoreLabel = root.Q<Label>("NewHighScoreLabel");
        playAgainButton = root.Q<Button>("PlayAgainButton");
        quitButton = root.Q<Button>("QuitButton");
        
        playAgainButton?.RegisterCallback<ClickEvent>(evt => PlayAgain());
        quitButton?.RegisterCallback<ClickEvent>(evt => QuitToMenu());
        
        
        HideDeathScreen();
    }
    
    public void ShowDeathScreen(int waveReached, int goldEarned)
    {
        if (!PlayerManager.Instance.IsLoggedIn)
        {
            Debug.LogError("Player not logged in!");
            return;
        }
        
        
        playerNameLabel.text = $"Player: {PlayerManager.Instance.CurrentPlayerName}";
        
        
        waveLabel.text = $"Wave Reached: {waveReached}";
        goldLabel.text = $"Gold Earned: {goldEarned}";
        
        
        int previousHighScore = PlayerManager.Instance.GetCurrentPlayerHighScore();
        
        
        PlayerManager.Instance.UpdateGameStats(waveReached, goldEarned);
        
        
        int newHighScore = PlayerManager.Instance.GetCurrentPlayerHighScore();
        bool isNewHighScore = (newHighScore > previousHighScore);
        
        
        if (isNewHighScore)
        {
            highScoreLabel.text = $"NEW HIGH SCORE: {newHighScore}";
            highScoreLabel.style.color = new StyleColor(new Color(255, 215, 0)); 
            newHighScoreLabel.text = "🏆 PERSONAL BEST! 🏆";
            newHighScoreLabel.style.display = DisplayStyle.Flex;
        }
        else
        {
            highScoreLabel.text = $"Best Wave: {newHighScore}";
            highScoreLabel.style.color = new StyleColor(new Color(100, 200, 255)); 
            newHighScoreLabel.style.display = DisplayStyle.None;
        }
        
        
        root.style.display = DisplayStyle.Flex;
        Time.timeScale = 1f; 
        
        Debug.Log($"Death Screen - Wave: {waveReached}, Gold: {goldEarned}, High Score: {newHighScore}");
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
        PlayerManager.Instance.Logout();
        SceneManager.LoadScene("LoginScene");
    }
}