using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    
    public bool isPaused = false;
    
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
    
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Debug.Log("Game paused");
    }
    
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        Debug.Log("Game resumed");
    }
    
    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }
}