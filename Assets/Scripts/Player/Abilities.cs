using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class Abilities : MonoBehaviour
{
    public event Action<bool> onPlayerDashChange;

    [Header("Player Data")]
    [SerializeField] private PlayerData playerData;

    [Header("Wave Data")]
    [SerializeField] private WaveManager waveManager;

    [Header("Dash")]
    [SerializeField] private Image dashImage;
    [SerializeField] private Color dashColor = Color.cyan;
    [SerializeField] private ParticleSystem dustParticles;
    private float dashCooldown = 3f;
    private float dashCounter = 0;
    private float dashCoolDownCounter = 0;

    [Header("Slower")]
    [SerializeField] private GameObject slowerLogo;
    [SerializeField] private GameObject slowerText;
    [SerializeField] private Image slowerImage;
    [SerializeField] private Color slowerColor = Color.cyan;
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float slowerLifetime = 5f;
    [SerializeField] private float slowerZOffset = 5f;
    private float slowerCooldown = 3f;
    private bool isCooldownSlower = false;

    [Header("Laser")]
    [SerializeField] private GameObject laserLogo;
    [SerializeField] private GameObject laserText;
    [SerializeField] private Image laserImage;
    [SerializeField] private Color laserColor = Color.cyan;
    [SerializeField] private GameObject laserObject;

    [Header("Audio Manager")]
    [SerializeField] AudioManager audioManager;
    [SerializeField] string roll;
    [SerializeField] string splat;
    [SerializeField] string laser;

    private float laserCooldown = 3f;
    private bool isCooldownLaser = false;

    private int targetLayer;

    private void Start()
    {
        targetLayer = LayerMask.NameToLayer("Slower");
        dashCooldown = playerData.dashCooldown;
        slowerCooldown = playerData.shieldCooldown;
        laserCooldown = playerData.laserCooldown;

        dashImage.fillAmount = 0f;
        slowerImage.fillAmount = 0f;
        laserImage.fillAmount = 0f;
    }

    private void Update()
    {
        DashCounter();
        SlowerCounter();
        LaserCounter();
    }

    private void LaserCounter()
    {
        if (waveManager.currentWaveIndex >= 9)
        {
            laserLogo.SetActive(true);
            laserText.SetActive(true);

            if (isCooldownLaser)
            {
                laserImage.fillAmount -= 1 / laserCooldown * Time.deltaTime;

                if (laserImage.fillAmount <= 0f)
                {
                    laserImage.fillAmount = 0f;
                    isCooldownLaser = false;
                }
            }
        }
    }

    public void LaserLogic()
    {
        if (waveManager.currentWaveIndex >= 9)
        {
            if (!isCooldownLaser)
            {
                if (!AudioManager.muteSFX)
                {
                    audioManager.PlaySound(laser);
                }
                isCooldownLaser = true;
                laserImage.fillAmount = 1f;

                StartCoroutine(ActivateLaser());
            }
        }
    }

    private IEnumerator ActivateLaser()
    {
        laserObject.SetActive(true);
        yield return new WaitForSeconds(playerData.laserDuration);
        laserObject.SetActive(false);
    }

    private void SlowerCounter()
    {
        if (waveManager.currentWaveIndex >= 4)
        {
            slowerLogo.SetActive(true);
            slowerText.SetActive(true);

            if (isCooldownSlower)
            {
                slowerImage.fillAmount -= 1 / slowerCooldown * Time.deltaTime;

                if (slowerImage.fillAmount <= 0f)
                {
                    slowerImage.fillAmount = 0f;

                    isCooldownSlower = false;
                }
            }
        }
    }

    public void SlowerLogic()
    {
        if (waveManager.currentWaveIndex >= 4)
        {
            if (!isCooldownSlower)
            {
                if (!AudioManager.muteSFX)
                {
                    audioManager.PlaySound(splat);
                }
                isCooldownSlower = true;
                slowerImage.fillAmount = 1f;

                GameObject spawnedObject = Instantiate(prefabToSpawn, new Vector3(transform.position.x, transform.position.y, transform.position.z + slowerZOffset), Quaternion.Euler(-90f, 0f, 0f));

                StartCoroutine(DestroyAfterTime(spawnedObject));
            }
        }
    }

    private IEnumerator DestroyAfterTime(GameObject obj)
    {
        yield return new WaitForSeconds(slowerLifetime);
    }

    private void DashCounter()
    {
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                playerData.isDashing = false;
                onPlayerDashChange?.Invoke(playerData.isDashing);
                playerData.activeMoveSpeed = playerData.speed;
            }
        }

        if (dashCoolDownCounter > 0)
        {
            dashCoolDownCounter -= Time.deltaTime;
            dashImage.fillAmount = dashCoolDownCounter / playerData.dashCooldown;
        }
    }

    public void DashLogic()
    {
        if (dashCoolDownCounter <= 0 && dashCounter <= 0)
        {
            if (!AudioManager.muteSFX)
            {
                audioManager.PlaySound(roll);
            }
            playerData.isDashing = true;
            dustParticles.Play();
            onPlayerDashChange?.Invoke(playerData.isDashing);
            dashCoolDownCounter = playerData.dashCooldown;
            dashCounter = playerData.dashLength;
            playerData.activeMoveSpeed = playerData.dashSpeed;
            dashImage.fillAmount = 1f;
        }
    }

    public void DestroySlowers()
    {
        GameObject[] slowerObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in slowerObjects)
        {
            if (obj.layer == targetLayer)
            {
                Destroy(obj);
            }
        }
    }
}