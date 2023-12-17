using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 20f;
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
            PlayerController player = hit.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                // Deal damage to the enemy
                player.TakeDamage(damage);

                // Destroy the projectile
                Destroy(gameObject);
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
