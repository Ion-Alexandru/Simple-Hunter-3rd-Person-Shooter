using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Shooting variables
    public Transform bulletProjectilePrefab;
    public Transform abilityProjectilePrefab;
    public Transform spawnBulletPosition;
    public float fireRate = 0.5f;

    public bool isShooting = false;
    public bool canShoot = true;
    public bool canUseAbility = true;
    public float abilityCooldown = 10;
    public float abilityCooldownTimer = 0;

    public float verticalRotationLimit = 80f;

    public ParticleSystem shootingParticalSystem;

    private Coroutine shootingCoroutine;

    // Player stats variables
    public int playerMaxHP = 200;
    public int playerHP;
    public int playerLevel = 0;
    public int ammoCount = 100;

    // Player stats
    public TextMeshProUGUI playerHPBar;
    public Slider experienceBar;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI levelText;

    void Start()
    {
        // Display the hp of the player at the start of the scene
        playerHP = playerMaxHP;
        playerHPBar.text = playerHP.ToString();

        // Display the ammo amount at the start of the game
        ammoText.text = ammoCount.ToString();

        experienceBar.value = 0;
        levelText.text = playerLevel.ToString();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canShoot)
            {
                shootingCoroutine = StartCoroutine(ShootCoroutine());
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                isShooting = false;
            }
        }

        if (Input.GetMouseButton(1))
        {
            if(canUseAbility)
            {
                StartCoroutine(AbilityCoroutine());
            }
        }

        canShoot = ammoCount > 0;
    }

    IEnumerator ShootCoroutine()
    {
        isShooting = true;

        while (canShoot)
        {
            Shoot();
            shootingParticalSystem.Play();
            yield return new WaitForSeconds(fireRate);
        }

        isShooting = false;
    }

    IEnumerator AbilityCoroutine()
    {
        canUseAbility = false;

        UseAbility();
        shootingParticalSystem.Play();

        abilityCooldownTimer = abilityCooldown;

        while (abilityCooldownTimer > 0)
        {
            abilityCooldownTimer -= Time.deltaTime;
            yield return null;
        }

        canUseAbility = true;
    }

    void Shoot()
    {
        // Apply camera shake for weapon weight
        ShootingShakeScript.Instance.ShakeCamera(1.5f, 0.1f);

        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            mouseWorldPosition = raycastHit.point;
        }

        Vector3 aimDirection = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        Instantiate(bulletProjectilePrefab, spawnBulletPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));

        ammoCount -= 1;
        ammoText.text = ammoCount.ToString();
    }

    public void UpdateExperinceBar(float value)
    {
        experienceBar.value += value;

        if (experienceBar.value >= 1)
        {
            playerLevel += 1;
            levelText.text = playerLevel.ToString();
            experienceBar.value -= 1;
        }
    }

    public void TakeDamage(int damage)
    {
        playerHP -= damage;

        // Display the hp of the player
        playerHPBar.text = playerHP.ToString();
    }

    void UseAbility()
    {
        // Apply camera shake for weapon weight
        ShootingShakeScript.Instance.ShakeCamera(1.5f, 0.1f);

        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            mouseWorldPosition = raycastHit.point;
        }

        Vector3 aimDirection = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        Instantiate(abilityProjectilePrefab, spawnBulletPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));
    }
}