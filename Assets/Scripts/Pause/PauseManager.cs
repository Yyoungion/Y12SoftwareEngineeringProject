using UnityEngine;

// Controls the game's pause state
public class PauseManager : MonoBehaviour
{
    // Reference so other scripts can access the PauseManager
    public static PauseManager Instance { get; private set; }
    
    // Tracks whether the game is currently paused
    public bool isPaused = false;
    
    // Called when the object is first created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Pauses the game
    public void Pause()
    {
        // Update pause state
        isPaused = true;
        
        // Stop all time-based gameplay
        Time.timeScale = 0f;
        
        // Output a message to the Unity Console
        Debug.Log("Game paused");
    }
    
    // Resumes the game
    public void Resume()
    {
        // Update pause state
        isPaused = false;
        
        // Restore normal game speed
        Time.timeScale = 1f;
        
        // Output a message to the Unity Console
        Debug.Log("Game resumed");
    }
    
    // Switches between paused and unpaused states
    public void TogglePause()
    {
        // If already paused, resume the game
        if (isPaused)
            Resume();
        // Otherwise, pause the game
        else
            Pause();
    }
}