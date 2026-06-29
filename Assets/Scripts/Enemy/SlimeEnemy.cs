using UnityEngine; // Imports the Unity engine.

public class SlimeEnemy : MonoBehaviour // Defines the SlimeEnemy class.
{
    [Header("Stats")] // Groups the enemy's stats.
    public float health = 50f; // The enemy's current health.
    public float maxHealth = 50f; // The enemy's maximum health.
    public float attackDamage = 10f; // The amount of damage dealt to the player

    [Header("Movement")] // Groups movement settings.
    public float walkSpeed = 1.5f; // Speed at which the slime walks.
    public float detectionRange = 6f; // Distance at which the slime can see the player the player.
    public float stopDistance = 2.5f; // Distance where the slime stops moving before attacking.

    [Header("Jump Attack")] // Groups jump attack settings.
    public float attackRange = 3f; // Distance required to begin an attack.
    public float jumpSpeed = 10f; // Speed of the slime during its jump.
    public float jumpDuration = 0.4f; // How long the jump lasts.
    public float attackCooldown = 2.5f; // Time between attacks.

    [Header("Drops")] // Groups reward settings.
    public int currencyDropAmount = 5; // Base amount of currency dropped.
    public int currencyDropVariance = 2; // Random variation in currency dropped.
    public int xpDropAmount = 10; // Amount of experience awarded.

    [Header("References")] // Groups references to other components.
    public Transform player; // Reference to the player.
    public Animator animator; // Reference to the Animator component.

    // References to required components.
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    // Stores the slime's current state.
    private bool isAttacking = false;
    private bool isWalking = false;
    private float lastAttackTime = -10f;

    // Variables used during jump attacks.
    private Vector2 jumpDirection;
    private float jumpTimer = 0f;
    private Vector2 lastDirection = Vector2.down;

    void Start()
    {
        // Get required components attached to this GameObject.
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Store the sprite's original colour.
        originalColor = spriteRenderer.color;

        // If no player has been assigned in the Inspector
        if (player == null)
        {
            // Find the GameObject tagged "Player".
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            // If a player exists...
            if (playerObj != null)
            {
                // Store its Transform.
                player = playerObj.transform;
            }
        }

        // Ensure the slime starts in a non-attacking state.
        animator.SetBool("IsAttacking", false);

        // Initialise the animation facing downward while idle.
        UpdateAnimator(Vector2.down, 0f);
    }

    // Called once every frame.
    void Update()
    {
        // Exit if no player exists.
        if (player == null) return;

        // Continue the jump attack if already attacking.
        if (isAttacking)
        {
            UpdateJumpAttack();
            return;
        }

        // Calculate the distance to the player.
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Calculate the normalised direction towards the player.
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // If the player is within detection range...
        if (distanceToPlayer <= detectionRange)
        {
            // Attack if close enough and the cooldown has expired.
            if (distanceToPlayer <= attackRange && CanAttack())
            {
                StartJumpAttack(directionToPlayer);
            }
            // Otherwise, walk towards the player.
            else if (distanceToPlayer > stopDistance)
            {
                WalkTowardPlayer(directionToPlayer);
            }
            // Otherwise stop moving.
            else
            {
                StopWalking(directionToPlayer);
            }
        }
        else
        {
            // Player is too far away, so remain idle.
            StopWalking(lastDirection);
        }
    }

    // Returns true if enough time has passed since the previous attack.
    bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    // Moves the slime towards the player.
    void WalkTowardPlayer(Vector2 direction)
    {
        // Remember the last movement direction.
        lastDirection = direction;

        // Move using the Rigidbody.
        rb.linearVelocity = direction * walkSpeed;

        // Mark the slime as walking.
        if (!isWalking)
        {
            isWalking = true;
        }

        // Update the walking animation.
        UpdateAnimator(direction, 1f);
    }

    // Stops the slime's movement.
    void StopWalking(Vector2 faceDirection)
    {
        // Stop all movement.
        rb.linearVelocity = Vector2.zero;

        // Mark the slime as no longer walking.
        if (isWalking)
        {
            isWalking = false;
        }

        // Play the idle animation while facing the player.
        UpdateAnimator(faceDirection, 0f);
    }

    // Starts the slime's jump attack.
    void StartJumpAttack(Vector2 direction)
    {
        // Print a debug message.
        Debug.Log("Slime starting jump attack!");

        // Enter the attacking state.
        isAttacking = true;
        isWalking = false;
        jumpTimer = 0f;

        // Record the attack time for cooldown.
        lastAttackTime = Time.time;

        // Store the jump direction.
        jumpDirection = direction;
        lastDirection = direction;

        // Tell the Animator that an attack has started.
        animator.SetBool("IsAttacking", true);

        // Face the attack direction.
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);

        // Stop the movement animation.
        animator.SetFloat("Speed", 0f);

        // Trigger the attack animation.
        animator.SetTrigger("Attack");

