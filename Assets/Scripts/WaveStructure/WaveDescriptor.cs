
using System;
using UnityEngine;

[Serializable]
public class WaveDescriptor 
{
    public float spawnRate;
    public int spawnCount;
    public EnemyWave enemyWave;
    public GameObject enemyPrefab;

    public WaveDescriptor(int spawnCount,GameObject enemyPrefab ,EnemyWave enemyWave)
    {
        this.spawnRate = 0;
        this.spawnCount = spawnCount;
        this.enemyPrefab = enemyPrefab;
        this.enemyWave = enemyWave;
    }

    public void ResetSpawnTimer() 
    { 
        spawnRate = enemyWave.GetSpawnRate();
    }
}
