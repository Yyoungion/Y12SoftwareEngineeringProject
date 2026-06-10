using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuController : MonoBehaviour
{
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
            masterVolumeSlider.value = GetAudioManagerFloat("masterVolume", 1f);
        }
        
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.RegisterValueChangedCallback(evt => OnMusicVolumeChanged(evt.newValue));
            musicVolumeSlider.value = GetAudioManagerFloat("musicVolume", 0.7f);
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.RegisterValueChangedCallback(evt => OnSFXVolumeChanged(evt.newValue));
            sfxVolumeSlider.value = GetAudioManagerFloat("sfxVolume", 0.8f);
        }
        
        // Register difficulty buttons
        easyButton?.RegisterCallback<ClickEvent>(evt => SetDifficulty("Easy"));
        normalButton?.RegisterCallback<ClickEvent>(evt => SetDifficulty("Normal"));
        hardButton?.RegisterCallback<ClickEvent>(evt => SetDifficulty("Hard"));

        resetButton?.RegisterCallback<ClickEvent>(evt => ResetToDefault());
        backButton?.RegisterCallback<ClickEvent>(evt => CloseSettings());
        
        // Start hidden
        CloseSettings();
    }
    
    void OnMasterVolumeChanged(float value)
    {
        InvokeAudioManagerMethod("SetMasterVolume", value);
    }
    
    void OnMusicVolumeChanged(float value)
    {
        InvokeAudioManagerMethod("SetMusicVolume", value);
    }
    
    void OnSFXVolumeChanged(float value)
    {
        InvokeAudioManagerMethod("SetSFXVolume", value);
    }

    private float GetAudioManagerFloat(string propertyName, float fallbackValue)
    {
        object audioManager = GetAudioManagerInstance();
        if (audioManager == null)
        {
            return fallbackValue;
        }

        PropertyInfo propertyInfo = audioManager.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
        if (propertyInfo != null && propertyInfo.PropertyType == typeof(float))
        {
            object propertyValue = propertyInfo.GetValue(audioManager);
            if (propertyValue is float floatValue)
            {
                return floatValue;
            }
        }

        return fallbackValue;
    }

    private void InvokeAudioManagerMethod(string methodName, float value)
    {
        object audioManager = GetAudioManagerInstance();
        if (audioManager == null)
        {
            return;
        }

        MethodInfo methodInfo = audioManager.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
        methodInfo?.Invoke(audioManager, new object[] { value });
    }

    private object GetAudioManagerInstance()
    {
        Type audioManagerType = FindTypeByName("AudioManager");
        if (audioManagerType == null)
        {
            return null;
        }

        PropertyInfo instanceProperty = audioManagerType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
        return instanceProperty?.GetValue(null);
    }

    private Type FindTypeByName(string typeName)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type type = assembly.GetType(typeName);
            if (type != null)
            {
                return type;
            }

            Type[] assemblyTypes;
            try
            {
                assemblyTypes = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                assemblyTypes = exception.Types;
            }

            foreach (Type candidate in assemblyTypes)
            {
                if (candidate != null && candidate.Name == typeName)
                {
                    return candidate;
                }
            }
        }

        return null;
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
        if (masterVolumeSlider != null) masterVolumeSlider.value = 1f;
        if (musicVolumeSlider != null) musicVolumeSlider.value = 0.7f;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = 0.8f;
        
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