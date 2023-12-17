using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 10f;
    public float maxRange = 10f;
    private float distanceTraveled = 0f;
    public int damage = 20;

    void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Update the distance traveled
        distanceTraveled += speed * Time.deltaTime;

        // Check for collisions with the enemy
        CheckCollisions();

        // Destroy the projectile when it reaches max range
        if (distanceTraveled >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    void CheckCollisions()
    {
        // Use a raycast to check for collisions with the enemy
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime))
        {
            // Check if the collided object has an EnemyMovement script
            EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                // Deal damage to the enemy
                enemy.TakeDamage(damage);

                // Destroy the projectile
                Destroy(gameObject);
            }

            FlyingEnemy flyingEnemy = hit.collider.GetComponent<FlyingEnemy>();
            if (flyingEnemy != null)
            {
                // Deal damage to the enemy
                flyingEnemy.TakeDamage(damage);

                // Destroy the projectile
                Destroy(gameObject);
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}