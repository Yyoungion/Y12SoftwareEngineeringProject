using UnityEngine;
using UnityEngine.UIElements;

// Controls the upgrade menu UI and handles player upgrades
public class UpgradeMenuController : MonoBehaviour
{
    // References to the UI document and root elements
    private UIDocument uiDocument;
    private VisualElement root;
    private VisualElement overlay;

    // Displays the player's current gold
    private Label currentGoldLabel;

    // Upgrade buttons
    private Button damageUpgradeButton;
    private Button attackSpeedUpgradeButton;
    private Button speedUpgradeButton;
    private Button defenseUpgradeButton;
    private Button healthUpgradeButton;
    private Button coinMultiplierUpgradeButton;
    private Button closeButton;

    // Labels describing each upgrade
    private Label damageDescription;
    private Label attackSpeedDescription;
    private Label speedDescription;
    private Label defenseDescription;
    private Label healthDescription;
    private Label coinMultiplierDescription;

    // Tracks whether the menu is currently open
    private bool isMenuOpen = false;

    // Called before the first frame
    void Start()
    {
        // Get the UI document attached to this GameObject
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Find the overlay element
        overlay = root.Q<VisualElement>("Overlay") ?? root;

        // Find the gold display label
        currentGoldLabel = root.Q<Label>("CurrentGoldLabel");

        // Find all upgrade buttons
        damageUpgradeButton = root.Q<Button>("DamageUpgradeButton");
        attackSpeedUpgradeButton = root.Q<Button>("AttackSpeedUpgradeButton");
        speedUpgradeButton = root.Q<Button>("SpeedUpgradeButton");
        defenseUpgradeButton = root.Q<Button>("DefenseUpgradeButton") ?? root.Q<Button>("DefenceUpgradeButton");
        healthUpgradeButton = root.Q<Button>("HealthUpgradeButton") ?? root.Q<Button>("MaxHealthUpgradeButton");
        coinMultiplierUpgradeButton = root.Q<Button>("CoinMultiplierUpgradeButton");
        closeButton = root.Q<Button>("CloseButton");

        // Find the description labels
        damageDescription = root.Q<Label>("DamageDescription");
        attackSpeedDescription = root.Q<Label>("AttackSpeedDescription");
        speedDescription = root.Q<Label>("SpeedDescription");
        defenseDescription = root.Q<Label>("DefenseDescription") ?? root.Q<Label>("DefenceDescription");
        healthDescription = root.Q<Label>("HealthDescription") ?? root.Q<Label>("MaxHealthDescription");
        coinMultiplierDescription = root.Q<Label>("CoinMultiplierDescription");

        // Register button click events
        damageUpgradeButton?.RegisterCallback<ClickEvent>(evt => OnDamageUpgrade());
        attackSpeedUpgradeButton?.RegisterCallback<ClickEvent>(evt => OnAttackSpeedUpgrade());
        speedUpgradeButton?.RegisterCallback<ClickEvent>(evt => OnSpeedUpgrade());
        defenseUpgradeButton?.RegisterCallback<ClickEvent>(evt => OnDefenseUpgrade());
        healthUpgradeButton?.RegisterCallback<ClickEvent>(evt => OnHealthUpgrade());
        coinMultiplierUpgradeButton?.RegisterCallback<ClickEvent>(evt => OnCoinMultiplierUpgrade());
        closeButton?.RegisterCallback<ClickEvent>(evt => CloseMenu());

        // Changes in player gold
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged += UpdateGoldDisplay;
        }

