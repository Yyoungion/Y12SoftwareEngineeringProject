using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class UpgradeMenuController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    private VisualElement overlay;
    private VisualElement menuContainer;
    
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
    private Coroutine openMenuAnimation;
    private Coroutine closeMenuAnimation;

    [SerializeField] private float slideInOffset = 80f;
    [SerializeField] private float openAnimationDuration = 0.22f;

    private T QueryElement<T>(params string[] names) where T : VisualElement
    {
        if (root == null)
        {
            return null;
        }

        foreach (string name in names)
        {
            T element = root.Q<T>(name);
            if (element != null)
            {
                return element;
            }
        }

        return null;
    }
    
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        
        // Find elements
        currentGoldLabel = root.Q<Label>("CurrentGoldLabel");
        overlay = root.Q<VisualElement>("Overlay");
        menuContainer = root.Q<VisualElement>("MenuContainer");
        
        // Find buttons
        damageUpgradeButton = root.Q<Button>("DamageUpgradeButton");
        attackSpeedUpgradeButton = root.Q<Button>("AttackSpeedUpgradeButton");
        speedUpgradeButton = root.Q<Button>("SpeedUpgradeButton");
        defenseUpgradeButton = QueryElement<Button>("DefenseUpgradeButton", "DefenceUpgradeButton");
        healthUpgradeButton = QueryElement<Button>("HealthUpgradeButton", "MaxHealthUpgradeButton");
        coinMultiplierUpgradeButton = root.Q<Button>("CoinMultiplierUpgradeButton");
        closeButton = root.Q<Button>("CloseButton");
        
        // Find descriptions
        damageDescription = root.Q<Label>("DamageDescription");
        attackSpeedDescription = root.Q<Label>("AttackSpeedDescription");
        speedDescription = root.Q<Label>("SpeedDescription");
        defenseDescription = QueryElement<Label>("DefenseDescription", "DefenceDescription");
        healthDescription = QueryElement<Label>("HealthDescription", "MaxHealthDescription");
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
        CloseMenu(false);
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

        if (openMenuAnimation != null)
        {
            StopCoroutine(openMenuAnimation);
            openMenuAnimation = null;
        }

        if (closeMenuAnimation != null)
        {
            StopCoroutine(closeMenuAnimation);
            closeMenuAnimation = null;
        }

        SetClosedVisualState();
        
        // Pause game
        Time.timeScale = 0f;
        
        UpdateAllDisplays();
        openMenuAnimation = StartCoroutine(AnimateOpenMenu());
        Debug.Log("Upgrade menu opened - Game paused");
    }
    
    public void CloseMenu(bool animate = true)
    {
        if (openMenuAnimation != null)
        {
            StopCoroutine(openMenuAnimation);
            openMenuAnimation = null;
        }

        if (closeMenuAnimation != null)
        {
            StopCoroutine(closeMenuAnimation);
            closeMenuAnimation = null;
        }

        if (root == null)
        {
            return;
        }

        isMenuOpen = false;
        Time.timeScale = 1f;

        if (!animate)
        {
            SetClosedVisualState();
            root.style.display = DisplayStyle.None;
            Debug.Log("Upgrade menu closed - Game resumed");
            return;
        }

        closeMenuAnimation = StartCoroutine(AnimateCloseMenu());
        
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
        if (damageDescription != null) damageDescription.text = $"Level {PlayerController.Instance.damageLevel} → +{PlayerController.Instance.damagePerUpgrade} damage per level";
        if (damageUpgradeButton != null) damageUpgradeButton.text = $"{PlayerController.Instance.damageCost} Gold";
        
        // Update attack speed
        if (attackSpeedDescription != null) attackSpeedDescription.text = $"Level {PlayerController.Instance.attackSpeedLevel} → -{PlayerController.Instance.attackSpeedPerUpgrade}s cooldown";
        if (attackSpeedUpgradeButton != null) attackSpeedUpgradeButton.text = $"{PlayerController.Instance.attackSpeedCost} Gold";
        
        // Update move speed
        if (speedDescription != null) speedDescription.text = $"Level {PlayerController.Instance.speedLevel} → +{PlayerController.Instance.speedPerUpgrade} speed";
        if (speedUpgradeButton != null) speedUpgradeButton.text = $"{PlayerController.Instance.speedCost} Gold";
        
        // Update defense
        if (defenseDescription != null) defenseDescription.text = $"Level {PlayerController.Instance.defenseLevel} → +{PlayerController.Instance.defensePerUpgrade}% damage reduction";
        if (defenseUpgradeButton != null) defenseUpgradeButton.text = $"{PlayerController.Instance.defenseCost} Gold";
        
        // Update health
        if (healthDescription != null) healthDescription.text = $"Level {PlayerController.Instance.healthLevel} → +{PlayerController.Instance.healthPerUpgrade} max health";
        if (healthUpgradeButton != null) healthUpgradeButton.text = $"{PlayerController.Instance.healthCost} Gold";
        
        // Update coin multiplier
        if (coinMultiplierDescription != null) coinMultiplierDescription.text = $"Level {PlayerController.Instance.coinMultiplierLevel} → +{PlayerController.Instance.coinMultiplierPerUpgrade * 100}% coins";
        if (coinMultiplierUpgradeButton != null) coinMultiplierUpgradeButton.text = $"{PlayerController.Instance.coinMultiplierCost} Gold";
    }

    void SetClosedVisualState()
    {
        if (overlay != null)
        {
            overlay.style.opacity = 0f;
        }

        if (menuContainer != null)
        {
            menuContainer.style.translate = new Translate(
                new Length(0f, LengthUnit.Pixel),
                new Length(slideInOffset, LengthUnit.Pixel)
            );
        }
    }

    IEnumerator AnimateOpenMenu()
    {
        float elapsedTime = 0f;

        while (elapsedTime < openAnimationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / openAnimationDuration);
            float easedTime = normalizedTime * normalizedTime * (3f - 2f * normalizedTime);

            if (overlay != null)
            {
                overlay.style.opacity = easedTime;
            }

            if (menuContainer != null)
            {
                float currentOffset = Mathf.Lerp(slideInOffset, 0f, easedTime);
                menuContainer.style.translate = new Translate(
                    new Length(0f, LengthUnit.Pixel),
                    new Length(currentOffset, LengthUnit.Pixel)
                );
            }

            yield return null;
        }

        if (overlay != null)
        {
            overlay.style.opacity = 1f;
        }

        if (menuContainer != null)
        {
            menuContainer.style.translate = Translate.None();
        }

        openMenuAnimation = null;
    }

    IEnumerator AnimateCloseMenu()
    {
        float elapsedTime = 0f;

        while (elapsedTime < openAnimationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / openAnimationDuration);
            float easedTime = normalizedTime * normalizedTime * (3f - 2f * normalizedTime);

            if (overlay != null)
            {
                overlay.style.opacity = 1f - easedTime;
            }

            if (menuContainer != null)
            {
                float currentOffset = Mathf.Lerp(0f, slideInOffset, easedTime);
                menuContainer.style.translate = new Translate(
                    new Length(0f, LengthUnit.Pixel),
                    new Length(currentOffset, LengthUnit.Pixel)
                );
            }

            yield return null;
        }

        SetClosedVisualState();
        root.style.display = DisplayStyle.None;
        closeMenuAnimation = null;
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