using System;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveManagement/EnemyWave", fileName = "EnemyWave")]
[Serializable]
public class  EnemyWave : ScriptableObject
{
    [SerializeField] int Quantity;
    [SerializeField] float spawnRate;
    [SerializeField] GameObject enemyPrefab;

    public WaveDescriptor GetWaveDescriptor()
    {
        return new WaveDescriptor(Quantity, enemyPrefab , this);
    }

    public float GetSpawnRate() => spawnRate;
}
