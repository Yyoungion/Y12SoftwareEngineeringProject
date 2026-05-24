using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int currentWave = 0;
    public int enemiesPerWave = 5;
    public float enemiesPerWaveMultiplier = 1.5f;
    public float timeBetweenWaves = 10f;
    
    [Header("Spawning")]
    public GameObject slimePrefab;
    public Transform[] spawnPoints;
    public float spawnDelay = 0.5f;
    
    private int enemiesRemainingInWave;
    private bool waveActive = false;
    
    void Start()
    {
        StartCoroutine(StartNextWave());
    }
    
    void Update()
    {
        // Check if all enemies defeated
        if (waveActive)
        {
            int enemiesAlive = FindObjectsOfType<SlimeEnemy>().Length;
            
            if (enemiesAlive == 0)
            {
                waveActive = false;
                Debug.Log("Wave Complete!");
                StartCoroutine(StartNextWave());
            }
        }
    }
    
    IEnumerator StartNextWave()
    {
        // Wait between waves
        Debug.Log($"Next wave in {timeBetweenWaves} seconds...");
        yield return new WaitForSeconds(timeBetweenWaves);
        
        // Start new wave
        currentWave++;
        enemiesRemainingInWave = Mathf.RoundToInt(enemiesPerWave * Mathf.Pow(enemiesPerWaveMultiplier, currentWave - 1));
        
        Debug.Log($"=== WAVE {currentWave} START === ({enemiesRemainingInWave} enemies)");
        waveActive = true;
        
        // Spawn enemies
        for (int i = 0; i < enemiesRemainingInWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    
    void SpawnEnemy()
    {
        if (slimePrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Slime prefab or spawn points not set!");
            return;
        }
        
        // Pick random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        // Spawn enemy
        Instantiate(slimePrefab, spawnPoint.position, Quaternion.identity);
    }
}