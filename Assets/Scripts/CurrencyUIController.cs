using UnityEngine;
using UnityEngine.UIElements;

public class CurrencyUIController : MonoBehaviour
{
    private UIDocument uiDocument;
    private Label CurrencyLabel;
    private Label LevelLabel;
    private VisualElement XPBarFill;
    
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        
        // Find elements
        CurrencyLabel = root.Q<Label>("CurrencyLabel");
        LevelLabel = root.Q<Label>("LevelLabel");
        XPBarFill = root.Q<VisualElement>("XPBarFill");

        // Subscribe to currency manager events
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged += UpdateCurrencyDisplay;
            CurrencyManager.Instance.OnXPChanged += UpdateXPDisplay;
            CurrencyManager.Instance.OnLevelUp += UpdateLevelDisplay;
            
            // Initial update
            UpdateCurrencyDisplay(CurrencyManager.Instance.GetCurrency());
            UpdateLevelDisplay(CurrencyManager.Instance.GetLevel());
            UpdateXPDisplay(CurrencyManager.Instance.currentXP, CurrencyManager.Instance.xpToNextLevel);
        }
    }
    
    void OnDestroy()
    {
        // Unsubscribe
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateCurrencyDisplay;
            CurrencyManager.Instance.OnXPChanged -= UpdateXPDisplay;
            CurrencyManager.Instance.OnLevelUp -= UpdateLevelDisplay;
        }
    }
    
    void UpdateCurrencyDisplay(int amount)
    {
        if (CurrencyLabel != null)
        {
            CurrencyLabel.text = $"Gold: {amount}";
        }
    }
    
    void UpdateLevelDisplay(int level)
    {
        if (LevelLabel != null)
        {
            LevelLabel.text = $"Level: {level}";
        }
    }
    
    void UpdateXPDisplay(int currentXP, int maxXP)
    {
        if (XPBarFill != null)
        {
            float percentage = (float)currentXP / maxXP;
            XPBarFill.style.width = Length.Percent(percentage * 100);
        }
    }
}