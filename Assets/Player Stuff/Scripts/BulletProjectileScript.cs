using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectileScript : MonoBehaviour
{
    private Rigidbody bulletRigidbody;

    public float bulletSpeed = 10f;

    public float maxTravelDistance = 40f;
    private float distanceTraveled = 0f;
    public int damage = 20;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody.velocity = transform.forward * bulletSpeed;
    }

    private void Update()
    {

        distanceTraveled += bulletSpeed * Time.deltaTime;

        if (distanceTraveled >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has an EnemyMovement script
        EnemyMovement enemy = other.GetComponent<EnemyMovement>();
        if (enemy != null)
        {
            // Deal damage to the enemy
            enemy.TakeDamage(damage);
        }

        // Check if the collided object has a FlyingEnemy script
        FlyingEnemy flyingEnemy = other.GetComponent<FlyingEnemy>();
        if (flyingEnemy != null)
        {
            // Deal damage to the enemy
            flyingEnemy.TakeDamage(damage);
        }

        // Destroy the projectile
        Destroy(gameObject);
    }
}
