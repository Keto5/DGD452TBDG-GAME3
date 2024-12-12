using UnityEngine;
using System.Collections;

public class MovingTargetSpawnerScript : MonoBehaviour
{
    // Public variables for customization
    public GameObject movingTargetPrefab; // The bottle prefab to spawn
    public float spawnAreaWidth = 10f; // Width of the spawn area
    public float spawnAreaHeight = 5f; // Height of the spawn area
    public float minSpawnInterval = 1f; // Minimum time between spawns
    public float maxSpawnInterval = 3f; // Maximum time between spawns
    public string spawnDirection = "right"; // Direction to spawn targets

    private float timeSinceLastSpawn = 0f; // Tracks time since last spawn
    private float nextSpawnTime; // Determines the time for the next spawn
    private float spawnChanceIncreaseRate = 0.5f; // Chance increases every 0.5 seconds
    private float spawnChance = 0f; // Current spawn chance

    void Start()
    {
        // Set initial spawn time
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        // Increase spawn chance every 0.5 seconds
        if (timeSinceLastSpawn >= 0.5f)
        {
            spawnChance += spawnChanceIncreaseRate;
            timeSinceLastSpawn = 0f;
        }

        // Check if it's time to spawn a target
        if (spawnChance >= 100 || Random.value < spawnChance / 100f)
        {
            SpawnTargets();
            nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            spawnChance = 0f; // Reset spawn chance
        }
    }

    private void SpawnTargets()
    {
        // Random position within the spawn area
        float randomX = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
        float randomY = Random.Range(-spawnAreaHeight / 2f, spawnAreaHeight / 2f);
        Vector3 spawnPosition = transform.position + new Vector3(randomX, randomY, 0);

        GameObject newTarget = Instantiate(movingTargetPrefab, spawnPosition, Quaternion.identity);
        MovingTargetScript targetScript = newTarget.GetComponent<MovingTargetScript>();

        if (targetScript != null)
        {
            targetScript.SetDirection(spawnDirection);
        }
        
        
        // Instantiate a bottle at the calculated position
        Instantiate(movingTargetPrefab, spawnPosition, Quaternion.identity);
        
    }

    // Visualize the spawn area in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaWidth, spawnAreaHeight, 0));
    }
}