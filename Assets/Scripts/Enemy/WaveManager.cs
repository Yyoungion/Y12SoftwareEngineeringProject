using UnityEngine; // Imports the Unity engine.
using System.Collections; // Imports support for things such as IEnumerator.

public class WaveManager : MonoBehaviour // Defines the WaveManager class.
{
    [Header("Wave Settings")] // Groups wave settings in the Inspector.
    public int currentWave = 0; // Stores the current wave number.
    public int enemiesPerWave = 5; // Base number of enemies in the first wave.
    public float timeBetweenWaves = 10f; // Time to wait before starting the next wave.

    public float enemiesPerWaveMultiplier = 1f; // Multiplier used to increase enemies each wave.

    [Header("Spawning")] // Groups spawning settings in the Inspector.
    public GameObject slimePrefab; // Prefab of the slime enemy to spawn.
    public Transform[] spawnPoints; // Array of possible spawn locations.
    public float spawnDelay = 0.5f; // Delay between spawning each enemy.

    // Tracks how many enemies are in the current wave.
    private int enemiesRemainingInWave;

    // Indicates whether a wave is currently active.
    private bool waveActive = false;
    void Start()
    {
        // Begin the first wave after the delay.
        StartCoroutine(StartNextWave());
    }

    // Called once every frame.
    void Update()
    {
        // Only check for completion if a wave is active.
        if (waveActive)
        {
            // Count the number of SlimeEnemy objects currently alive.
            int enemiesAlive = FindObjectsOfType<SlimeEnemy>().Length;

            // If no enemies remain...
            if (enemiesAlive == 0)
            {
                // Mark the wave as finished.
                waveActive = false;

                // Print a message in the Console.
                Debug.Log("Wave Complete!");

                // Begin the next wave.
                StartCoroutine(StartNextWave());
            }
        }
    }

    // Coroutine that waits before starting the next wave.
    IEnumerator StartNextWave()
    {
        // Display the countdown until the next wave.
        Debug.Log($"Next wave in {timeBetweenWaves} seconds...");

        // Wait for the specified time.
        yield return new WaitForSeconds(timeBetweenWaves);

        // Increase the wave number.
        currentWave++;

        // Use the default wave multiplier.
        float waveMultiplier = enemiesPerWaveMultiplier;

        // If the DifficultyManager exists...
        if (DifficultyManager.Instance != null)
        {
            // Get the multiplier for the selected difficulty.
            waveMultiplier = DifficultyManager.Instance.GetEnemiesPerWaveMultiplier();
        }

        // Calculate the number of enemies in this wave.
        enemiesRemainingInWave = Mathf.RoundToInt(
            enemiesPerWave * Mathf.Pow(waveMultiplier, currentWave - 1));

        // Print the wave information.
        Debug.Log($"=== WAVE {currentWave} START === ({enemiesRemainingInWave} enemies)");

        // Mark the wave as active.
        waveActive = true;

        // Spawn each enemy with a delay.
        for (int i = 0; i < enemiesRemainingInWave; i++)
        {
            // Spawn one enemy.
            SpawnEnemy();

            // Wait before spawning the next one.
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Spawns a single slime enemy.
    void SpawnEnemy()
    {
        // Check that the prefab and spawn points have been assigned.
        if (slimePrefab == null || spawnPoints.Length == 0)
        {
            // Display an error if setup is incomplete.
            Debug.LogError("Slime prefab or spawn points not set!");
            return;
        }

        // Select a random spawn point.
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Create a new slime at the selected location.
        GameObject slimeInstance = Instantiate(slimePrefab, spawnPoint.position, Quaternion.identity);

        // Apply the current difficulty settings to the spawned enemy.
        if (DifficultyManager.Instance != null)
        {
            // Get the SlimeEnemy component.
            SlimeEnemy slimeAI = slimeInstance.GetComponent<SlimeEnemy>();

            // If the component exists
            if (slimeAI != null)
            {
                // Update its stats based on the selected difficulty.
                DifficultyManager.Instance.UpdateEnemyForDifficulty(slimeAI);
            }
        }
    }
}