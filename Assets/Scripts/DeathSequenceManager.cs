using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeathSequenceManager : MonoBehaviour
{
    public static DeathSequenceManager Instance { get; private set; }
    
    [Header("Slime Spawning")]
    public GameObject slimePrefab;
    public int slimeCount = 1000;
    public float spawnAreaWidth = 30f;
    public float spawnHeight = 20f;
    public float spawnRate = 0.001f; // Spawns per frame
    
    [Header("Timing")]
    public float sequenceDuration = 3f; // How long until death screen appears
    
    private bool isDeathSequenceActive = false;
    private int slimesSpawned = 0;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void StartDeathSequence()
    {
        if (isDeathSequenceActive) return;
        
        isDeathSequenceActive = true;
        
        // Pause the game
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.Pause();
        }
        else
        {
            Time.timeScale = 0f;
        }
        
        Debug.Log("Death sequence started!");
        
        StartCoroutine(DeathSequence());
    }
    
    IEnumerator DeathSequence()
    {
        // Get camera bounds to spawn slimes above screen
        Camera mainCamera = Camera.main;
        Vector3 cameraPos = mainCamera.transform.position;
        float screenTop = cameraPos.y + mainCamera.orthographicSize;
        
        // Spawn slimes over time
        float elapsedTime = 0f;
        
        while (slimesSpawned < slimeCount && elapsedTime < sequenceDuration * 0.7f)
        {
            for (int i = 0; i < Mathf.Max(1, Mathf.RoundToInt(slimeCount * spawnRate)); i++)
            {
                if (slimesSpawned >= slimeCount) break;
                
                SpawnSlime(cameraPos, screenTop);
                slimesSpawned++;
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Spawn remaining slimes
        while (slimesSpawned < slimeCount)
        {
            SpawnSlime(cameraPos, screenTop);
            slimesSpawned++;
            yield return new WaitForSeconds(0.001f);
        }
        
        // Wait for slimes to cover screen
        yield return new WaitForSecondsRealtime(sequenceDuration);
        
        // Show death screen
        ShowDeathScreen();
    }
    
    void SpawnSlime(Vector3 cameraPos, float spawnHeight)
    {
        if (slimePrefab == null)
        {
            Debug.LogError("Slime prefab not assigned to DeathSequenceManager!");
            return;
        }
        
        // Random X position across screen
        float randomX = Random.Range(
            cameraPos.x - spawnAreaWidth / 2,
            cameraPos.x + spawnAreaWidth / 2
        );
        
        // Spawn above screen
        Vector3 spawnPos = new Vector3(randomX, spawnHeight, 0);
        
        // Instantiate slime
        GameObject slimeInstance = Instantiate(slimePrefab, spawnPos, Quaternion.identity);
        
        // Disable AI so they just fall
        SlimeEnemy slimeAI = slimeInstance.GetComponent<SlimeEnemy>();
        if (slimeAI != null)
        {
            slimeAI.enabled = false;
        }
        
        // Fall during pause using unscaled time because 2D physics is frozen.
        DeathSequenceSlimeFall fall = slimeInstance.AddComponent<DeathSequenceSlimeFall>();
        fall.Initialize(Random.Range(5f, 15f), Random.Range(-360f, 360f));

        // Keep the rigidbody from competing with manual motion.
        Rigidbody2D rb = slimeInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f;
        }
    }
    
    void ShowDeathScreen()
    {
        // Unpause for UI
        Time.timeScale = 1f;
        
        // Find and show death screen
        DeathScreenController deathScreen = FindObjectOfType<DeathScreenController>();
        if (deathScreen != null)
        {
            deathScreen.ShowDeathScreen();
        }
        else
        {
            Debug.LogWarning("DeathScreenController not found!");
        }
    }
}

public class DeathSequenceSlimeFall : MonoBehaviour
{
    private float fallSpeed;
    private float spinSpeed;

    public void Initialize(float fallSpeed, float spinSpeed)
    {
        this.fallSpeed = fallSpeed;
        this.spinSpeed = spinSpeed;
    }

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.unscaledDeltaTime;
        transform.Rotate(0f, 0f, spinSpeed * Time.unscaledDeltaTime);
    }
}