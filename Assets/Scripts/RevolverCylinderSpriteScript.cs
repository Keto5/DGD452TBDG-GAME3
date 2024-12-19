using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverCylinderSpriteScript : MonoBehaviour
{
    public Sprite[] cylinderSprites; // Array of 7 sprites for the cylinder, from 0 to 6 bullets
    public PlayerRevolverScript playerRevolverScript; // Reference to the PlayerRevolverScript
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerRevolverScript == null)
        {
            Debug.LogError("PlayerRevolverScript is not assigned!");
        }

        if (cylinderSprites.Length != 7)
        {
            Debug.LogError("CylinderSprites array must contain exactly 7 sprites!");
        }
    }

    void Update()
    {
        UpdateCylinderSprite();
    }

    void UpdateCylinderSprite()
    {
        if (playerRevolverScript != null && spriteRenderer != null)
        {
            // Ensure the number of bullets is within the range [0, 6]
            int bullets = Mathf.Clamp(playerRevolverScript.currentBullets, 0, 6);

            // Set the sprite based on the current number of bullets in the revovler
            spriteRenderer.sprite = cylinderSprites[bullets];
        }
    }
}