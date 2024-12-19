using UnityEngine;

public class BanditScript : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 4f;
    public Sprite banditShootPlayerSprite;
    public AudioClip banditShotSound1; // Shoot sound 1
    public AudioClip banditShotSound2; // Shoot sound 2
    public AudioClip banditSpawnSound1; // Spawn sound 1
    public AudioClip banditSpawnSound2; // Spawn sound 2

    private float speed;
    private bool isPlayerHit = false;
    private AudioSource audioSource;
    private float timeToShootPlayer;
    private ScoreMasterScript scoreMaster;

    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed); // Assign random speed
        
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        
        scoreMaster = FindObjectOfType<ScoreMasterScript>(); // Get reference to ScoreMasterScript in the scene
        
        timeToShootPlayer = Random.Range(2f, 4f); // Randomize the time to shoot the player (2 to 5 seconds)
        
        Invoke(nameof(ShootPlayer), timeToShootPlayer); // Start countdown to shoot the player
    }

    private void Update()
    {
        if (!isPlayerHit)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime); // Move in the assigned direction
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DestroyTargetsHitbox"))
        {
            Destroy(gameObject); // Destroy without sound
        }
    }

    public void PlayRandomSpawnSound()
    {
        if (audioSource != null)
        {
            // Randomly pick a spawn sound
            AudioClip spawnSound = Random.value > 0.5f ? banditSpawnSound1 : banditSpawnSound2;
            audioSource.PlayOneShot(spawnSound);
        }
    }

    public void HitByBullet()
    {
        Debug.Log("Bandit hit by bullet");
        
        AddScore(250); // Add score
        
        Destroy(gameObject, 0.1f); // Destroy the bandit and Delay slightly so sound plays
    }

    private void AddScore(int points)
    {
        if (scoreMaster != null)
        {
            scoreMaster.AddPoints(points);
        }
    }

    private void ShootPlayer()
    {
        if (!isPlayerHit)
        {
            isPlayerHit = true;

            // Change sprite to indicate shooting the player
            GetComponent<SpriteRenderer>().sprite = banditShootPlayerSprite;

            // Decrease player health
            PlayerHealthScript playerHealth = FindObjectOfType<PlayerHealthScript>();
            if (playerHealth != null)
            {
                playerHealth.DecreaseHealth(1);
            }

            // Destroy the bandit after shooting the player
            Destroy(gameObject, 0.5f);
        }
    }
}
