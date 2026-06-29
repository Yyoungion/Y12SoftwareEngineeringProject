using UnityEngine;      // Imports the Unity engine 
using System;           // Imports the System

public class CurrencyManager : MonoBehaviour // Defines the CurrencyManager class, inheriting from MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    
    [Header("Currency")] // Creates a "Currency" header
    public int currentCurrency = 0; // Stores the player's currency.
    
    // Event that is triggered whenever the currency amount changes.
    public event Action<int> OnCurrencyChanged;
    
    // Called automatically by Unity before Start() when the object is created.
    void Awake()
    {
        // Check if this is the first CurrencyManager instance.
        if (Instance == null)
        {
            // Set this object as the singleton instance.
            Instance = this;

            // Keep this GameObject when changing scenes.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another CurrencyManager already exists, destroy this duplicate.
            Destroy(gameObject);
        }
    }
    
    // Adds the amount of currency to the player's total.
    public void AddCurrency(int amount)
    {
        // Increase the current currency.
        currentCurrency += amount;

        // Display a message in the Unity Console.
        Debug.Log($"Gained {amount} currency! Total: {currentCurrency}");

        // Notify all scripts that the currency has changed.
        OnCurrencyChanged?.Invoke(currentCurrency);
    }
    
    // Attempts to spend the specified amount of currency.
    // Returns true if successful, false if there isn't enough currency.
    public bool SpendCurrency(int amount)
    {
        // Check if the player has enough currency.
        if (currentCurrency >= amount)
        {
            // Subtract the spent amount.
            currentCurrency -= amount;

            // Display message in the Console.
            Debug.Log($"Spent {amount} currency! Remaining: {currentCurrency}");

            // Notify that the currency has changed.
            OnCurrencyChanged?.Invoke(currentCurrency);

            return true;
        }
        else
        {
            // Display an error message if there isn't enough currency.
            Debug.Log("Not enough currency!");

            return false;
        }
    }
    
    // Returns the player's current currency amount.
    public int GetCurrency()
    {
        // Return the current currency value.
        return currentCurrency;
    }
}