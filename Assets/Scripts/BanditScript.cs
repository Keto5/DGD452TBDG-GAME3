using UnityEngine;

public class BanditScript : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 5f;
    public Sprite banditShootPlayerSprite;
    public AudioClip banditShotSound1;
    public AudioClip banditShotSound2;

    private float speed;
    private bool isPlayerHit = false;
    private AudioSource audioSource;
    private float timeToShootPlayer;
    private ScoreMasterScript scoreMaster;

    private void Start()
    {
        // Assign random speed
        speed = Random.Range(minSpeed, maxSpeed);

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Get reference to ScoreMasterScript in the scene
        scoreMaster = FindObjectOfType<ScoreMasterScript>();
        
        // Randomize the time to shoot the player (2 to 5 seconds)
        timeToShootPlayer = Random.Range(2f, 5f);

        // Start countdown to shoot the player
        Invoke(nameof(ShootPlayer), timeToShootPlayer);
    }

    private void Update()
    {
        if (!isPlayerHit)
        {
            // Move in the assigned direction
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DestroyTargetsHitbox"))
        {
            Destroy(gameObject); // Destroy without sound
        }
    }

    /*private void OnMouseDown()
    {
        if (!isPlayerHit)
        {
            // Play one of the "BanditShot" sounds
            AudioClip clipToPlay = Random.value > 0.5f ? banditShotSound1 : banditShotSound2;
            audioSource.PlayOneShot(clipToPlay);

            // Add 250 points to the score
            //ScoreMasterScript.Instance.AddScore(250);

            // Destroy the bandit after the sound
            Destroy(gameObject, clipToPlay.length);
        }
    }*/
    
    public void HitByBullet()
    {
        // Randomly select a shot sound
        //AudioClip destructionSound = Random.value > 0.5f ? banditShotSound1 : banditShotSound2;

        // Play destruction sound
        //PlaySound(destructionSound);
        Debug.Log("Bluh bluh");
        // Add score
        AddScore(250);

        // Destroy the bandit
        Destroy(gameObject, 0.1f); // Delay slightly so sound plays
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
