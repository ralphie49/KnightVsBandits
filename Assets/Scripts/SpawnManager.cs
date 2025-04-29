using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public int enemiesCount;
    public float startDelay = 2.0f;
    public Transform[] spawnPoints; // Assign 2 empty GameObjects at x = -22 and 13
    public float timeBetweenWaves = 30f;
    private int waveNumber = 3;
    public int currentEnemyCount = 0;
    // List to keep track of active enemies
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            for (int i = 0; i < waveNumber; i++)
            {
                if (GetCurrentEnemyCount() < 3)
                {
                    SpawnEnemy();
                }
                yield return new WaitForSeconds(6f); // Small delay between spawns                
            }
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnLocation = spawnPoints[spawnIndex];
        int enemyIndex = Random.Range(0, enemies.Length);
        GameObject enemy = Instantiate(enemies[enemyIndex], spawnLocation.position, Quaternion.identity);

        activeEnemies.Add(enemy);
    }
    // Method to get the current number of enemies in the scene
    public int GetCurrentEnemyCount()
    {
        // Remove null references from the list (in case enemies are destroyed)
        activeEnemies.RemoveAll(enemy => enemy == null);
        return activeEnemies.Count;
    }
}

