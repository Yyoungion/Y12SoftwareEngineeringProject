using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    [Header("Stats")]
    public float health = 50f;
    public float maxHealth = 50f;
    public float attackDamage = 10f;
    
    [Header("Movement")]
    public float walkSpeed = 1.5f;
    public float detectionRange = 6f;
    public float stopDistance = 2.5f;
    
    [Header("Jump Attack")]
    public float attackRange = 3f;
    public float jumpSpeed = 10f;
    public float jumpDuration = 0.4f;
    public float attackCooldown = 2.5f;
    
    [Header("Drops")]
    public int currencyDropAmount = 5;
    public int currencyDropVariance = 2;
    public int xpDropAmount = 10;

    [Header("References")]
    public Transform player;
    public Animator animator;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    
    private bool isAttacking = false;
    private bool isWalking = false;
    private float lastAttackTime = -10f;
    
    private Vector2 jumpDirection;
    private float jumpTimer = 0f;
    private Vector2 lastDirection = Vector2.down;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        originalColor = spriteRenderer.color;
        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        
        
        
        
        
        
        animator.SetBool("IsAttacking", false);
        UpdateAnimator(Vector2.down, 0f);
    }
    
    void Update()
    {
        if (player == null) return;
        
        if (isAttacking)
        {
            UpdateJumpAttack();
            return;
        }
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        
        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= attackRange && CanAttack())
            {
                StartJumpAttack(directionToPlayer);
            }
            else if (distanceToPlayer > stopDistance)
            {
                WalkTowardPlayer(directionToPlayer);
            }
            else
            {
                StopWalking(directionToPlayer);
            }
        }
        else
        {
            StopWalking(lastDirection);
        }
    }

    
    
    bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }
    
    void WalkTowardPlayer(Vector2 direction)
    {
        lastDirection = direction;
        rb.linearVelocity = direction * walkSpeed;
        
        if (!isWalking)
        {
            isWalking = true;
        }
        UpdateAnimator(direction, 1f);
    }
    
    void StopWalking(Vector2 faceDirection)
    {
        rb.linearVelocity = Vector2.zero;
        
        if (isWalking)
        {
            isWalking = false;
        }
        
        UpdateAnimator(faceDirection, 0f);
    }
    
    void StartJumpAttack(Vector2 direction)
    {
        Debug.Log("Slime starting jump attack!");
        
        isAttacking = true;
        isWalking = false;
        jumpTimer = 0f;
        lastAttackTime = Time.time;
        
        jumpDirection = direction;
        lastDirection = direction;
        
        
        animator.SetBool("IsAttacking", true);
        
        
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
        animator.SetFloat("Speed", 0f); 
        
        
        animator.SetTrigger("Attack");
        
        
        if (direction.x < -0.1f)
            spriteRenderer.flipX = true;
        else if (direction.x > 0.1f)
            spriteRenderer.flipX = false;
    }
    
    void UpdateJumpAttack()
    {
        jumpTimer += Time.deltaTime;
        rb.linearVelocity = jumpDirection * jumpSpeed;
        
        if (jumpTimer >= jumpDuration)
        {
            EndJumpAttack();
        }
    }
    
    void EndJumpAttack()
    {
        Debug.Log("Jump attack finished!");
        isAttacking = false;
        rb.linearVelocity = Vector2.zero;
        
        animator.SetBool("IsAttacking", false);
        
    float distanceToPlayer = Vector2.Distance(transform.position, player.position);
    if (distanceToPlayer <= 2f)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        
        
        LayerMask mask = LayerMask.GetMask("Player", "Wall"); 
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distanceToPlayer, mask);

        if (hit.collider != null && hit.collider.transform == player)
        {
            DamagePlayer();
        }
    }
        
        UpdateAnimator(lastDirection, 0f);
    }
    
    void DamagePlayer()
    {
        PlayerController playerController = PlayerController.Instance != null ? PlayerController.Instance : player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(attackDamage);
            Debug.Log("Slime damaged player for " + attackDamage + "!");
        }
    }
    
    void UpdateAnimator(Vector2 direction, float speed)
    {
        if (animator == null) return;

        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
        animator.SetFloat("Speed", speed);

        if (direction.x < -0.1f)
            spriteRenderer.flipX = true;
        else if (direction.x > 0.1f)
            spriteRenderer.flipX = false;
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        
        Debug.Log("Slime took " + damage + " damage! Health: " + health);
        
        StartCoroutine(FlashRed());
        
        
        animator.SetFloat("MoveX", lastDirection.x);
        animator.SetFloat("MoveY", lastDirection.y);
        animator.SetTrigger("Hurt");
        
        
        if (lastDirection.x < -0.1f)
            spriteRenderer.flipX = true;
        else if (lastDirection.x > 0.1f)
            spriteRenderer.flipX = false;
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    System.Collections.IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
    
    public void Die()
    {
        Debug.Log("Slime died!");
        
        
        if (CurrencyManager.Instance != null)
        {
            float coinMultiplier = PlayerController.Instance != null ? PlayerController.Instance.currentCoinMultiplier : 1f;
            int minCurrency = Mathf.Max(1, Mathf.FloorToInt((currencyDropAmount - currencyDropVariance) * coinMultiplier));
            int maxCurrency = Mathf.Max(minCurrency, Mathf.CeilToInt((currencyDropAmount + currencyDropVariance) * coinMultiplier));

            int actualCurrency = Random.Range(minCurrency, maxCurrency + 1);
            
            CurrencyManager.Instance.AddCurrency(actualCurrency);
            Debug.Log($"Dropped {actualCurrency} gold (range {minCurrency}-{maxCurrency}, multiplier {coinMultiplier}x)!");
        }
        
        
        isAttacking = false;
        isWalking = false;
        if (rb != null) rb.linearVelocity = Vector2.zero;
        
        
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        
        
        this.enabled = false;
        
        
        Destroy(gameObject, 1f);
    }
        
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}