using System.Collections;
using UnityEngine;

public class MovingTargetScript : MonoBehaviour
{
    public float speed = 2f; // Speed of the target's movement
    public AudioClip targetDestroyedSound1; // First destruction sound
    public AudioClip targetDestroyedSound2; // Second destruction sound
    
    private Vector2 movementDirection;
    private AudioSource audioSource;
    private ScoreMasterScript scoreMaster;

    void Start()
    {
        // Get reference to AudioSource
        audioSource = GetComponent<AudioSource>();

        // Get reference to ScoreMasterScript in the scene
        scoreMaster = FindObjectOfType<ScoreMasterScript>();

        // Set a random speed for the target
        speed = Random.Range(1f, 5f); // Speed between slow and fast

        // Set default movement direction
        if (movementDirection == Vector2.zero)
        {
            SetRandomMovementDirection();
        }
    }

    void Update()
    {
        // Move the target in the chosen direction
        transform.Translate(movementDirection * speed * Time.deltaTime);
    }

    private void SetRandomMovementDirection()
    {
        int randomDirection = Random.Range(0, 4); // Random value between 0 and 3

        switch (randomDirection)
        {
            case 0:
                movementDirection = Vector2.right; // Move right
                break;
            case 1:
                movementDirection = Vector2.left; // Move left
                break;
            case 2:
                movementDirection = Vector2.up; // Move up
                break;
            case 3:
                movementDirection = Vector2.down; // Move down
                break;
        }
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
        AudioClip destructionSound = Random.value > 0.5f ? targetDestroyedSound1 : targetDestroyedSound2;

        // Play destruction sound
        PlaySound(destructionSound);

        // Add score
        AddScore(75);

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
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }
    public void SetDirection(string direction)
    {
        switch (direction.ToLower())
        {
            case "right":
                movementDirection = Vector2.right;
                break;
            case "left":
                movementDirection = Vector2.left;
                break;
            case "up":
                movementDirection = Vector2.up;
                break;
            case "down":
                movementDirection = Vector2.down;
                break;
        }
    }
}
