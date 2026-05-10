using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
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
		rb = GetComponent<Rigidbody2D>();
		animator = animator == null ? GetComponent<Animator>() : animator;
		spriteRenderer = GetComponent<SpriteRenderer>();
		originalColor = spriteRenderer.color;
		health = Mathf.Min(health, maxHealth);
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

		animator.SetFloat("Speed", movement.sqrMagnitude);
		animator.SetFloat("MoveX", movement.x);
		animator.SetFloat("MoveY", movement.y);
		animator.SetFloat("LastMoveX", lastMovement.x);
		animator.SetFloat("LastMoveY", lastMovement.y);
		animator.SetFloat("Horizontal", movement.x);
		animator.SetFloat("Vertical", movement.y);
		animator.SetFloat("LastHorizontal", lastMovement.x);
		animator.SetFloat("LastVertical", lastMovement.y);
	}

	private void Attack()
	{
		if (animator != null)
		{
			animator.SetTrigger("Attack");
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

	private void Die()
	{
		Debug.Log("Player died!");
		Destroy(gameObject, 1f);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}
