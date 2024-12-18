using UnityEngine;
using System.Collections;

public class MovingTargetSpawnerScript : MonoBehaviour
{
    // Public variables for customization
    public GameObject movingTargetPrefab; // The bottle prefab to spawn
    public float spawnAreaWidth = 10f; // Width of the spawn area
    public float spawnAreaHeight = 5f; // Height of the spawn area
    public string spawnDirection = "right"; // Direction to spawn targets

    private float timeSinceLastSpawn = 0f; // Tracks time since last spawn
    private float nextSpawnTime; // Determines the time for the next spawn

    void Start()
    {
        // Set initial spawn time between 5 and 10 seconds
        nextSpawnTime = Random.Range(5f, 10f);
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        // Check if it's time to spawn a target
        if (timeSinceLastSpawn >= nextSpawnTime)
        {
            SpawnTargets();
            nextSpawnTime = Random.Range(5f, 10f); // Set the next spawn time between 5 and 10 seconds
            timeSinceLastSpawn = 0f; // Reset the spawn timer
        }
    }

    private void SpawnTargets()
    {
        // Determine the number of targets to spawn (1 to 4, with a low chance for more than 1)
        int numberOfTargets = Random.Range(1, 5);

        for (int i = 0; i < numberOfTargets; i++)
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
        }
    }

    // Visualize the spawn area in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaWidth, spawnAreaHeight, 0));
    }
}
