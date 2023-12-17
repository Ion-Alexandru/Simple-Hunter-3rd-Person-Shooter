using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;
    public EnemyHealthBar enemyHealthBar;

    public float attackRange = 1.0f;
    public float attackCooldown = 1.0f;
    public int attackDamage = 20;

    public float flyingHeight = 10.0f; // The height at which the enemy will fly

    public float stoppingDistance = 2.0f;
    public int maxHealth = 40;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    private Transform player;
    private int currentHealth;

    private bool isAttacking = false;
    private bool cancelAttack = false;
    private float attackTimer = 0f;

    public float projectileSpeed = 15f;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isAttacking)
        {
            MoveTowardsPlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction from the enemy to the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Ignore vertical movement
        directionToPlayer.y = 0;

        // Rotate the enemy to face the player
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), 0.1f);

        // Move the enemy toward the player, but stop when within the stopping distance
        if (directionToPlayer.magnitude > stoppingDistance)
        {
            // Calculate the desired position at the flying height
            Vector3 targetPosition = player.position + Vector3.up * flyingHeight;

            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5.0f);
        }
        else
        {
            // Stop moving and start attacking
            isAttacking = true;
            attackTimer = 0f;
            cancelAttack = false;
        }
    }

    void AttackPlayer()
    {
        // Increment the attack timer
        attackTimer += Time.deltaTime;

        // If the player moves out of the attack range during the attack animation, cancel the attack
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            cancelAttack = true;
        }

        // If the attack cooldown has expired, shoot a projectile
        if (attackTimer >= attackCooldown)
        {
            ShootProjectile();

            // Reset attack-related variables
            isAttacking = false;
            attackTimer = 0f;
        }

        // If the player moved out of range during the attack, stop the attack and resume moving
        if (cancelAttack)
        {
            isAttacking = false;
        }
    }

    void ShootProjectile()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = player.position - projectileSpawnPoint.position;

        // Instantiate the projectile at the spawn point and with the calculated rotation
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(directionToPlayer.normalized));

        // Ignore collisions between the projectile and the enemy
        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());

        // Access the ProjectileMovement script to set the projectile speed
        ProjectileMovement projectileMovement = projectile.AddComponent<ProjectileMovement>();
        projectileMovement.speed = projectileSpeed;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        enemyHealthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            playerController.UpdateExperinceBar(0.1f);

            gameManager.enemiesKilled += 1;

            Die();
        }
    }

    void Die()
    {
        // Perform any actions when the enemy dies (e.g., play death animation, spawn particles, etc.)
        Destroy(gameObject);
    }
}