using UnityEngine;

public class BanditSpawnerScript : MonoBehaviour
{
    public GameObject banditPrefab;
    public float spawnAreaWidth = 10f;
    public float spawnAreaHeight = 5f;
    public string spawnDirection = "right"; // Can be "left", "right", "up", or "down"
    public bool enableRoundRobin = true; // Toggle for round-robin spawning

    private float timeSinceLastSpawn = 0f;
    private float nextSpawnTime;

    private static int currentSpawnerIndex = 0;
    private static BanditSpawnerScript[] spawners;

    private float minimumSpawnTime = 10f; // Starting minimum time
    private float maximumSpawnTime = 15f; // Starting maximum time
    private const float minimumSpawnTimeLimit = 2f; // Hard cap for minimum spawn time
    private const float spawnTimeReduction = 0.5f; // Amount to decrease minimum spawn time

    private void Start()
    {
        if (enableRoundRobin)
        {
            // Initialize static spawner list if not already initialized
            if (spawners == null || spawners.Length == 0)
            {
                spawners = FindObjectsOfType<BanditSpawnerScript>();
            }
        }

        // Set initial spawn time
        nextSpawnTime = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= nextSpawnTime && CanSpawn())
        {
            SpawnBandit();

            // Decrease the minimum spawn time
            minimumSpawnTime = Mathf.Max(minimumSpawnTime - spawnTimeReduction, minimumSpawnTimeLimit);

            // Update the next spawn time based on the new minimum
            nextSpawnTime = Random.Range(minimumSpawnTime, maximumSpawnTime);
            timeSinceLastSpawn = 0f;

            // Update round-robin index
            if (enableRoundRobin)
            {
                currentSpawnerIndex = (currentSpawnerIndex + 1) % spawners.Length;
            }
        }
    }

    private bool CanSpawn()
    {
        // If round-robin is enabled, only allow the current spawner to spawn
        return !enableRoundRobin || spawners[currentSpawnerIndex] == this;
    }

    private void SpawnBandit()
    {
        // Random position within the spawn area
        float randomX = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
        float randomY = Random.Range(-spawnAreaHeight / 2f, spawnAreaHeight / 2f);
        Vector3 spawnPosition = transform.position + new Vector3(randomX, randomY, 0);

        // Create the bandit
        GameObject newBandit = Instantiate(banditPrefab, spawnPosition, Quaternion.identity);

        // Set the bandit's direction
        BanditScript banditScript = newBandit.GetComponent<BanditScript>();
        if (banditScript != null)
        {
            SetBanditDirection(newBandit.transform);

            // Play random spawn sound
            banditScript.PlayRandomSpawnSoundAtCenter();
        }
    }

    private void SetBanditDirection(Transform banditTransform)
    {
        switch (spawnDirection.ToLower())
        {
            case "right":
                banditTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case "left":
                banditTransform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case "up":
                banditTransform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case "down":
                banditTransform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            default:
                Debug.LogWarning("Invalid spawn direction. Defaulting to 'right'.");
                banditTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaWidth, spawnAreaHeight, 0));
    }
}