        // Hide the menu when the game starts
        CloseMenu();
    }

    // Called once every frame
    void Update()
    {
        // Open or close the menu when the Tab key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isMenuOpen)
                CloseMenu();
            else
                OpenMenu();
        }
    }

    // Opens the upgrade menu
    public void OpenMenu()
    {
        // Display the menu
        overlay.style.display = DisplayStyle.Flex;
        isMenuOpen = true;

        // Pause the game
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Pause();
        }
        else
        {
            Time.timeScale = 0f;
        }

        // Refresh all displayed information
        UpdateAllDisplays();

        Debug.Log("Upgrade menu opened - Game paused");
    }

    // Closes the upgrade menu
    public void CloseMenu()
    {
        // Hide the menu
        overlay.style.display = DisplayStyle.None;
        isMenuOpen = false;

        // Resume the game
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Resume();
        }
        else
        {
            Time.timeScale = 1f;
        }

        Debug.Log("Upgrade menu closed - Game resumed");
    }

    // Updates all menu information
    void UpdateAllDisplays()
    {
        // Refresh the player's gold
        if (CurrencyManager.Instance != null)
        {
            UpdateGoldDisplay(CurrencyManager.Instance.GetCurrency());
        }

        // Refresh all upgrade information
        UpdateUpgradeButtons();
    }

    // Updates the gold display
    void UpdateGoldDisplay(int gold)
    {
        if (currentGoldLabel != null)
        {
            currentGoldLabel.text = $"Gold: {gold}";
        }
    }

    // Updates the upgrade descriptions and costs
    void UpdateUpgradeButtons()
    {
        if (PlayerController.Instance == null)
            return;

        damageDescription.text = $"Level {PlayerController.Instance.damageLevel} → +{PlayerController.Instance.damagePerUpgrade} damage per level";
        damageUpgradeButton.text = $"{PlayerController.Instance.damageCost} Gold";

        attackSpeedDescription.text = $"Level {PlayerController.Instance.attackSpeedLevel} → -{PlayerController.Instance.attackSpeedPerUpgrade}s cooldown";
        attackSpeedUpgradeButton.text = $"{PlayerController.Instance.attackSpeedCost} Gold";

        speedDescription.text = $"Level {PlayerController.Instance.speedLevel} → +{PlayerController.Instance.speedPerUpgrade} speed";
        speedUpgradeButton.text = $"{PlayerController.Instance.speedCost} Gold";

        defenseDescription.text = $"Level {PlayerController.Instance.defenseLevel} → +{PlayerController.Instance.defensePerUpgrade}% damage reduction";
        defenseUpgradeButton.text = $"{PlayerController.Instance.defenseCost} Gold";

        healthDescription.text = $"Level {PlayerController.Instance.healthLevel} → +{PlayerController.Instance.healthPerUpgrade} max health";
        healthUpgradeButton.text = $"{PlayerController.Instance.healthCost} Gold";

        coinMultiplierDescription.text = $"Level {PlayerController.Instance.coinMultiplierLevel} → +{PlayerController.Instance.coinMultiplierPerUpgrade * 100}% coins";
        coinMultiplierUpgradeButton.text = $"{PlayerController.Instance.coinMultiplierCost} Gold";
    }

    // Purchases a damage upgrade
    void OnDamageUpgrade()
    {
        if (PlayerController.Instance != null && PlayerController.Instance.UpgradeDamage())
        {
            Debug.Log("Upgraded Damage!");
            UpdateAllDisplays();
        }
    }

    // Purchases an attack speed upgrade
    void OnAttackSpeedUpgrade()
    {
        if (PlayerController.Instance != null && PlayerController.Instance.UpgradeAttackSpeed())
        {
            Debug.Log("Upgraded Attack Speed!");
            UpdateAllDisplays();
        }
    }

    // Purchases a movement speed upgrade
    void OnSpeedUpgrade()
    {
        if (PlayerController.Instance != null && PlayerController.Instance.UpgradeSpeed())
        {
            Debug.Log("Upgraded Move Speed!");
            UpdateAllDisplays();
        }
    }

    // Purchases a defence upgrade
    void OnDefenseUpgrade()
    {
        if (PlayerController.Instance != null && PlayerController.Instance.UpgradeDefense())
        {
            Debug.Log("Upgraded Defense!");
            UpdateAllDisplays();
        }
    }

    // Purchases a health upgrade
    void OnHealthUpgrade()
    {
        if (PlayerController.Instance != null && PlayerController.Instance.UpgradeHealth())
        {
            Debug.Log("Upgraded Max Health!");
            UpdateAllDisplays();
        }
    }

    // Purchases a coin multiplier upgrade
    void OnCoinMultiplierUpgrade()
    {
        if (PlayerController.Instance != null && PlayerController.Instance.UpgradeCoinMultiplier())
        {
            Debug.Log("Upgraded Coin Multiplier!");
            UpdateAllDisplays();
        }
    }

    // Called when this object is destroyed
    void OnDestroy()
    {
        // Stop listening for currency changes
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateGoldDisplay;
        }

        // Makes sure the game is running
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Resume();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}