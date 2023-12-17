using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject flyingEnemyPrefab;

    public float spawnRadius = 30f;
    public float initialDelay = 10f;
    public float spawnInterval = 2f;
    public float minSpawnInterval = 1.5f;
    public float intervalReductionTime = 5f;

    private float elapsedTime;

    void Start()
    {
        elapsedTime = 0f;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(initialDelay);

        elapsedTime = 0f;

        Terrain terrain = Terrain.activeTerrain;

        while (true)
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector3 randomSpawnPosition = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward * spawnRadius;

            // Get the terrain height at the random spawn position and decrease it by 0.5
            randomSpawnPosition.y = terrain.SampleHeight(transform.position + randomSpawnPosition) + 2;

            // Spawn normal enemies
            Instantiate(enemyPrefab, transform.position + randomSpawnPosition, Quaternion.identity);

            // Spawn flying enemies after 30 seconds
            if (elapsedTime >= 30f)
            {
                float flyingRandomAngle = Random.Range(0f, 360f);
                Vector3 flyingRandomSpawnPosition = Quaternion.Euler(0f, flyingRandomAngle, 0f) * Vector3.forward * spawnRadius;

                // Get the terrain height at the flying spawn position
                flyingRandomSpawnPosition.y = terrain.SampleHeight(transform.position + flyingRandomSpawnPosition) + 10f;

                // Spawn flying enemies
                Instantiate(flyingEnemyPrefab, transform.position + flyingRandomSpawnPosition, Quaternion.identity);
            }

            if (elapsedTime > intervalReductionTime)
            {
                spawnInterval -= 0.25f;
                spawnInterval = Mathf.Max(spawnInterval, minSpawnInterval);
            }

            yield return new WaitForSeconds(spawnInterval);
            elapsedTime += spawnInterval;
        }
    }
}