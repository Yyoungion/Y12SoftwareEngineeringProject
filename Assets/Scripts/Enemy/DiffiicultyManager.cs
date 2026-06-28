using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }
    
    public enum Difficulty { Easy, Normal, Hard }
    public Difficulty currentDifficulty = Difficulty.Normal;
    
    [Header("Easy Settings")]
    public float easyEnemyHealthMultiplier = 0.7f;
    public float easyEnemyDamageMultiplier = 0.6f;
    public float easyEnemiesPerWaveMultiplier = 1.2f;
    
    [Header("Normal Settings")]
    public float normalEnemyHealthMultiplier = 1f;
    public float normalEnemyDamageMultiplier = 1f;
    public float normalEnemiesPerWaveMultiplier = 1.5f;
    
    [Header("Hard Settings")]
    public float hardEnemyHealthMultiplier = 1.5f;
    public float hardEnemyDamageMultiplier = 1.4f;
    public float hardEnemiesPerWaveMultiplier = 2f;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;
        Debug.Log($"Difficulty set to: {difficulty}");
        ApplyDifficultySettings();
    }
    
    void ApplyDifficultySettings()
    {
        
        SlimeEnemy[] allSlimes = FindObjectsOfType<SlimeEnemy>();
        foreach (SlimeEnemy slime in allSlimes)
        {
            UpdateEnemyForDifficulty(slime);
        }
        
        
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.enemiesPerWaveMultiplier = GetEnemiesPerWaveMultiplier();
        }
    }
    
    public void UpdateEnemyForDifficulty(SlimeEnemy enemy)
    {
        float healthMult = GetEnemyHealthMultiplier();
        float damageMult = GetEnemyDamageMultiplier();
        
        
        enemy.maxHealth *= healthMult;
        enemy.health *= healthMult;
        enemy.attackDamage *= damageMult;
    }
    
    public float GetEnemyHealthMultiplier()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => easyEnemyHealthMultiplier,
            Difficulty.Normal => normalEnemyHealthMultiplier,
            Difficulty.Hard => hardEnemyHealthMultiplier,
            _ => 1f
        };
    }
    
    public float GetEnemyDamageMultiplier()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => easyEnemyDamageMultiplier,
            Difficulty.Normal => normalEnemyDamageMultiplier,
            Difficulty.Hard => hardEnemyDamageMultiplier,
            _ => 1f
        };
    }
    
    public float GetEnemiesPerWaveMultiplier()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => easyEnemiesPerWaveMultiplier,
            Difficulty.Normal => normalEnemiesPerWaveMultiplier,
            Difficulty.Hard => hardEnemiesPerWaveMultiplier,
            _ => 1.5f
        };
    }
}