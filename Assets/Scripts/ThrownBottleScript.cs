using UnityEngine;

public class ThrownBottleScript : MonoBehaviour
{
    public float throwForce = 10f; // Initial throw force
    public float throwArcAngle = 120f; // Arc angle for random direction
    public float minSpinSpeed = 100f; // Minimum spin speed (degrees per second)
    public float maxSpinSpeed = 500f; // Maximum spin speed (degrees per second)

    // Sprites for the bottle
    public Sprite[] bottleSprites; // Array of sprites to choose from

    // Sound effects
    public AudioClip bottleDestroyedSound1; // First destruction sound
    public AudioClip bottleDestroyedSound2; // Second destruction sound
    public AudioClip bottleThrownSound; // Sound played when bottle is created

    private ScoreMasterScript scoreMaster;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Randomly choose a sprite for the bottle
        if (bottleSprites != null && bottleSprites.Length > 0)
        {
            spriteRenderer.sprite = bottleSprites[Random.Range(0, bottleSprites.Length)];
        }

        // Randomize throw direction
        float randomAngle = Random.Range(-throwArcAngle / 2f, throwArcAngle / 2f);
        Vector2 throwDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
        rb.velocity = throwDirection * throwForce;

        // Apply random spin
        float randomSpin = Random.Range(minSpinSpeed, maxSpinSpeed);
        rb.angularVelocity = randomSpin * (Random.value > 0.5f ? 1 : -1);

        // Play thrown sound at the center of the screen
        PlaySound(bottleThrownSound);

        // Find ScoreMasterScript
        scoreMaster = FindObjectOfType<ScoreMasterScript>();

        // Ensure the bottle has the "Target" tag
        gameObject.tag = "Target";
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider has the "DestroyTargetsHitbox" tag
        if (collision.CompareTag("DestroyTargetsHitbox"))
        {
            // Destroy without playing sound effects
            Destroy(gameObject);
        }
    }

    public void HitByBullet()
    {
        // Randomly select a destruction sound
        AudioClip destructionSound = Random.value > 0.5f ? bottleDestroyedSound1 : bottleDestroyedSound2;

        // Play destruction sound at the center of the screen
        PlaySound(destructionSound);

        // Add score
        AddScore(100);

        // Destroy the bottle
        Destroy(gameObject);
    }

    private void AddScore(int points)
    {
        if (scoreMaster != null)
        {
            scoreMaster.AddPoints(points);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            // Get the center of the screen's position (camera's position)
            Vector3 screenCenterPosition = Camera.main.transform.position;
            screenCenterPosition.z = -10; // Ensure it's on the same Z plane for 2D sound

            // Play the sound at the screen's center
            AudioSource.PlayClipAtPoint(clip, screenCenterPosition);
        }
    }
}
