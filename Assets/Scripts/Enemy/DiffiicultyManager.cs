using UnityEngine; // Imports the Unity engine.

public class DifficultyManager : MonoBehaviour // Defines the DifficultyManager class.
{
    // Singleton instance so other scripts can access.
    public static DifficultyManager Instance { get; private set; }
    
    // Enumeration fore difficulty levels.
    public enum Difficulty { Easy, Normal, Hard }

    // Stores the currently selected difficulty. Defaults to Normal.
    public Difficulty currentDifficulty = Difficulty.Normal;
    
    [Header("Easy Settings")] // Groups Easy difficulty settings in the Inspector.
    public float easyEnemyHealthMultiplier = 0.7f;      // Multiplies enemy health on Easy.
    public float easyEnemyDamageMultiplier = 0.6f;      // Multiplies enemy damage on Easy.
    public float easyEnemiesPerWaveMultiplier = 1.2f;   // Multiplies the number of enemies per wave on Easy.
    
    [Header("Normal Settings")] // Groups Normal difficulty settings in the Inspector.
    public float normalEnemyHealthMultiplier = 1f;      // Enemy health multiplier for Normal.
    public float normalEnemyDamageMultiplier = 1f;      // Enemy damage multiplier for Normal.
    public float normalEnemiesPerWaveMultiplier = 1.5f; // Enemies per wave multiplier for Normal.
    
    [Header("Hard Settings")] // Groups Hard difficulty settings in the Inspector.
    public float hardEnemyHealthMultiplier = 1.5f;      // Enemy health multiplier for Hard.
    public float hardEnemyDamageMultiplier = 1.4f;      // Enemy damage multiplier for Hard.
    public float hardEnemiesPerWaveMultiplier = 2f;     // Enemies per wave multiplier for Hard.
    
    // Called when the object is created.
    void Awake()
    {
        // Check if this is the first DifficultyManager.
        if (Instance == null)
        {
            // Assign this object as the singleton instance.
            Instance = this;

            // Keep this object when changing scenes.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy duplicate DifficultyManager objects.
            Destroy(gameObject);
        }
    }
    
    // Changes the current game difficulty.
    public void SetDifficulty(Difficulty difficulty)
    {
        // Store the selected difficulty.
        currentDifficulty = difficulty;

        // Display the selected difficulty in the Console.
        Debug.Log($"Difficulty set to: {difficulty}");

        // Apply the new difficulty settings to enemies and wave manager.
        ApplyDifficultySettings();
    }
    
    // Applies the selected difficulty settings to all enemies and the wave manager.
    void ApplyDifficultySettings()
    {
        // Find every SlimeEnemy currently in the scene.
        SlimeEnemy[] allSlimes = FindObjectsOfType<SlimeEnemy>();

        // Update each slime with the new difficulty values.
        foreach (SlimeEnemy slime in allSlimes)
        {
            UpdateEnemyForDifficulty(slime);
        }
        
        // Find the WaveManager.
        WaveManager waveManager = FindObjectOfType<WaveManager>();

        // Update its enemies-per-wave multiplier.
        if (waveManager != null)
        {
            waveManager.enemiesPerWaveMultiplier = GetEnemiesPerWaveMultiplier();
        }
    }
    
    // Updates an individual enemy's stats based on the current difficulty.
    public void UpdateEnemyForDifficulty(SlimeEnemy enemy)
    {
        // Get the appropriate health and damage multipliers.
        float healthMult = GetEnemyHealthMultiplier();
        float damageMult = GetEnemyDamageMultiplier();
        
        // Scale the enemy's maximum health.
        enemy.maxHealth *= healthMult;

        // Scale the enemy's current health.
        enemy.health *= healthMult;

        // Scale the enemy's attack damage.
        enemy.attackDamage *= damageMult;
    }
    
    // Returns the health multiplier based on the selected difficulty.
    public float GetEnemyHealthMultiplier()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => easyEnemyHealthMultiplier,
            Difficulty.Normal => normalEnemyHealthMultiplier,
            Difficulty.Hard => hardEnemyHealthMultiplier,
            _ => 1f // Default value if something goes wrong.
        };
    }
    
    // Returns the damage multiplier based on the selected difficulty.
    public float GetEnemyDamageMultiplier()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => easyEnemyDamageMultiplier,
            Difficulty.Normal => normalEnemyDamageMultiplier,
            Difficulty.Hard => hardEnemyDamageMultiplier,
            _ => 1f // Default value if something goes wrong.
        };
    }
    
    // Returns the enemies-per-wave multiplier based on the selected difficulty.
    public float GetEnemiesPerWaveMultiplier()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => easyEnemiesPerWaveMultiplier,
            Difficulty.Normal => normalEnemiesPerWaveMultiplier,
            Difficulty.Hard => hardEnemiesPerWaveMultiplier,
            _ => 1.5f // Default value if something goes wrong.
        };
    }
}