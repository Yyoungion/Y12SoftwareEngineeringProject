using UnityEngine;                 // Imports the Unity engine.
using UnityEngine.UIElements;      // Imports the UI Toolkit

public class CurrencyUIController : MonoBehaviour // Defines the CurrencyUIController class.
{
    // Reference to the UI Document component.
    private UIDocument uiDocument;

    // Reference to the label that displays the player's currency.
    private Label currencyLabel;
    
    // Called automatically by Unity
    void Start()
    {
        // Gets the UIDocument component.
        uiDocument = GetComponent<UIDocument>();

        // Gets the root visual element.
        var root = uiDocument.rootVisualElement;
        
        // Finds the Label named "CurrencyLabel".
        currencyLabel = root.Q<Label>("CurrencyLabel");
        
        // Check if the CurrencyManager exists.
        if (CurrencyManager.Instance != null)
        {
            //Currency changed event.
            CurrencyManager.Instance.OnCurrencyChanged += UpdateCurrencyDisplay;

            // Display the current currency.
            UpdateCurrencyDisplay(CurrencyManager.Instance.GetCurrency());
        }
    }
    
    // Called automatically when this GameObject is destroyed.
    void OnDestroy()
    {
        // Check if the CurrencyManager still exists.
        if (CurrencyManager.Instance != null)
        {
            // Prevent memory leaks and errors.
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateCurrencyDisplay;
        }
    }
    
    // Updates the currency label whenever the player's currency changes.
    void UpdateCurrencyDisplay(int amount)
    {
        // Check that the label exists before trying to modify it.
        if (currencyLabel != null)
        {
            // Display the player's current amount of gold.
            currencyLabel.text = $"Gold: {amount}";
        }
    }
}