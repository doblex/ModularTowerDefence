using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    Wave wave;

    List<WaveDescriptor> waveDescriptors;

    Transform spawnPoint;

    private void Awake()
    {
        waveDescriptors = new List<WaveDescriptor>();

        spawnPoint = transform;
    }

    private void Update()
    {
        SpawnEnemies();
        CheckWave();
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < waveDescriptors.Count; i++)
        {
            WaveDescriptor wave = waveDescriptors[i];

            if (wave.spawnCount <= 0)
            {
                waveDescriptors.Remove(wave);
                i--;
                continue;
            }

            if (wave.spawnRate >= 0)
            {
                GameObject enemyClone = Instantiate(wave.enemyPrefab, spawnPoint.position, spawnPoint.rotation, gameObject.transform);
                MovementManager.Instance.AddEnemy(enemyClone.GetComponent<Enemy>());
                wave.spawnCount--;
                wave.ResetSpawnTimer();
            }

            wave.spawnRate -= Time.deltaTime;
        }
    }

    private void CheckWave() 
    {
        if (waveDescriptors.Count == 0)
        {
            RoundManager.Instance.GetNextWave(ref waveDescriptors);
        }
    }
}
