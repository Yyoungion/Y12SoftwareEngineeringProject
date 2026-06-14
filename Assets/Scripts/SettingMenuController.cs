using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuController : MonoBehaviour
{
    private const float DefaultVolumePercent = 50f;

    public static float MasterVolumePercent { get; private set; } = DefaultVolumePercent;
    public static float MusicVolumePercent { get; private set; } = DefaultVolumePercent;
    public static float SfxVolumePercent { get; private set; } = DefaultVolumePercent;

    private UIDocument uiDocument;
    private VisualElement root;
    private VisualElement overlay;
    
    // Volume sliders
    private Slider masterVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;
    
    // Difficulty buttons
    private Button easyButton;
    private Button normalButton;
    private Button hardButton;
    
    // Other controls
    private Button resetButton;
    private Button backButton;
    
    // Settings state
    private string currentDifficulty = "Normal";
    
    private bool isOpen = false;
    
    void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        overlay = root.Q<VisualElement>("Overlay") ?? root;
        
        // Find volume sliders
        masterVolumeSlider = root.Q<Slider>("MasterVolumeSlider");
        musicVolumeSlider = root.Q<Slider>("MusicVolumeSlider");
        sfxVolumeSlider = root.Q<Slider>("SFXVolumeSlider");
        
        // Find difficulty buttons
        easyButton = root.Q<Button>("EasyButton");
        normalButton = root.Q<Button>("NormalButton");
        hardButton = root.Q<Button>("HardButton");
        
        resetButton = root.Q<Button>("ResetButton");
        backButton = root.Q<Button>("BackButton");
        
        // Register slider events
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.RegisterValueChangedCallback(evt => OnMasterVolumeChanged(evt.newValue));
            masterVolumeSlider.SetValueWithoutNotify(MasterVolumePercent);
        }
        
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.RegisterValueChangedCallback(evt => OnMusicVolumeChanged(evt.newValue));
            musicVolumeSlider.SetValueWithoutNotify(MusicVolumePercent);
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.RegisterValueChangedCallback(evt => OnSFXVolumeChanged(evt.newValue));
            sfxVolumeSlider.SetValueWithoutNotify(SfxVolumePercent);
        }

        ApplyMasterVolume(MasterVolumePercent);
        ApplyMusicVolume(MusicVolumePercent);
        ApplySfxVolume(SfxVolumePercent);
        
        // Register difficulty buttons
        easyButton?.RegisterCallback<ClickEvent>(evt => SetDifficulty("Easy"));
        normalButton?.RegisterCallback<ClickEvent>(evt => SetDifficulty("Normal"));
        hardButton?.RegisterCallback<ClickEvent>(evt => SetDifficulty("Hard"));

        resetButton?.RegisterCallback<ClickEvent>(evt => ResetToDefault());
        backButton?.RegisterCallback<ClickEvent>(evt => CloseSettings());
        
        // Start hidden
        CloseSettings();
    }

    void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSettings();
        }
    }
    
    void OnMasterVolumeChanged(float value)
    {
        MasterVolumePercent = Mathf.Clamp(value, 0f, 100f);
        ApplyMasterVolume(MasterVolumePercent);
    }
    
    void OnMusicVolumeChanged(float value)
    {
        MusicVolumePercent = Mathf.Clamp(value, 0f, 100f);
        ApplyMusicVolume(MusicVolumePercent);
    }
    
    void OnSFXVolumeChanged(float value)
    {
        SfxVolumePercent = Mathf.Clamp(value, 0f, 100f);
        ApplySfxVolume(SfxVolumePercent);
    }

    private static float PercentToNormalized(float percent)
    {
        return Mathf.Clamp01(percent / 100f);
    }

    private void ApplyMasterVolume(float percent)
    {
        MasterVolumePercent = Mathf.Clamp(percent, 0f, 100f);
    }

    private void ApplyMusicVolume(float percent)
    {
        MusicVolumePercent = Mathf.Clamp(percent, 0f, 100f);
    }

    private void ApplySfxVolume(float percent)
    {
        SfxVolumePercent = Mathf.Clamp(percent, 0f, 100f);
    }
    
    void SetDifficulty(string difficulty)
    {
        currentDifficulty = difficulty;
        
        // Update button colors to show selection
        UpdateDifficultyButtons();
        
        Debug.Log($"Difficulty set to: {difficulty}");
        
        // TODO: Apply difficulty changes to game
        // - Increase enemy health/damage on Hard
        // - Decrease on Easy
        // - Update spawn rates, etc.
    }
    
    void UpdateDifficultyButtons()
    {
        // Reset all to normal color
        easyButton.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
        normalButton.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
        hardButton.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
        
        // Highlight selected
        switch (currentDifficulty)
        {
            case "Easy":
                easyButton.style.backgroundColor = new Color(0.2f, 0.8f, 0.2f);
                break;
            case "Normal":
                normalButton.style.backgroundColor = new Color(0.2f, 0.8f, 0.2f);
                break;
            case "Hard":
                hardButton.style.backgroundColor = new Color(0.8f, 0.2f, 0.2f);
                break;
        }
    }
    
    void ResetToDefault()
    {
        // Reset volumes
        if (masterVolumeSlider != null) masterVolumeSlider.SetValueWithoutNotify(DefaultVolumePercent);
        if (musicVolumeSlider != null) musicVolumeSlider.SetValueWithoutNotify(DefaultVolumePercent);
        if (sfxVolumeSlider != null) sfxVolumeSlider.SetValueWithoutNotify(DefaultVolumePercent);

        MasterVolumePercent = DefaultVolumePercent;
        MusicVolumePercent = DefaultVolumePercent;
        SfxVolumePercent = DefaultVolumePercent;

        ApplyMasterVolume(MasterVolumePercent);
        ApplyMusicVolume(MusicVolumePercent);
        ApplySfxVolume(SfxVolumePercent);
        
        // Reset difficulty
        SetDifficulty("Normal");
        
        Debug.Log("Settings reset to default");
    }
    
    public void OpenSettings()
    {
        overlay.style.display = DisplayStyle.Flex;
        isOpen = true;
        
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Pause();
        }
        else
        {
            Time.timeScale = 0f;
        }
        
        UpdateDifficultyButtons();
        Debug.Log("Settings opened");
    }
    
    public void CloseSettings()
    {
        overlay.style.display = DisplayStyle.None;
        isOpen = false;
        
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Resume();
        }
        else
        {
            Time.timeScale = 1f;
        }
        
        Debug.Log("Settings closed");
    }
}