        // Flip the sprite depending on direction.
        if (direction.x < -0.1f)
            spriteRenderer.flipX = true;
        else if (direction.x > 0.1f)
            spriteRenderer.flipX = false;
    }

    // Updates the jump attack every frame.
    void UpdateJumpAttack()
    {
        // Increase the jump timer.
        jumpTimer += Time.deltaTime;

        // Continue moving in the jump direction.
        rb.linearVelocity = jumpDirection * jumpSpeed;

        // End the attack once the jump finishes.
        if (jumpTimer >= jumpDuration)
        {
            EndJumpAttack();
        }
    }

    // Ends the slime's jump attack.
    void EndJumpAttack()
    {
        // Print a debug message.
        Debug.Log("Jump attack finished!");

        // Exit the attacking state.
        isAttacking = false;

        // Stop all movement.
        rb.linearVelocity = Vector2.zero;

        // Tell the Animator that the attack has finished.
        animator.SetBool("IsAttacking", false);

        // Calculate the distance to the player.
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Only attempt to damage the player if they are close enough.
        if (distanceToPlayer <= 2f)
        {
            // Calculate the direction towards the player.
            Vector2 direction = (player.position - transform.position).normalized;

            // Create a LayerMask so the raycast only detects the player and walls.
            LayerMask mask = LayerMask.GetMask("Player", "Wall");

            // Fire a raycast to check whether a wall blocks the attack.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distanceToPlayer, mask);

            // Damage the player only if they were hit directly.
            if (hit.collider != null && hit.collider.transform == player)
            {
                DamagePlayer();
            }
        }

        // Return to the idle animation.
        UpdateAnimator(lastDirection, 0f);
    }

    // Deals damage to the player.
    void DamagePlayer()
    {
        // Find the PlayerController.
        PlayerController playerController =
            PlayerController.Instance != null ?
            PlayerController.Instance :
            player.GetComponent<PlayerController>();

        // If the player exists...
        if (playerController != null)
        {
            // Deal damage to the player.
            playerController.TakeDamage(attackDamage);

            // Print a debug message.
            Debug.Log("Slime damaged player for " + attackDamage + "!");
        }
    }

    // Updates the movement animations.
    void UpdateAnimator(Vector2 direction, float speed)
    {
        // Exit if there is no Animator component.
        if (animator == null) return;

        // Update the animation direction.
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);

        // Update the movement speed.
        animator.SetFloat("Speed", speed);

        // Flip the sprite depending on movement direction.
        if (direction.x < -0.1f)
            spriteRenderer.flipX = true;
        else if (direction.x > 0.1f)
            spriteRenderer.flipX = false;
    }

    // Reduces the slime's health when it is damaged.
    public void TakeDamage(float damage)
    {
        // Subtract the damage from the slime's health.
        health -= damage;

        // Print the remaining health.
        Debug.Log("Slime took " + damage + " damage! Health: " + health);

        // Start the flash-red effect.
        StartCoroutine(FlashRed());

        // Face the last movement direction during the hurt animation.
        animator.SetFloat("MoveX", lastDirection.x);
        animator.SetFloat("MoveY", lastDirection.y);

        // Play the hurt animation.
        animator.SetTrigger("Hurt");

        // Flip the sprite if necessary.
        if (lastDirection.x < -0.1f)
            spriteRenderer.flipX = true;
        else if (lastDirection.x > 0.1f)
            spriteRenderer.flipX = false;

        // If the slime has no health remaining...
        if (health <= 0)
        {
            // Kill the slime.
            Die();
        }
    }

    // Makes the slime briefly flash red after taking damage.
    System.Collections.IEnumerator FlashRed()
    {
        // Change the sprite colour to red.
        spriteRenderer.color = Color.red;

        // Wait for 0.1 seconds.
        yield return new WaitForSeconds(0.1f);

        // Restore the original colour.
        spriteRenderer.color = originalColor;
    }

    // Handles the slime's death.
    public void Die()
    {
        // Print a debug message.
        Debug.Log("Slime died!");

        // Check if the CurrencyManager exists.
        if (CurrencyManager.Instance != null)
        {
            // Get the player's current coin multiplier.
            float coinMultiplier = PlayerController.Instance != null ?
                PlayerController.Instance.currentCoinMultiplier : 1f;

            // Calculate the minimum currency that can be dropped.
            int minCurrency = Mathf.Max(1,
                Mathf.FloorToInt((currencyDropAmount - currencyDropVariance) * coinMultiplier));

            // Calculate the maximum currency that can be dropped.
            int maxCurrency = Mathf.Max(minCurrency,
                Mathf.CeilToInt((currencyDropAmount + currencyDropVariance) * coinMultiplier));

            // Choose a random amount within the range.
            int actualCurrency = Random.Range(minCurrency, maxCurrency + 1);

            // Add the currency to the player's total.
            CurrencyManager.Instance.AddCurrency(actualCurrency);

            // Print the amount of currency dropped.
            Debug.Log($"Dropped {actualCurrency} gold (range {minCurrency}-{maxCurrency}, multiplier {coinMultiplier}x)!");
        }

        // Stop any movement or attacks.
        isAttacking = false;
        isWalking = false;

        // Stop the Rigidbody's movement.
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // Play the death animation.
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Disable this script
        this.enabled = false;

        // Destroy the GameObject after one second.
        Destroy(gameObject, 1f);
    }

    // Draws circles in the Scene view.
    void OnDrawGizmosSelected()
    {
        // Draw the enemy's detection range in yellow.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw the attack range in red.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw the stopping distance in blue.
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}