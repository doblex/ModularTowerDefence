
using System;
using UnityEngine;

[Serializable]
public class WaveDescriptor 
{
    public float spawnRate;
    public int spawnCount;
    public EnemyWave enemyWave;
    public GameObject enemyPrefab;

    public WaveDescriptor(float spawnRate, int spawnCount,GameObject enemyPrefab ,EnemyWave enemyWave)
    {
        this.spawnRate = spawnRate;
        this.spawnCount = spawnCount;
        this.enemyPrefab = enemyPrefab;
        this.enemyWave = enemyWave;
    }

    public void ResetSpawnTimer() 
    { 
        spawnRate = enemyWave.GetSpawnRate();
    }
}
