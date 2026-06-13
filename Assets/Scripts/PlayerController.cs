using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
	public static PlayerController Instance { get; private set; }

    [Header("Base Stats")]
    public float baseAttackDamage = 25f;
    public float baseAttackCooldown = 0.5f;
    public float baseMoveSpeed = 5f;
    public float baseMaxHealth = 100f;
    public float baseCoinMultiplier = 1f;

    [Header("Current Stats")]
    public float currentAttackDamage;
    public float currentAttackCooldown;
    public float currentMoveSpeed;
    public float currentMaxHealth;
    public float currentCoinMultiplier;
    public float currentDefense = 0f;

    [Header("Upgrade Levels")]
    public int damageLevel = 0;
    public int attackSpeedLevel = 0;
    public int speedLevel = 0;
    public int defenseLevel = 0;
    public int healthLevel = 0;
    public int coinMultiplierLevel = 0;

    [Header("Upgrade Costs")]
    public int damageCost = 20;
    public int attackSpeedCost = 25;
    public int speedCost = 30;
    public int defenseCost = 35;
    public int healthCost = 40;
    public int coinMultiplierCost = 50;
    
    public float costIncreaseMultiplier = 1.5f;

    [Header("Upgrade Amounts")]
    public float damagePerUpgrade = 5f;
    public float attackSpeedPerUpgrade = 0.05f; // Reduces cooldown
    public float speedPerUpgrade = 0.5f;
    public float defensePerUpgrade = 5f; // 5% damage reduction per level
    public float healthPerUpgrade = 20f;
    public float coinMultiplierPerUpgrade = 0.2f;

	[Header("Movement")]
	public float moveSpeed = 5f;

	[Header("Attack")]
	public float attackDamage = 25f;
	public float attackRange = 1.5f;
	public float attackCooldown = 0.5f;
	public LayerMask enemyLayer;

	[Header("Health")]
	public float health = 100f;
	public float maxHealth = 100f;
	public Color damageColor = Color.red;

	[Header("Healing")]
	public int healCost = 10;
	public float healAmount = 25f;
	public KeyCode healKey = KeyCode.E;

	[Header("References")]
	[SerializeField] private GameObject attackHitbox;
	[SerializeField] private Animator animator;
	[SerializeField] private AudioClip attackSfx;
	[SerializeField, Range(0f, 1f)] private float attackSfxVolume = 1f;

	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;
	private AudioSource audioSource;
	private Color originalColor;
	private Vector2 movement;
	private Vector2 lastMovement = Vector2.down;
	private float lastAttackTime;

	private void Awake()
	{
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

		// initialize base stats from current inspector values
		baseAttackDamage = attackDamage;
		baseAttackCooldown = attackCooldown;
		baseMoveSpeed = moveSpeed;
		baseMaxHealth = maxHealth;

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
		if (attackSfx == null)
		{
			attackSfx = CreateDefaultAttackSfx();
		}
		originalColor = spriteRenderer.color;
		health = Mathf.Min(health, maxHealth);
	}

	private void Start()
	{
		RecalculateStats();
	}

	private void Update()
	{
		ReadMovementInput();
		UpdateAnimatorParameters();

		if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && Time.time >= lastAttackTime + attackCooldown)
		{
			Attack();
			lastAttackTime = Time.time;
		}

		 if (Input.GetKeyDown(healKey))
		{
			TryHeal();
		}

		// Debug upgrade hotkeys
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (UpgradeDamage()) Debug.Log("Upgraded Damage!");
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			if (UpgradeSpeed()) Debug.Log("Upgraded Speed!");
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			if (UpgradeAttackSpeed()) Debug.Log("Upgraded Attack Speed!");
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			if (UpgradeDefense()) Debug.Log("Upgraded Defense!");
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			if (UpgradeHealth()) Debug.Log("Upgraded Health!");
		}
		if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			if (UpgradeCoinMultiplier()) Debug.Log("Upgraded Coin Multiplier!");
		}
	}

	private void FixedUpdate()
	{
		rb.MovePosition(rb.position + movement * currentMoveSpeed * Time.fixedDeltaTime);
	}

	public void EnableHitbox()
	{
		if (attackHitbox != null)
		{
			attackHitbox.SetActive(true);
		}
	}

	public void DisableHitbox()
	{
		if (attackHitbox != null)
		{
			attackHitbox.SetActive(false);
		}
	}

	public void TakeDamage(float damage)
	{
		damage = ApplyDefense(damage);
		health -= damage;
		
		Debug.Log($"Player took {damage} damage! Health: {health}");
		
		if (health <= 0)
		{
			Die();
		}
	}

	private void ReadMovementInput()
	{
		movement = Vector2.zero;

		if (Keyboard.current == null)
		{
			return;
		}

		if (Keyboard.current.wKey.isPressed) movement.y += 1;
		if (Keyboard.current.sKey.isPressed) movement.y -= 1;
		if (Keyboard.current.aKey.isPressed) movement.x -= 1;
		if (Keyboard.current.dKey.isPressed) movement.x += 1;

		movement = movement.normalized;

		if (movement != Vector2.zero)
		{
			lastMovement = movement;
		}
	}

	private void UpdateAnimatorParameters()
	{
		if (animator == null)
		{
			return;
		}

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

	private void Attack()
	{
		PlayAttackSfx();

		if (animator != null)
		{
			// only trigger if parameter exists to avoid console warnings
			foreach (var p in animator.parameters)
			{
				if (p.name == "Attack" && p.type == AnimatorControllerParameterType.Trigger)
				{
					animator.SetTrigger("Attack");
					break;
				}
			}
		}

		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

		foreach (Collider2D enemy in hitEnemies)
		{
			SlimeEnemy slime = enemy.GetComponentInParent<SlimeEnemy>();
			if (slime != null)
			{
				slime.TakeDamage(attackDamage);
			}
		}

		Debug.Log("Player attacking!");
	}

	private void PlayAttackSfx()
	{
		if (audioSource != null && attackSfx != null)
		{
			float masterVolume = SettingsMenuController.MasterVolumePercent / 100f;
			float sfxVolume = SettingsMenuController.SfxVolumePercent / 100f;
			float finalVolume = Mathf.Clamp01(attackSfxVolume * masterVolume * sfxVolume);
			audioSource.PlayOneShot(attackSfx, finalVolume);
		}
	}

	public void SetAttackSfxVolume(float volume)
	{
		attackSfxVolume = Mathf.Clamp01(volume);
	}

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

	private System.Collections.IEnumerator DamageFlash()
	{
		spriteRenderer.color = damageColor;
		yield return new WaitForSeconds(0.1f);
		spriteRenderer.color = originalColor;
	}

	public void TryHeal()
	{
		// Check if already at max health
		if (health >= maxHealth)
		{
			Debug.Log("Already at max health!");
			return;
		}
		
		// Check if have enough currency
		if (CurrencyManager.Instance != null)
		{
			if (CurrencyManager.Instance.SpendCurrency(healCost))
			{
				// Heal
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

	private void Die()
	{
		Debug.Log("Player died!");
		Destroy(gameObject, 1f);
	}

	public void RecalculateStats()
	{
		currentAttackDamage = baseAttackDamage + (damageLevel * damagePerUpgrade);
		currentAttackCooldown = Mathf.Max(0.1f, baseAttackCooldown - (attackSpeedLevel * attackSpeedPerUpgrade));
		currentMoveSpeed = baseMoveSpeed + (speedLevel * speedPerUpgrade);
		currentDefense = defenseLevel * defensePerUpgrade;
		currentMaxHealth = baseMaxHealth + (healthLevel * healthPerUpgrade);
		currentCoinMultiplier = baseCoinMultiplier + (coinMultiplierLevel * coinMultiplierPerUpgrade);

		float healthPercent = maxHealth > 0f ? health / maxHealth : 1f;
		maxHealth = currentMaxHealth;
		health = Mathf.Clamp(maxHealth * healthPercent, 0f, maxHealth);

		attackDamage = currentAttackDamage;
		attackCooldown = currentAttackCooldown;
		moveSpeed = currentMoveSpeed;

		Debug.Log($"Stats Updated - Damage: {currentAttackDamage}, Attack Speed: {currentAttackCooldown}s, Speed: {currentMoveSpeed}, Defense: {currentDefense}%, Max Health: {currentMaxHealth}, Coin Mult: {currentCoinMultiplier}x");
	}

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

	public float ApplyDefense(float incomingDamage)
	{
		float damageReduction = currentDefense / 100f;
		return incomingDamage * (1f - damageReduction);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}
