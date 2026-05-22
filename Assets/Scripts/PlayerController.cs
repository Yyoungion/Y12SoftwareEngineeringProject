using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
	public static PlayerController Instance { get; private set; }

	[Header("Base Stats")]
	public float baseAttackDamage = 25f;
	public float baseAttackRange = 1.5f;
	public float baseAttackCooldown = 0.5f;
	public float baseMoveSpeed = 5f;

	[Header("Upgrades")]
	public int damageUpgradeLevel = 0;
	public int rangeUpgradeLevel = 0;
	public int speedUpgradeLevel = 0;
	public int attackSpeedUpgradeLevel = 0;

	[Header("Upgrade Costs")]
	public int damageUpgradeCost = 20;
	public int rangeUpgradeCost = 25;
	public int speedUpgradeCost = 30;
	public int attackSpeedUpgradeCost = 35;

	[Header("Upgrade Amounts")]
	public float damagePerUpgrade = 5f;
	public float rangePerUpgrade = 0.3f;
	public float speedPerUpgrade = 0.5f;
	public float attackSpeedPerUpgrade = 0.05f; // reduces cooldown

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

	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;
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
		baseAttackRange = attackRange;
		baseAttackCooldown = attackCooldown;
		baseMoveSpeed = moveSpeed;

		rb = GetComponent<Rigidbody2D>();
		animator = animator == null ? GetComponent<Animator>() : animator;
		spriteRenderer = GetComponent<SpriteRenderer>();
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
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			if (UpgradeRange()) Debug.Log("Upgraded Range!");
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			if (UpgradeSpeed()) Debug.Log("Upgraded Speed!");
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			if (UpgradeAttackSpeed()) Debug.Log("Upgraded Attack Speed!");
		}
	}

	private void FixedUpdate()
	{
		rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
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
		health -= damage;
		StartCoroutine(DamageFlash());
		Debug.Log("Player took " + damage + " damage! Health: " + health);

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
		attackDamage = baseAttackDamage + (damageUpgradeLevel * damagePerUpgrade);
		attackRange = baseAttackRange + (rangeUpgradeLevel * rangePerUpgrade);
		moveSpeed = baseMoveSpeed + (speedUpgradeLevel * speedPerUpgrade);
		attackCooldown = Mathf.Max(0.1f, baseAttackCooldown - (attackSpeedUpgradeLevel * attackSpeedPerUpgrade));

		// If other attack scripts exist, they should read these updated values from this controller.

		Debug.Log($"Stats Updated - Damage: {attackDamage}, Range: {attackRange}, Speed: {moveSpeed}, Attack Speed: {attackCooldown}");
	}

	public bool UpgradeDamage()
	{
		if (CurrencyManager.Instance.SpendCurrency(damageUpgradeCost))
		{
			damageUpgradeLevel++;
			damageUpgradeCost = Mathf.RoundToInt(damageUpgradeCost * 1.5f);
			RecalculateStats();
			return true;
		}
		return false;
	}

	public bool UpgradeRange()
	{
		if (CurrencyManager.Instance.SpendCurrency(rangeUpgradeCost))
		{
			rangeUpgradeLevel++;
			rangeUpgradeCost = Mathf.RoundToInt(rangeUpgradeCost * 1.5f);
			RecalculateStats();
			return true;
		}
		return false;
	}

	public bool UpgradeSpeed()
	{
		if (CurrencyManager.Instance.SpendCurrency(speedUpgradeCost))
		{
			speedUpgradeLevel++;
			speedUpgradeCost = Mathf.RoundToInt(speedUpgradeCost * 1.5f);
			RecalculateStats();
			return true;
		}
		return false;
	}

	public bool UpgradeAttackSpeed()
	{
		if (CurrencyManager.Instance.SpendCurrency(attackSpeedUpgradeCost))
		{
			attackSpeedUpgradeLevel++;
			attackSpeedUpgradeCost = Mathf.RoundToInt(attackSpeedUpgradeCost * 1.5f);
			RecalculateStats();
			return true;
		}
		return false;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}
