using UnityEngine;

public class BanditSpawnerScript : MonoBehaviour
{
    public GameObject banditPrefab;
    public float spawnAreaWidth = 10f;
    public float spawnAreaHeight = 5f;
    public string spawnDirection = "right"; // Can be "left", "right", "up", or "down"

    private float timeSinceLastSpawn = 0f;
    private float nextSpawnTime;

    private void Start()
    {
        // Set the initial spawn time
        nextSpawnTime = Random.Range(10f, 15f);
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= nextSpawnTime)
        {
            SpawnBandit();
            nextSpawnTime = Random.Range(10f, 15f);
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnBandit()
    {
        // Random position within the spawn area
        float randomX = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
        float randomY = Random.Range(-spawnAreaHeight / 2f, spawnAreaHeight / 2f);
        Vector3 spawnPosition = transform.position + new Vector3(randomX, randomY, 0);

        // Instantiate the bandit
        GameObject newBandit = Instantiate(banditPrefab, spawnPosition, Quaternion.identity);

        // Set the bandit's direction
        BanditScript banditScript = newBandit.GetComponent<BanditScript>();
        if (banditScript != null)
        {
            SetBanditDirection(newBandit.transform);
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
