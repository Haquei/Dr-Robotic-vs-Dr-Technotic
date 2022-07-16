using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] Transform spawnPoint;
    [SerializeField] Enemy toSpawn;
    [SerializeField] float timeBetweenSpawns;

    float lastSpawnTime;

    private void Start()
    {
        lastSpawnTime = -timeBetweenSpawns;  
    }

    void Update()
    {
        if (Time.time - lastSpawnTime > timeBetweenSpawns)
        {
            Spawn();
            lastSpawnTime = Time.time;
        }
    }

    void Spawn()
    {
        Instantiate(toSpawn, spawnPoint.position, Quaternion.identity);
    }
}
