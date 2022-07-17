using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] BoxCollider[] spawnZones;
    [SerializeField] WaveData[] waves;
    [SerializeField] float timeBetweenWaves;
    [SerializeField] float timeBeforeFirstWave;

    [SerializeField] Enemy smallEnemy;
    [SerializeField] Enemy bigEnemy;
    [SerializeField] Enemy flyingEnemy;
    [SerializeField] Spawner spawer;

    int currentWave;
    float lastSpawnTime = 0f;

    void Update()
    {
        if (currentWave >= waves.Length) return; // You won the game

        if (Time.time - lastSpawnTime > timeBetweenWaves)
        {
            SpawnWave(waves[currentWave++]);
            lastSpawnTime = Time.time;
        }            
    }

    void SpawnWave(WaveData waveData)
    {
        // Just all at the same time for now.
        SpawnEnemyType(smallEnemy, waveData.smallEnemies);
        SpawnEnemyType(bigEnemy, waveData.bigEnemies);
        SpawnEnemyType(flyingEnemy, waveData.fliers);

        // And spawers
        for (int i = 0; i < waveData.spawners; i++)
        {
            int randomZone = Random.Range(0, spawnZones.Length);
            Vector3 randomPoint = RandomPointInSpawnzone(spawnZones[randomZone]);
            Instantiate(spawer, randomPoint, Quaternion.identity); // TODO Make this face target
        }
    }

    void SpawnEnemyType(Enemy enemyType, int num)
    {
        for (int i = 0; i < num; i++)
        {
            int randomZone = Random.Range(0, spawnZones.Length);
            Vector3 randomPoint = RandomPointInSpawnzone(spawnZones[randomZone]);
            Instantiate(enemyType, randomPoint, Quaternion.identity);
        }
    }

    public static Vector3 RandomPointInSpawnzone(BoxCollider collider)
    {
        Bounds bounds = collider.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0, // Just easier this way
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

}

[System.Serializable]
struct WaveData
{
    public int spawners;
    public int smallEnemies;
    public int bigEnemies;
    public int fliers;

    public WaveData(int spawners, int smallEnemies, int bigEnemies, int fliers)
    {
        this.spawners = spawners;
        this.smallEnemies = smallEnemies;
        this.bigEnemies = bigEnemies;
        this.fliers = fliers;
    }
}