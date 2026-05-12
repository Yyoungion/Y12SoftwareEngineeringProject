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
        
        // Update fill width (0% to 100%)
        healthBarFill.style.width = Length.Percent(healthPercent * 100);
        
        // Update text
        healthLabel.text = $"Health: {Mathf.Ceil(playerHealth.health)}/{playerHealth.maxHealth}";
        
        // Change color based on health
        if (healthPercent > 0.5f)
        {
            // Green when healthy
            healthBarFill.style.backgroundColor = new Color(0.2f, 0.8f, 0.2f);
        }
        else if (healthPercent > 0.25f)
        {
            // Yellow when medium
            healthBarFill.style.backgroundColor = new Color(0.9f, 0.9f, 0.2f);
        }
        else
        {
            // Red when low
            healthBarFill.style.backgroundColor = new Color(0.9f, 0.2f, 0.2f);
        }
    }
}