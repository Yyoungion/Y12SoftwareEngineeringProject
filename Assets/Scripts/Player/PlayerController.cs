using UnityEngine;
using UnityEngine.InputSystem;

// Controls the player's movement, combat, health, upgrades, and interactions
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    // Base player statistics before upgrades
    [Header("Base Stats")]
    public float baseAttackDamage = 25f;
    public float baseAttackCooldown = 0.5f;
    public float baseMoveSpeed = 5f;
    public float baseMaxHealth = 100f;
    public float baseCoinMultiplier = 1f;

    // Current player statistics after upgrades 
    [Header("Current Stats")]
    public float currentAttackDamage;
    public float currentAttackCooldown;
    public float currentMoveSpeed;
    public float currentMaxHealth;
    public float currentCoinMultiplier;
    public float currentDefense = 0f;

    // Tracks the player's current upgrade levels
    [Header("Upgrade Levels")]
    public int damageLevel = 0;
    public int attackSpeedLevel = 0;
    public int speedLevel = 0;
    public int defenseLevel = 0;
    public int healthLevel = 0;
    public int coinMultiplierLevel = 0;

    // Gold cost for each upgrade
    [Header("Upgrade Costs")]
    public int damageCost = 20;
    public int attackSpeedCost = 25;
    public int speedCost = 30;
    public int defenseCost = 35;
    public int healthCost = 40;
    public int coinMultiplierCost = 50;

    // Multiplier that increases upgrade costs after every purchase
    public float costIncreaseMultiplier = 1.5f;

    // Amount each upgrade increases a stat
    [Header("Upgrade Amounts")]
    public float damagePerUpgrade = 5f;
    public float attackSpeedPerUpgrade = 0.05f;
    public float speedPerUpgrade = 0.5f;
    public float defensePerUpgrade = 5f;
    public float healthPerUpgrade = 20f;
    public float coinMultiplierPerUpgrade = 0.2f;

    // Movement settings
    [Header("Movement")]
    public float moveSpeed = 5f;

    // Attack settings
    [Header("Attack")]
    public float attackDamage = 25f;
    public float attackRange = 1.5f;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;
    public LayerMask obstacleLayers;

    // Health settings
    [Header("Health")]
    public float health = 100f;
    public float maxHealth = 100f;
    public Color damageColor = Color.red;

    // Healing settings
    [Header("Healing")]
    public int healCost = 10;
    public float healAmount = 25f;
    public KeyCode healKey = KeyCode.E;

    // References assigned in the Unity Inspector
    [Header("References")]
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip attackSfx;
    [SerializeField, Range(0f, 1f)] private float attackSfxVolume = 1f;

    // Component references
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    // Stores the sprite's normal colour
    private Color originalColor;

    // Stores player movement input
    private Vector2 movement;

    // Remembers the last movement direction for idle animations
    private Vector2 lastMovement = Vector2.down;

    // Tracks when the player last attacked
    private float lastAttackTime;

    // Called when the object is first created
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Store the player's starting stats
        baseAttackDamage = attackDamage;
        baseAttackCooldown = attackCooldown;
        baseMoveSpeed = moveSpeed;
        baseMaxHealth = maxHealth;

        // Get required components
        rb = GetComponent<Rigidbody2D>();
        animator = animator == null ? GetComponent<Animator>() : animator;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0f;
        }

        // Default obstacle layer to "Wall" if none is selected
        if (obstacleLayers.value == 0)
        {
            obstacleLayers = LayerMask.GetMask("Wall");
        }

        // Save the sprite's original colour
        originalColor = spriteRenderer.color;

        // Prevent health from starting above the maximum
        health = Mathf.Min(health, maxHealth);
    }

    // Called before the first frame
    private void Start()
    {
        // Calculate all player stats
        RecalculateStats();
    }

    // Called every frame
    private void Update()
    {
        // Read keyboard movement input
        ReadMovementInput();

        // Update animation parameters
        UpdateAnimatorParameters();

        // Attack when the left mouse button is clicked and the cooldown has expired
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }

        // Heal when the heal key is pressed
        if (Input.GetKeyDown(healKey))
        {
            TryHeal();
        }
	}

    private void FixedUpdate()
    {
        // Move the player using Rigidbody2D physics
        rb.MovePosition(rb.position + movement * currentMoveSpeed * Time.fixedDeltaTime);
    }

    // Enables the attack hitbox during the attack animation
    public void EnableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
        }
    }

    // Disables the attack hitbox after the attack animation
    public void DisableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    // Damages the player
    public void TakeDamage(float damage)
    {
        // Apply defence before subtracting health
        damage = ApplyDefense(damage);
        health -= damage;

        Debug.Log($"Player took {damage} damage! Health: {health}");

        // Kill the player if health reaches zero
        if (health <= 0)
        {
            Die();
        }
    }

    // Reads movement input from the keyboard
    private void ReadMovementInput()
    {
        movement = Vector2.zero;

        // Stop if no keyboard is connected
        if (Keyboard.current == null)
        {
            return;
        }

        // Read WASD movement
        if (Keyboard.current.wKey.isPressed) movement.y += 1;
        if (Keyboard.current.sKey.isPressed) movement.y -= 1;
        if (Keyboard.current.aKey.isPressed) movement.x -= 1;
        if (Keyboard.current.dKey.isPressed) movement.x += 1;

        // Prevent faster diagonal movement
        movement = movement.normalized;

        // Store the last movement direction
        if (movement != Vector2.zero)
        {
            lastMovement = movement;
        }
    }
    // Updates animation parameters
    private void UpdateAnimatorParameters()
    {
        // Exit if no Animator is attached
        if (animator == null)
        {
            return;
        }

        // Helper function that only sets parameters that exist
        void SetIfExists(string param, float value)
        {
            foreach (var p in animator.parameters)
            {
                if (p.name == param && p.type == AnimatorControllerParameterType.Float)
                {
                    animator.SetFloat(param, value);
                    break;
                }
            }
        }

        // Update movement and idle direction parameters
        SetIfExists("Speed", movement.sqrMagnitude);
        SetIfExists("MoveX", movement.x);
        SetIfExists("MoveY", movement.y);
        SetIfExists("LastMoveX", lastMovement.x);
        SetIfExists("LastMoveY", lastMovement.y);
        SetIfExists("Horizontal", movement.x);
        SetIfExists("Vertical", movement.y);
        SetIfExists("LastHorizontal", lastMovement.x);
        SetIfExists("LastVertical", lastMovement.y);
    }

    // Performs a melee attack
    private void Attack()
    {
        // Play the attack sound effect
        PlayAttackSfx();

        // Trigger the attack animation
        if (animator != null)
        {
            foreach (var p in animator.parameters)
            {
                if (p.name == "Attack" && p.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.SetTrigger("Attack");
                    break;
                }
            }
        }

        // Find all enemies within attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy == null)
            {
                continue;
            }

            // Check whether a wall blocks the attack
            Vector2 targetPoint = enemy.bounds.center;
            RaycastHit2D obstacleHit = Physics2D.Linecast(transform.position, targetPoint, obstacleLayers);

            if (obstacleHit.collider != null)
            {
                continue;
            }

            // Damage the enemy if it is a slime
            SlimeEnemy slime = enemy.GetComponentInParent<SlimeEnemy>();

            if (slime != null)
            {
                slime.TakeDamage(attackDamage);
            }
        }

        Debug.Log("Player attacking!");
    }

    // Plays the attack sound effect
    private void PlayAttackSfx()
    {
        if (audioSource != null && attackSfx != null)
        {
            // Calculate the final volume using the player's settings
            float masterVolume = SettingsMenuController.MasterVolumePercent / 100f;
            float sfxVolume = SettingsMenuController.SfxVolumePercent / 100f;
            float finalVolume = Mathf.Clamp01(attackSfxVolume * masterVolume * sfxVolume);

            audioSource.PlayOneShot(attackSfx, finalVolume);
        }
    }

    // Changes the attack sound volume
    public void SetAttackSfxVolume(float volume)
    {
        attackSfxVolume = Mathf.Clamp01(volume);
    }

    // Generates a default attack sound if none is assigned
    private AudioClip CreateDefaultAttackSfx()
    {
        const int sampleRate = 44100;
        const float durationSeconds = 0.12f;

        int sampleCount = Mathf.CeilToInt(sampleRate * durationSeconds);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float normalizedTime = (float)i / sampleRate;
            float envelope = Mathf.Exp(-normalizedTime * 28f);
            float noise = Random.Range(-1f, 1f);
            float tone = Mathf.Sin(2f * Mathf.PI * 140f * normalizedTime);

            samples[i] = (noise * 0.7f + tone * 0.3f) * envelope * 0.4f;
        }

        AudioClip clip = AudioClip.Create("DefaultAttackSfx", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);

        return clip;
    }

    // Briefly flashes the player red after taking damage
    private System.Collections.IEnumerator DamageFlash()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    // Attempts to heal the player using gold
    public void TryHeal()
    {
        // Do nothing if already at full health
        if (health >= maxHealth)
        {
            Debug.Log("Already at max health!");
            return;
        }

        // Check if the player has enough currency
        if (CurrencyManager.Instance != null)
        {
            if (CurrencyManager.Instance.SpendCurrency(healCost))
            {
                // Restore health
                health += healAmount;

                if (health > maxHealth)
                    health = maxHealth;

                Debug.Log($"Healed for {healAmount}! Current health: {health}");
            }
            else
            {
                Debug.Log($"Not enough gold! Need {healCost}, have {CurrencyManager.Instance.GetCurrency()}");
            }
        }
    }

    // Handles the player's death
    void Die()
    {
        Debug.Log("Player died!");

        // Get the current wave
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        int waveReached = 1;

        if (waveManager != null)
        {
            waveReached = waveManager.currentWave;
        }

        // Get the amount of gold earned
        int goldEarned = 0;

        if (CurrencyManager.Instance != null)
        {
            goldEarned = CurrencyManager.Instance.currentCurrency;
        }

        // Display the death screen
        DeathScreenController deathScreen = FindObjectOfType<DeathScreenController>();

        if (deathScreen != null)
        {
            deathScreen.ShowDeathScreen(waveReached, goldEarned);
        }
        else
        {
            Debug.LogError("DeathScreenController not found in scene!");
        }

        // Ensure time is running
        Time.timeScale = 1f;

        // Disable player controls
        enabled = false;
    }

    // Recalculates all player stats after upgrades
    public void RecalculateStats()
    {
        // Calculate upgraded statistics
        currentAttackDamage = baseAttackDamage + (damageLevel * damagePerUpgrade);
        currentAttackCooldown = Mathf.Max(0.1f, baseAttackCooldown - (attackSpeedLevel * attackSpeedPerUpgrade));
        currentMoveSpeed = baseMoveSpeed + (speedLevel * speedPerUpgrade);
        currentDefense = defenseLevel * defensePerUpgrade;
        currentMaxHealth = baseMaxHealth + (healthLevel * healthPerUpgrade);
        currentCoinMultiplier = baseCoinMultiplier + (coinMultiplierLevel * coinMultiplierPerUpgrade);

        // Preserve the player's current health percentage
        float healthPercent = maxHealth > 0f ? health / maxHealth : 1f;

        maxHealth = currentMaxHealth;
        health = Mathf.Clamp(maxHealth * healthPercent, 0f, maxHealth);

        // Apply the updated stats
        attackDamage = currentAttackDamage;
        attackCooldown = currentAttackCooldown;
        moveSpeed = currentMoveSpeed;

        Debug.Log($"Stats Updated - Damage: {currentAttackDamage}, Attack Speed: {currentAttackCooldown}s, Speed: {currentMoveSpeed}, Defense: {currentDefense}%, Max Health: {currentMaxHealth}, Coin Mult: {currentCoinMultiplier}x");
    }

    // Purchases a damage upgrade
    public bool UpgradeDamage()
    {
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendCurrency(damageCost))
        {
            damageLevel++;
            damageCost = Mathf.RoundToInt(damageCost * costIncreaseMultiplier);
            RecalculateStats();
            return true;
        }

        return false;
    }

    // Purchases an attack speed upgrade
    public bool UpgradeAttackSpeed()
    {
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendCurrency(attackSpeedCost))
        {
            attackSpeedLevel++;
            attackSpeedCost = Mathf.RoundToInt(attackSpeedCost * costIncreaseMultiplier);
            RecalculateStats();
            return true;
        }

        return false;
    }

    // Purchases a movement speed upgrade
    public bool UpgradeSpeed()
    {
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendCurrency(speedCost))
        {
            speedLevel++;
            speedCost = Mathf.RoundToInt(speedCost * costIncreaseMultiplier);
            RecalculateStats();
            return true;
        }

        return false;
    }

    // Purchases a defence upgrade
    public bool UpgradeDefense()
    {
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendCurrency(defenseCost))
        {
            defenseLevel++;
            defenseCost = Mathf.RoundToInt(defenseCost * costIncreaseMultiplier);
            RecalculateStats();
            return true;
        }

        return false;
    }

    // Purchases a health upgrade
    public bool UpgradeHealth()
    {
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendCurrency(healthCost))
        {
            healthLevel++;
            healthCost = Mathf.RoundToInt(healthCost * costIncreaseMultiplier);
            RecalculateStats();
            return true;
        }

        return false;
    }

    // Purchases a coin multiplier upgrade
    public bool UpgradeCoinMultiplier()
    {
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendCurrency(coinMultiplierCost))
        {
            coinMultiplierLevel++;
            coinMultiplierCost = Mathf.RoundToInt(coinMultiplierCost * costIncreaseMultiplier);
            RecalculateStats();
            return true;
        }

        return false;
    }

    // Applies the player's defence to incoming damage
    public float ApplyDefense(float incomingDamage)
    {
        float damageReduction = currentDefense / 100f;
        return incomingDamage * (1f - damageReduction);
    }

    // Draws the attack range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}