using UnityEngine;
using UnityEngine.UIElements;

public class UpgradeMenuController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    
    private Label currentGoldLabel;
    
    // Upgrade buttons
    private Button damageUpgradeButton;
    private Button attackSpeedUpgradeButton;
    private Button speedUpgradeButton;
    private Button defenseUpgradeButton;
    private Button healthUpgradeButton;
    private Button coinMultiplierUpgradeButton;
    private Button closeButton;
    
    // Description labels
    private Label damageDescription;
    private Label attackSpeedDescription;
    private Label speedDescription;
    private Label defenseDescription;
    private Label healthDescription;
    private Label coinMultiplierDescription;
    
    private bool isMenuOpen = false;
    
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        
        // Find elements
        currentGoldLabel = root.Q<Label>("CurrentGoldLabel");
        
        // Find buttons
        damageUpgradeButton = root.Q<Button>("DamageUpgradeButton");
        attackSpeedUpgradeButton = root.Q<Button>("AttackSpeedUpgradeButton");
        speedUpgradeButton = root.Q<Button>("SpeedUpgradeButton");
        defenseUpgradeButton = root.Q<Button>("DefenseUpgradeButton");
        healthUpgradeButton = root.Q<Button>("HealthUpgradeButton");
        coinMultiplierUpgradeButton = root.Q<Button>("CoinMultiplierUpgradeButton");
        closeButton = root.Q<Button>("CloseButton");
        
        // Find descriptions
        damageDescription = root.Q<Label>("DamageDescription");
        attackSpeedDescription = root.Q<Label>("AttackSpeedDescription");
        speedDescription = root.Q<Label>("SpeedDescription");
        defenseDescription = root.Q<Label>("DefenseDescription");
        healthDescription = root.Q<Label>("HealthDescription");
        coinMultiplierDescription = root.Q<Label>("CoinMultiplierDescription");
        
        // Register button clicks
        damageUpgradeButton?.RegisterCallback<ClickEvent>(evt => OnDamageUpgrade());
        attackSpeedUpgradeButton?.RegisterCallback<ClickEvent>(evt => OnAttackSpeedUpgrade());
        speedUpgradeButton?.RegisterCallback<ClickEvent>(evt => OnSpeedUpgrade());
        defenseUpgradeButton?.RegisterCallback<ClickEvent>(evt => OnDefenseUpgrade());
        healthUpgradeButton?.RegisterCallback<ClickEvent>(evt => OnHealthUpgrade());
        coinMultiplierUpgradeButton?.RegisterCallback<ClickEvent>(evt => OnCoinMultiplierUpgrade());
        closeButton?.RegisterCallback<ClickEvent>(evt => CloseMenu());
        
        // Subscribe to currency changes
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged += UpdateGoldDisplay;
        }
        
        // Start hidden
        CloseMenu();
    }
    
    void Update()
    {
        // Toggle menu with Tab or ESC
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuOpen)
                CloseMenu();
            else
                OpenMenu();
        }
    }
    
    public void OpenMenu()
    {
        root.style.display = DisplayStyle.Flex;
        isMenuOpen = true;
        
        // Pause game
        Time.timeScale = 0f;
        
        UpdateAllDisplays();
        Debug.Log("Upgrade menu opened - Game paused");
    }
    
    public void CloseMenu()
    {
        root.style.display = DisplayStyle.None;
        isMenuOpen = false;
        
        // Unpause game
        Time.timeScale = 1f;
        
        Debug.Log("Upgrade menu closed - Game resumed");
    }
    
    void UpdateAllDisplays()
    {
        UpdateGoldDisplay(CurrencyManager.Instance.GetCurrency());
        UpdateUpgradeButtons();
    }
    
    void UpdateGoldDisplay(int gold)
    {
        if (currentGoldLabel != null)
        {
            currentGoldLabel.text = $"Gold: {gold}";
        }
    }
    
    void UpdateUpgradeButtons()
    {
        if (PlayerController.Instance == null) return;

        // Update damage
        damageDescription.text = $"Level {PlayerController.Instance.damageLevel} → +{PlayerController.Instance.damagePerUpgrade} damage per level";
        damageUpgradeButton.text = $"{PlayerController.Instance.damageCost} Gold";
        
        // Update attack speed
        attackSpeedDescription.text = $"Level {PlayerController.Instance.attackSpeedLevel} → -{PlayerController.Instance.attackSpeedPerUpgrade}s cooldown";
        attackSpeedUpgradeButton.text = $"{PlayerController.Instance.attackSpeedCost} Gold";
        
        // Update move speed
        speedDescription.text = $"Level {PlayerController.Instance.speedLevel} → +{PlayerController.Instance.speedPerUpgrade} speed";
        speedUpgradeButton.text = $"{PlayerController.Instance.speedCost} Gold";
        
        // Update defense
        defenseDescription.text = $"Level {PlayerController.Instance.defenseLevel} → +{PlayerController.Instance.defensePerUpgrade}% damage reduction";
        defenseUpgradeButton.text = $"{PlayerController.Instance.defenseCost} Gold";
        
        // Update health
        healthDescription.text = $"Level {PlayerController.Instance.healthLevel} → +{PlayerController.Instance.healthPerUpgrade} max health";
        healthUpgradeButton.text = $"{PlayerController.Instance.healthCost} Gold";
        
        // Update coin multiplier
        coinMultiplierDescription.text = $"Level {PlayerController.Instance.coinMultiplierLevel} → +{PlayerController.Instance.coinMultiplierPerUpgrade * 100}% coins";
        coinMultiplierUpgradeButton.text = $"{PlayerController.Instance.coinMultiplierCost} Gold";
    }
    
    void OnDamageUpgrade()
    {
        if (PlayerController.Instance.UpgradeDamage())
        {
            Debug.Log("Upgraded Damage!");
            UpdateAllDisplays();
        }
    }
    
    void OnAttackSpeedUpgrade()
    {
        if (PlayerController.Instance.UpgradeAttackSpeed())
        {
            Debug.Log("Upgraded Attack Speed!");
            UpdateAllDisplays();
        }
    }
    
    void OnSpeedUpgrade()
    {
        if (PlayerController.Instance.UpgradeSpeed())
        {
            Debug.Log("Upgraded Move Speed!");
            UpdateAllDisplays();
        }
    }
    
    void OnDefenseUpgrade()
    {
        if (PlayerController.Instance.UpgradeDefense())
        {
            Debug.Log("Upgraded Defense!");
            UpdateAllDisplays();
        }
    }
    
    void OnHealthUpgrade()
    {
        if (PlayerController.Instance.UpgradeHealth())
        {
            Debug.Log("Upgraded Max Health!");
            UpdateAllDisplays();
        }
    }
    
    void OnCoinMultiplierUpgrade()
    {
        if (PlayerController.Instance.UpgradeCoinMultiplier())
        {
            Debug.Log("Upgraded Coin Multiplier!");
            UpdateAllDisplays();
        }
    }
    
    void OnDestroy()
    {
        // Unsubscribe
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateGoldDisplay;
        }
        
        // Make sure game unpauses if menu is destroyed
        Time.timeScale = 1f;
    }
}