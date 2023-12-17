using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoContainerSpawner : MonoBehaviour
{
    public GameObject ammoContainerPrefab;
    public GameObject healthContainerPrefab;

    public float spawnRadius = 30f;
    public float spawnInterval = 2f;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        Terrain terrain = Terrain.activeTerrain;

        while (true)
        {
            // Get random positions within a unit circle and scale by spawn radius
            Vector2 randomOffsetAmmo = Random.insideUnitCircle * spawnRadius;
            Vector2 randomOffsetHealth = Random.insideUnitCircle * spawnRadius;

            // Convert 2D positions to 3D by adding the y component from the terrain
            Vector3 randomSpawnPositionAmmo = new Vector3(randomOffsetAmmo.x, terrain.SampleHeight(transform.position), randomOffsetAmmo.y);
            Vector3 randomSpawnPositionHealth = new Vector3(randomOffsetHealth.x, terrain.SampleHeight(transform.position), randomOffsetHealth.y);

            // Spawn ammo container
            Instantiate(ammoContainerPrefab, transform.position + randomSpawnPositionAmmo, Quaternion.identity);

            // Spawn health container
            Instantiate(healthContainerPrefab, transform.position + randomSpawnPositionHealth, Quaternion.identity);

            // Wait to spawn containers
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
