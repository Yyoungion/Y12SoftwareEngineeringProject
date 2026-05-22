using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    
    [Header("Currency")]
    public int currentCurrency = 0;
    
    [Header("Experience/Level")]
    public int currentXP = 0;
    public int currentLevel = 1;
    public int xpToNextLevel = 100;
    public float xpMultiplier = 1.5f; // XP required increases by this each level
    
    // Events so UI can update
    public event Action<int> OnCurrencyChanged;
    public event Action<int, int> OnXPChanged; // current, max
    public event Action<int> OnLevelUp;
    
    void Awake()
    {
        // Singleton pattern
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
    
    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        Debug.Log($"Gained {amount} currency! Total: {currentCurrency}");
        OnCurrencyChanged?.Invoke(currentCurrency);
    }
    
    public bool SpendCurrency(int amount)
    {
        if (currentCurrency >= amount)
        {
            currentCurrency -= amount;
            Debug.Log($"Spent {amount} currency! Remaining: {currentCurrency}");
            OnCurrencyChanged?.Invoke(currentCurrency);
            return true;
        }
        else
        {
            Debug.Log("Not enough currency!");
            return false;
        }
    }
    
    public void AddXP(int amount)
    {
        currentXP += amount;
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
        
        Debug.Log($"Gained {amount} XP! Current: {currentXP}/{xpToNextLevel}");
        
        // Check for level up
        while (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }
    
    void LevelUp()
    {
        currentXP -= xpToNextLevel;
        currentLevel++;
        
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpMultiplier);
        
        Debug.Log($"LEVEL UP! Now level {currentLevel}");
        
        // Grant stat increases on level up
        if (PlayerController.Instance != null)
        {
            // Free upgrade on level up (you choose which stat)
            // Option 1: Random stat
            int randomStat = UnityEngine.Random.Range(0, 4);
            switch (randomStat)
            {
                case 0:
                    PlayerController.Instance.damageUpgradeLevel++;
                    Debug.Log("Damage increased!");
                    break;
                case 1:
                    PlayerController.Instance.rangeUpgradeLevel++;
                    Debug.Log("Range increased!");
                    break;
                case 2:
                    PlayerController.Instance.speedUpgradeLevel++;
                    Debug.Log("Speed increased!");
                    break;
                case 3:
                    PlayerController.Instance.attackSpeedUpgradeLevel++;
                    Debug.Log("Attack speed increased!");
                    break;
            }
            
            PlayerController.Instance.RecalculateStats();
        }
        
        OnLevelUp?.Invoke(currentLevel);
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
    }
    
    public int GetCurrency()
    {
        return currentCurrency;
    }
    
    public int GetLevel()
    {
        return currentLevel;
    }
}