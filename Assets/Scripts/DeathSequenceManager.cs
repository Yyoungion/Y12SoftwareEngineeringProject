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
        
        // Add velocity to make them fall
        Rigidbody2D rb = slimeInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.down * Random.Range(5f, 15f);
            rb.angularVelocity = Random.Range(-360f, 360f);
        }
    }
    }