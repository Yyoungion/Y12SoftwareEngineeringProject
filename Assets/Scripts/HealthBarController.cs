using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement healthBarFill;
    private Label healthLabel;
    
    private PlayerController playerHealth;
    
    void Start()
    {
        // Get UI Document
        uiDocument = GetComponent<UIDocument>();
        
        // Find elements
        var root = uiDocument.rootVisualElement;
        healthBarFill = root.Q<VisualElement>("HealthBarFill");
        healthLabel = root.Q<Label>("HealthLabel");
        
        // Find player health script
        playerHealth = FindFirstObjectByType<PlayerController>();
        
        if (playerHealth == null)
        {
            Debug.LogError("PlayerController not found! Make sure player has PlayerController script.");
        }
    }
    
    void Update()
    {
        if (playerHealth != null)
        {
            UpdateHealthBar();
        }
    }
    
    void UpdateHealthBar()
    {
        // Calculate health percentage
        float healthPercent = playerHealth.health / playerHealth.maxHealth;
        
        // Update fill width
        healthBarFill.style.width = Length.Percent(healthPercent * 100);
        
        // Update text with fraction - CHANGE THIS LINE
        int currentHealth = Mathf.CeilToInt(playerHealth.health);
        int maxHealth = Mathf.CeilToInt(playerHealth.maxHealth);
        healthLabel.text = $"Health: {currentHealth}/{maxHealth}";
        
        // Change color based on health
        if (healthPercent > 0.5f)
        {
            healthBarFill.style.backgroundColor = new Color(0.2f, 0.8f, 0.2f);
        }
        else if (healthPercent > 0.25f)
        {
            healthBarFill.style.backgroundColor = new Color(0.9f, 0.9f, 0.2f);
        }
        else
        {
            healthBarFill.style.backgroundColor = new Color(0.9f, 0.2f, 0.2f);
        }
    }
}