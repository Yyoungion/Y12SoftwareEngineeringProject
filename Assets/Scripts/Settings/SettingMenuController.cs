using UnityEngine;
using UnityEngine.UIElements;

// Controls the settings menu, including volume controls and difficulty selection
public class SettingsMenuController : MonoBehaviour
{
    // Default volume percentage for all audio settings
    private const float DefaultVolumePercent = 50f;

    // Stores the current volume settings
    public static float MasterVolumePercent { get; private set; } = DefaultVolumePercent;
    public static float MusicVolumePercent { get; private set; } = DefaultVolumePercent;
    public static float SfxVolumePercent { get; private set; } = DefaultVolumePercent;

    // References to the UI elements
    private UIDocument uiDocument;
    private VisualElement root;
    private VisualElement overlay;

    // Volume sliders
    private Slider masterVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;

    // Difficulty selection buttons
    private Button easyButton;
    private Button normalButton;
    private Button hardButton;

    // Other menu buttons
    private Button resetButton;
    private Button backButton;

    // Stores the currently selected difficulty
    private string currentDifficulty = "Normal";

    // Tracks whether the settings menu is open
    private bool isOpen = false;

    // Called when the object is created
    void Awake()
    {
        // Get the UI document and root element
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        overlay = root.Q<VisualElement>("Overlay") ?? root;

        // Find the volume sliders
        masterVolumeSlider = root.Q<Slider>("MasterVolumeSlider");
        musicVolumeSlider = root.Q<Slider>("MusicVolumeSlider");
        sfxVolumeSlider = root.Q<Slider>("SFXVolumeSlider");

        // Find the difficulty buttons
        easyButton = root.Q<Button>("EasyButton");
        normalButton = root.Q<Button>("NormalButton");
        hardButton = root.Q<Button>("HardButton");

        // Find the reset and back buttons
        resetButton = root.Q<Button>("ResetButton");
        backButton = root.Q<Button>("BackButton");

        // Register the master volume slider
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.RegisterValueChangedCallback(evt => OnMasterVolumeChanged(evt.newValue));
            masterVolumeSlider.SetValueWithoutNotify(MasterVolumePercent);
        }

        // Register the music volume slider
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.RegisterValueChangedCallback(evt => OnMusicVolumeChanged(evt.newValue));
            musicVolumeSlider.SetValueWithoutNotify(MusicVolumePercent);
        }

        // Register the sound effects volume slider
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.RegisterValueChangedCallback(evt => OnSFXVolumeChanged(evt.newValue));
            sfxVolumeSlider.SetValueWithoutNotify(SfxVolumePercent);
        }

        // Apply the volume settings
        ApplyMasterVolume(MasterVolumePercent);
        ApplyMusicVolume(MusicVolumePercent);
        ApplySfxVolume(SfxVolumePercent);

        // Register button click events
        easyButton?.RegisterCallback<ClickEvent>(evt => SetDifficulty("Easy"));
        normalButton?.RegisterCallback<ClickEvent>(evt => SetDifficulty("Normal"));
        hardButton?.RegisterCallback<ClickEvent>(evt => SetDifficulty("Hard"));

        resetButton?.RegisterCallback<ClickEvent>(evt => ResetToDefault());
        backButton?.RegisterCallback<ClickEvent>(evt => CloseSettings());

        // Hide the settings menu when the game starts
        CloseSettings();
    }

    // Called once every frame
    void Update()
    {
        // Close the settings menu when Escape is pressed
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSettings();
        }
    }

    // Updates the master volume
    void OnMasterVolumeChanged(float value)
    {
        MasterVolumePercent = Mathf.Clamp(value, 0f, 100f);
        ApplyMasterVolume(MasterVolumePercent);
    }

    // Updates the music volume
    void OnMusicVolumeChanged(float value)
    {
        MusicVolumePercent = Mathf.Clamp(value, 0f, 100f);
        ApplyMusicVolume(MusicVolumePercent);
    }

    // Updates the sound effects volume
    void OnSFXVolumeChanged(float value)
    {
        SfxVolumePercent = Mathf.Clamp(value, 0f, 100f);
        ApplySfxVolume(SfxVolumePercent);
    }

    // Converts a percentage into a value between 0 and 1
    private static float PercentToNormalized(float percent)
    {
        return Mathf.Clamp01(percent / 100f);
    }

    // Applies the master volume setting
    private void ApplyMasterVolume(float percent)
    {
        MasterVolumePercent = Mathf.Clamp(percent, 0f, 100f);
    }

    // Applies the music volume setting
    private void ApplyMusicVolume(float percent)
    {
        MusicVolumePercent = Mathf.Clamp(percent, 0f, 100f);
    }

    // Applies the sound effects volume setting
    private void ApplySfxVolume(float percent)
    {
        SfxVolumePercent = Mathf.Clamp(percent, 0f, 100f);
    }

    // Changes the game difficulty
    void SetDifficulty(string difficulty)
    {
        currentDifficulty = difficulty;

        // Update the appearance of the difficulty buttons
        UpdateDifficultyButtons();

        Debug.Log($"Difficulty set to: {difficulty}");
    }

    // Updates the button colours to show the selected difficulty
    void UpdateDifficultyButtons()
    {
        // Reset all buttons to their default colour
        easyButton.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
        normalButton.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
        hardButton.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f);

        // Highlight the selected difficulty
        switch (currentDifficulty)
        {
            case "Easy":
                easyButton.style.backgroundColor = new Color(0.2f, 0.8f, 0.2f);
                break;

            case "Normal":
                normalButton.style.backgroundColor = new Color(1f, 0.55f, 0.1f);
                break;

            case "Hard":
                hardButton.style.backgroundColor = new Color(0.8f, 0.2f, 0.2f);
                break;
        }
    }

    // Restores all settings to their default values
    void ResetToDefault()
    {
        // Reset the sliders
        if (masterVolumeSlider != null)
            masterVolumeSlider.SetValueWithoutNotify(DefaultVolumePercent);

        if (musicVolumeSlider != null)
            musicVolumeSlider.SetValueWithoutNotify(DefaultVolumePercent);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.SetValueWithoutNotify(DefaultVolumePercent);

        // Reset the stored values
        MasterVolumePercent = DefaultVolumePercent;
        MusicVolumePercent = DefaultVolumePercent;
        SfxVolumePercent = DefaultVolumePercent;

        // Apply the default settings
        ApplyMasterVolume(MasterVolumePercent);
        ApplyMusicVolume(MusicVolumePercent);
        ApplySfxVolume(SfxVolumePercent);

        // Reset the difficulty
        SetDifficulty("Normal");

        Debug.Log("Settings reset to default");
    }

    // Opens the settings menu
    public void OpenSettings()
    {
        // Display the menu
        overlay.style.display = DisplayStyle.Flex;
        isOpen = true;

        // Pause the game
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Pause();
        }
        else
        {
            Time.timeScale = 0f;
        }

        // Update the difficulty buttons
        UpdateDifficultyButtons();

        Debug.Log("Settings opened");
    }

    // Closes the settings menu
    public void CloseSettings()
    {
        // Hide the menu
        overlay.style.display = DisplayStyle.None;
        isOpen = false;

        // Resume the game
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