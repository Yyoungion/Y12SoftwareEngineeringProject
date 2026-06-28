using UnityEngine;
using UnityEngine.UIElements;

public class CurrencyUIController : MonoBehaviour
{
    private UIDocument uiDocument;
    private Label currencyLabel;
    
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        
        currencyLabel = root.Q<Label>("CurrencyLabel");
        
        
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged += UpdateCurrencyDisplay;
            UpdateCurrencyDisplay(CurrencyManager.Instance.GetCurrency());
        }
    }
    
    void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateCurrencyDisplay;
        }
    }
    
    void UpdateCurrencyDisplay(int amount)
    {
        if (currencyLabel != null)
        {
            currencyLabel.text = $"Gold: {amount}";
        }
    }
}