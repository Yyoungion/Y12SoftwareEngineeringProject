using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    
    [Header("Currency")]
    public int currentCurrency = 0;
    
    
    public event Action<int> OnCurrencyChanged;
    
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
    
    public int GetCurrency()
    {
        return currentCurrency;
    }
}