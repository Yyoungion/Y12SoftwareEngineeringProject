using UnityEngine;
using UnityEngine.UIElements;

// Controls the player's health bar UI and updates it based on the player's health
public class HealthBarController : MonoBehaviour
{
    // References to the UI document and health bar elements
    private UIDocument uiDocument;
    private VisualElement healthBarFill;
    private Label healthLabel;
    
    // Reference to the player's health data
    private PlayerController playerHealth;
    
    // Called before the first frame
    void Start()
    {
        // Get the UI Document attached to this GameObject
        uiDocument = GetComponent<UIDocument>();
        
        // Find the health bar UI elements
        var root = uiDocument.rootVisualElement;
        healthBarFill = root.Q<VisualElement>("HealthBarFill");
        healthLabel = root.Q<Label>("HealthLabel");
        
        // Find the PlayerController in the scene
        playerHealth = FindFirstObjectByType<PlayerController>();
        
        // Display an error if the player cannot be found
        if (playerHealth == null)
        {
            Debug.LogError("PlayerController not found.");
        }
    }
    
    // Called once every frame
    void Update()
    {
        // Update the health bar if the player exists
        if (playerHealth != null)
        {
            UpdateHealthBar();
        }
    }
    
    // Updates the health bar's size, text, and colour
    void UpdateHealthBar()
    {
        // Calculate the player's current health as a percentage
        float healthPercent = playerHealth.health / playerHealth.maxHealth;
        
        // Adjust the width of the health bar to match the health percentage
        healthBarFill.style.width = Length.Percent(healthPercent * 100);
        
        // Display the player's current and maximum health
        int currentHealth = Mathf.CeilToInt(playerHealth.health);
        int maxHealth = Mathf.CeilToInt(playerHealth.maxHealth);
        healthLabel.text = $"Health: {currentHealth}/{maxHealth}";
        
        // Change the health bar colour depending on the remaining health
        if (healthPercent > 0.5f)
        {
            // Green when health is above 50%
            healthBarFill.style.backgroundColor = new Color(0.2f, 0.8f, 0.2f);
        }
        else if (healthPercent > 0.25f)
        {
            // Yellow when health is between 25% and 50%
            healthBarFill.style.backgroundColor = new Color(0.9f, 0.9f, 0.2f);
        }
        else
        {
            // Red when health is below 25%
            healthBarFill.style.backgroundColor = new Color(0.9f, 0.2f, 0.2f);
        }
    }
}