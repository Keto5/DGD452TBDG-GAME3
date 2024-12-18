using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthScript : MonoBehaviour
{
    public int playerHealth = 3; // Starting health

    public Sprite healthSprite3; // Sprite for 3 health
    public Sprite healthSprite2; // Sprite for 2 health
    public Sprite healthSprite1; // Sprite for 1 health

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Get the SpriteRenderer component attached to the player
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Update the player's health sprite to the starting value
        UpdateHealthSprite();
    }

    public void DecreaseHealth(int amount)
    {
        playerHealth -= amount;

        // Ensure health does not go below 0
        playerHealth = Mathf.Max(playerHealth, 0);

        // Update the player's health sprite
        UpdateHealthSprite();

        // Check if health has reached 0
        if (playerHealth <= 0)
        {
            // Load the GameOver scene
            SceneManager.LoadScene("GameOverScene");
        }
    }

    private void UpdateHealthSprite()
    {
        // Update the sprite based on the player's current health
        switch (playerHealth)
        {
            case 3:
                spriteRenderer.sprite = healthSprite3;
                break;
            case 2:
                spriteRenderer.sprite = healthSprite2;
                break;
            case 1:
                spriteRenderer.sprite = healthSprite1;
                break;
            default:
                // Optionally handle cases where health is 0 or less
                spriteRenderer.sprite = null;
                break;
        }
    }
}