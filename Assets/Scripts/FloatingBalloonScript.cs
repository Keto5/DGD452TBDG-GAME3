using UnityEngine;

public class FloatingBalloonScript : MonoBehaviour
{
    // Speed range for floating upwards
    [SerializeField] private float minFloatSpeed = 0.5f;
    [SerializeField] private float maxFloatSpeed = 1.5f;

    // Side-to-side sway speed and range
    [SerializeField] private float minSwaySpeed = 1.0f;
    [SerializeField] private float maxSwaySpeed = 3.0f;
    [SerializeField] private float minSwayRange = 0.2f;
    [SerializeField] private float maxSwayRange = 1.0f;

    // Reference to popping sounds
    [SerializeField] private AudioClip popSound1;
    [SerializeField] private AudioClip popSound2;
    
    public Sprite[] baloonSprites; // Array of sprites to choose from
    private AudioSource audioSource; // Reference to the AudioSource
    private ScoreMasterScript scoreMaster; // ScoreMaster reference
    private PlayerRevolverScript playerRevolver; // Reference to the PlayerRevolverScript
    private float floatSpeed; // Floating speed
    private float swaySpeed; // Sway speed
    private float swayRange; // Sway range
    private Vector3 initialPosition; // Initial position for swaying
    private const string destroyTargetsTag = "DestroyTargetsHitbox"; // DestroyTargetsHitbox tag
    private bool isPopped = false; // Whether this balloon has been popped
    private SpriteRenderer spriteRenderer; // Sprite renderer
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Randomly choose a sprite for the balloon
        if (baloonSprites != null && baloonSprites.Length > 0)
        {
            spriteRenderer.sprite = baloonSprites[Random.Range(0, baloonSprites.Length)];
        }
        
        // Set a random float speed
        floatSpeed = Random.Range(minFloatSpeed, maxFloatSpeed);

        // Randomize sway speed and range
        swaySpeed = Random.Range(minSwaySpeed, maxSwaySpeed);
        swayRange = Random.Range(minSwayRange, maxSwayRange);

        // Save initial position for swaying
        initialPosition = transform.position;

        // Get reference to AudioSource
        audioSource = GetComponent<AudioSource>();

        // Get reference to ScoreMasterScript
        GameObject scoreMasterObject = GameObject.Find("ScoreMaster");
        if (scoreMasterObject != null)
        {
            scoreMaster = scoreMasterObject.GetComponent<ScoreMasterScript>();
        }
        else
        {
            Debug.LogError("ScoreMaster object not found in the scene.");
        }

        // Get reference to PlayerRevolverScript
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerRevolver = playerObject.GetComponent<PlayerRevolverScript>();
        }
        else
        {
            Debug.LogError("Player object with PlayerRevolverScript not found.");
        }

        // Ensure object is tagged as "Target"
        gameObject.tag = "Target";
    }

    void Update()
    {
        // Float upwards
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Sway side to side
        float swayOffset = Mathf.Sin(Time.time * swaySpeed) * swayRange;
        transform.position = new Vector3(initialPosition.x + swayOffset, transform.position.y, transform.position.z);
    }

    /*void OnMouseDown()
    {
        // Destroy only if clicked and not already popped
        if (!isPopped)
        {
            PopBalloon();
        }
    }*/

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider has the "DestroyTargetsHitbox" tag
        if (collision.CompareTag("DestroyTargetsHitbox"))
        {
            // Destroy without playing sound effects
            Destroy(gameObject);
        }
    }

    // Called when the player shoots the balloon
    public void ShootBalloon()
    {
        print("womp");
        // Check for bullets and hammer status in the revolver before popping the balloon
        if (!isPopped && playerRevolver != null && playerRevolver.currentBullets > 0 && playerRevolver.hammerPulledBack)
        {
            playerRevolver.currentBullets--; // Deduct one bullet
            playerRevolver.hammerPulledBack = false; // Reset hammer
            //PopBalloon();
        }
    }

    public void PopBalloon()
    {
        print("womp womp");
        isPopped = true;

        // Randomly choose a popping sound
        AudioClip chosenSound = Random.value > 0.5f ? popSound1 : popSound2;

        if (audioSource != null && chosenSound != null)
        {
            audioSource.PlayOneShot(chosenSound);
        }

        // Add score
        if (scoreMaster != null)
        {
            scoreMaster.AddPoints(100);
        }

        // Destroy the balloon after sound finishes
        Destroy(gameObject, 0.2f);
    }
}
