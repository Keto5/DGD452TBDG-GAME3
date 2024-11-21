using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRevolverScript : MonoBehaviour
{
    public GameObject crosshairPrefab; // Assign the PlayerCrosshair prefab in the Inspector
    private GameObject crosshairInstance;

    public AudioClip revolverShootSound;
    public AudioClip revolverHammerClickSound;
    public AudioClip revolverEmptyClickSound;
    public AudioClip revolverReload1Sound;
    public AudioClip revolverReload2Sound;
    public AudioClip revolverReloadLastSound;
    public AudioClip hitTargetSound;

    public int maxBullets = 6; // Maximum bullets the revolver can hold
    public int currentBullets;

    public bool hammerPulledBack = true;
    private bool isReloading = false;

    private Camera mainCamera;
    private AudioSource audioSource;

    void Start()
    {
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        currentBullets = maxBullets;

        // Spawn crosshair instance
        crosshairInstance = Instantiate(crosshairPrefab);
        crosshairInstance.SetActive(true);
    }

    void Update()
    {
        UpdateCrosshairPosition();

        if (Input.GetMouseButtonDown(1) && !isReloading) // Right click to pull hammer
        {
            PullBackHammer();
        }

        if (Input.GetMouseButtonDown(0)) // Left click to shoot
        {
            if (IsCursorOverReloadHitbox())
            {
                ReloadOneBullet();
            }
            else
            {
                Shoot();
            }
        }
    }

    void UpdateCrosshairPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Distance from the camera
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        crosshairInstance.transform.position = worldPosition;
    }

    void PullBackHammer()
    {
        /*if (hammerPulledBack || currentBullets <= 0)
        {
            return;
        }*/

        hammerPulledBack = true;
        PlaySound(revolverHammerClickSound);
    }

    void Shoot()
    {
        if (isReloading) // Prevent shooting while reloading
            return;

        if (hammerPulledBack)
        {
            if (currentBullets > 0)
            {
                // Decrement bullets and play shooting sound
                currentBullets--;
                hammerPulledBack = false;
                PlaySound(revolverShootSound);

                // Raycast to check for hit
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                if (hit.collider != null && hit.collider.CompareTag("Target"))
                {
                    // Destroy target and play hit sound
                    Destroy(hit.collider.gameObject);
                    PlaySound(hitTargetSound);
                }
            }
            else
            {
                // Play empty revolver sound
                PlaySound(revolverEmptyClickSound);
                print("CLICK!");
                hammerPulledBack = false;
            }
        }
        else
        {
            // Play empty revolver sound
            PlaySound(revolverEmptyClickSound);
            print("CLICK!");
        }
    }

    bool IsCursorOverReloadHitbox()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        return hit.collider != null && hit.collider.CompareTag("ReloadHitbox");
    }

    void ReloadOneBullet()
    {
        if (currentBullets >= maxBullets)
            return;

        currentBullets++;

        if (currentBullets == maxBullets)
        {
            // Play full revolver reload sound
            PlaySound(revolverReloadLastSound);
        }
        else
        {
            // Play a random reload sound
            PlaySound(Random.value > 0.5f ? revolverReload1Sound : revolverReload2Sound);
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void OnDestroy()
    {
        if (crosshairInstance != null)
        {
            Destroy(crosshairInstance);
        }
    }
}
