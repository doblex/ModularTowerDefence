using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveManagement/Wave", fileName = "Wave")]
[Serializable]
public class Wave : ScriptableObject
{
    [SerializeField] public List<EnemyWave> EnemyWaves;

    [SerializeField] float timeforWave;

    int enemiesToSpawn = 0;
    bool allEnemiesDead = false;

    public float TimeForWave() =>  timeforWave;
    public bool AreAllEnemiesDead() => allEnemiesDead;


    private void SetUpEnemiesToSpawn()
    {
        enemiesToSpawn = 0;
        allEnemiesDead = false;

        foreach (EnemyWave enemyWave in EnemyWaves)
        {
            enemiesToSpawn += enemyWave.GetWaveDescriptor().spawnCount;
        }
    }

    public void EnemyDeath() 
    {
        enemiesToSpawn--;

        if (enemiesToSpawn <= 0)
            allEnemiesDead = true;
    }


    public void Init() 
    {
        SetUpEnemiesToSpawn();
     }
    

}
