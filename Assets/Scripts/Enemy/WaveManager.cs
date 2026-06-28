using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int currentWave = 0;
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 10f;

    public float enemiesPerWaveMultiplier = 1f; 
    
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
        Debug.Log($"Next wave in {timeBetweenWaves} seconds...");
        yield return new WaitForSeconds(timeBetweenWaves);
        
        currentWave++;
        
        
        float waveMultiplier = enemiesPerWaveMultiplier;
        if (DifficultyManager.Instance != null)
        {
            waveMultiplier = DifficultyManager.Instance.GetEnemiesPerWaveMultiplier();
        }
        
        enemiesRemainingInWave = Mathf.RoundToInt(enemiesPerWave * Mathf.Pow(waveMultiplier, currentWave - 1));
        
        Debug.Log($"=== WAVE {currentWave} START === ({enemiesRemainingInWave} enemies)");
        waveActive = true;
        
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
        
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject slimeInstance = Instantiate(slimePrefab, spawnPoint.position, Quaternion.identity);
        
        
        if (DifficultyManager.Instance != null)
        {
            SlimeEnemy slimeAI = slimeInstance.GetComponent<SlimeEnemy>();
            if (slimeAI != null)
            {
                DifficultyManager.Instance.UpdateEnemyForDifficulty(slimeAI);
            }
        }
    }
}