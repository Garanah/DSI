using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemyPrefab;
    public EnemySO enemyToSpawn;

    [Header("Spawn Settings")]
    public int numberToSpawn = 3;
    public bool spawnOnce = true;
    public float spawnInterval = 5f;

    [Header("Spawn Area")]
    public float spawnRadius = 8f;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            if (spawnOnce)
                StartCoroutine(SpawnWave());
            else
                StartCoroutine(SpawnLoop());
        }
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            SpawnOne();
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                SpawnOne();
                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnOne()
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPos.y = transform.position.y;

        GameObject e = Instantiate(enemyPrefab, randomPos, Quaternion.identity);

        EnemyBehaviours enemy = e.GetComponent<EnemyBehaviours>();
        if (enemy != null)
        {
            enemy.enemyData = enemyToSpawn;
        }
    }
}