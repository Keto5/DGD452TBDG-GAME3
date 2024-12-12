using UnityEngine;
using System.Collections;

public class BottleSpawnerScript : MonoBehaviour
{
    // Public variables for customization
    public GameObject bottlePrefab; // The bottle prefab to spawn
    public float spawnAreaWidth = 10f; // Width of the spawn area
    public float spawnAreaHeight = 5f; // Height of the spawn area
    public float minSpawnInterval = 2f; // Minimum time between spawns
    public float maxSpawnInterval = 10f; // Maximum time between spawns
    public bool spawnWithDelay = false; // Determines spawn method (simultaneous or delayed)

    private float timeSinceLastSpawn = 0f; // Tracks time since last spawn
    private float nextSpawnTime; // Determines the time for the next spawn

    void Start()
    {
        // Set the initial spawn time
        ScheduleNextSpawn();
    }

    void Update()
    {
        // Update the time since the last spawn
        timeSinceLastSpawn += Time.deltaTime;

        // Check if it's time to spawn bottles
        if (timeSinceLastSpawn >= nextSpawnTime)
        {
            SpawnBottles();
            timeSinceLastSpawn = 0f; // Reset the timer
            ScheduleNextSpawn(); // Schedule the next spawn
        }
    }

    void ScheduleNextSpawn()
    {
        // Determine the next spawn time with weighted randomness
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);

        // Increase the likelihood of spawning as time increases
        float weight = Mathf.InverseLerp(minSpawnInterval, maxSpawnInterval, nextSpawnTime);
        nextSpawnTime = Mathf.Lerp(minSpawnInterval, maxSpawnInterval, weight);
    }

    void SpawnBottles()
    {
        // Randomly decide how many bottles to spawn
        int bottleCount = Random.Range(0f, 1f) < 0.8f ? 1 : Random.Range(2, 7);

        if (spawnWithDelay)
        {
            // Spawn bottles one after another with a delay
            StartCoroutine(SpawnBottlesWithDelay(bottleCount));
        }
        else
        {
            // Spawn all bottles simultaneously
            for (int i = 0; i < bottleCount; i++)
            {
                SpawnBottle();
            }
        }
    }

    IEnumerator SpawnBottlesWithDelay(int count)
    {
        print("womp");
        for (int i = 0; i < count; i++)
        {
            SpawnBottle();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnBottle()
    {
        // Random position within the spawn area
        float randomX = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
        float randomY = Random.Range(-spawnAreaHeight / 2f, spawnAreaHeight / 2f);
        Vector3 spawnPosition = transform.position + new Vector3(randomX, randomY, 0);

        // Instantiate a bottle at the calculated position
        Instantiate(bottlePrefab, spawnPosition, Quaternion.identity);
    }

    // Visualize the spawn area in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaWidth, spawnAreaHeight, 0));
    }
}
