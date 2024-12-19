using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRevolverScript : MonoBehaviour
{
    public GameObject crosshairPrefab; // Assign the PlayerCrosshair prefab
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

    void Start()
    {
        mainCamera = Camera.main;
        currentBullets = maxBullets;

        // Spawn crosshair
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
        hammerPulledBack = true;
        PlaySoundAtCenter(revolverHammerClickSound);
    }

    void Shoot()
    {
        if (isReloading) // Prevent shooting while reloading
            return;

        if (hammerPulledBack)
        {
            if (currentBullets > 0)
            {
                // Decrease bullets and play shooting sound
                currentBullets--;
                hammerPulledBack = false;
                PlaySoundAtCenter(revolverShootSound);

                // Raycast to check for hit
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                if (hit.collider != null && hit.collider.CompareTag("Target"))
                {
                    // Call the bottle's HitByBullet method
                    ThrownBottleScript bottleScript = hit.collider.GetComponent<ThrownBottleScript>();
                    if (bottleScript != null)
                    {
                        bottleScript.HitByBullet();
                        PlaySoundAtCenter(hitTargetSound);
                    }
                    FloatingBalloonScript floatingBalloonScript = hit.collider.GetComponent<FloatingBalloonScript>();
                    if (floatingBalloonScript != null)
                    {
                        floatingBalloonScript.PopBalloon();
                        PlaySoundAtCenter(hitTargetSound);
                    }
                    MovingTargetScript movingTargetScript = hit.collider.GetComponent<MovingTargetScript>();
                    if (movingTargetScript != null)
                    {
                        movingTargetScript.HitByBullet();
                        PlaySoundAtCenter(hitTargetSound);
                    }
                    BanditScript banditScript = hit.collider.GetComponent<BanditScript>();
                    if (banditScript != null)
                    {
                        banditScript.HitByBullet();
                        PlaySoundAtCenter(hitTargetSound);
                    }
                }
            }
            else
            {
                // Play empty revolver click sound
                PlaySoundAtCenter(revolverEmptyClickSound);
            }
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
            PlaySoundAtCenter(revolverReloadLastSound);
        }
        else
        {
            // Play a random reload sound
            PlaySoundAtCenter(Random.value > 0.5f ? revolverReload1Sound : revolverReload2Sound);
        }
    }

    void PlaySoundAtCenter(AudioClip clip)
    {
        if (clip != null)
        {
            Vector3 screenCenterPosition = Camera.main.transform.position;
            screenCenterPosition.z = -10; // Ensure it's on the same Z plane for 2D sound
            AudioSource.PlayClipAtPoint(clip, screenCenterPosition);
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
