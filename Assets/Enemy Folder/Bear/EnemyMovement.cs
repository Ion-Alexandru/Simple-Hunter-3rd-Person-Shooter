using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;
    public EnemyHealthBar enemyHealthBar;
    [SerializeField] private Animator animator;

    public float attackRange = 1.0f;
    public float attackCooldown = 1.0f;
    public int attackDamage = 20;

    public float stoppingDistance = 2.0f;
    public int maxHealth = 40;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private int currentHealth;

    private bool isAttacking = false;
    private bool cancelAttack = false;
    private float attackTimer = 0f;

    bool isMoving = false;
    bool isIdle = false;
    bool isAlive = true;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAlive)
        {
            if (!isAttacking)
            {
                MoveTowardsPlayer();
                isMoving = true;
            }
            else
            {
                AttackPlayer();
                isMoving = false;
                isIdle = false; // Assuming that attacking should not be considered idle
                animator.SetTrigger("Attack1");
            }

            // Consider using else if here
            if (!isAttacking && !isMoving)
            {
                isIdle = true;
            }
            else
            {
                isIdle = false;
            }

            animator.SetBool("Run Forward", isMoving);
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction from the enemy to the player
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        // Rotate the enemy to face the player
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), 0.1f);

        // Move the enemy toward the player, but stop when within the stopping distance
        if (directionToPlayer.magnitude > stoppingDistance)
        {
            // Use NavMeshAgent to navigate
            navMeshAgent.SetDestination(player.position);
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

        // If the attack animation duration is complete, damage the player and reset
        if (attackTimer >= attackCooldown)
        {
            // Assuming playerController is a reference to the script managing player's health
            playerController.TakeDamage(attackDamage);

            // Reset attack-related variables
            isAttacking = false;
            attackTimer = 0f;
        }

        // If the player moved out of range during the attack, stop the attack and resume moving
        if (cancelAttack)
        {
            isAttacking = false;
            navMeshAgent.SetDestination(player.position);
        }
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
        isAlive = false;

        navMeshAgent.enabled = false;

        animator.SetBool("isDead", !isAlive);

        Destroy(gameObject); 

       //StartCoroutine(DestroyAfterTime(10f));
    }

    IEnumerator DestroyAfterTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Destroy(gameObject);
    }
}