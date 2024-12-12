using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawnerScript : MonoBehaviour
{
    // Public variables for customization
    public GameObject balloonPrefab; // The Balloon prefab to spawn
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

        // Check if it's time to spawn Balloon
        if (timeSinceLastSpawn >= nextSpawnTime)
        {
            SpawnBalloons();
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

    void SpawnBalloons()
    {
        // Randomly decide how many Balloon to spawn
        int balloonCount = Random.Range(0f, 1f) < 0.8f ? 1 : Random.Range(2, 7);

        if (spawnWithDelay)
        {
            // Spawn Balloon one after another with a delay
            StartCoroutine(SpawnBalloonsWithDelay(balloonCount));
        }
        else
        {
            // Spawn all bottles simultaneously
            for (int i = 0; i < balloonCount; i++)
            {
                SpawnBalloon();
            }
        }
    }

    IEnumerator SpawnBalloonsWithDelay(int count)
    {
        print("womp");
        for (int i = 0; i < count; i++)
        {
            SpawnBalloon();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnBalloon()
    {
        // Random position within the spawn area
        float randomX = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
        float randomY = Random.Range(-spawnAreaHeight / 2f, spawnAreaHeight / 2f);
        Vector3 spawnPosition = transform.position + new Vector3(randomX, randomY, 0);

        // Instantiate a Balloon at the calculated position
        Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);
    }

    // Visualize the spawn area in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaWidth, spawnAreaHeight, 0));
    }
}
