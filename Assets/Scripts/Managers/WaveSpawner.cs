using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;

    public delegate void OnWaveTimerChanged(float value);

    public OnWaveTimerChanged onWaveTimerChanged;

    [SerializeField] Wave wave;
    [SerializeField] float waveTimer;

    [SerializeField] List<WaveDescriptor> waveDescriptors;

    Transform spawnPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

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
        if (waveTimer >= 0)
        {
            waveTimer -= Time.deltaTime;
            onWaveTimerChanged?.Invoke(waveTimer);
            return;
        }

        for (int i = 0; i < waveDescriptors.Count; i++)
        {
            WaveDescriptor waveDescriptor = waveDescriptors[i];

            if (waveDescriptor.spawnCount <= 0)
            {
                waveDescriptors.Remove(waveDescriptor);
                i--;
                continue;
            }

            if (waveDescriptor.spawnRate <= 0)
            {
                GameObject enemyClone = Instantiate(waveDescriptor.enemyPrefab, spawnPoint.position, spawnPoint.rotation, gameObject.transform);
                Enemy enemy = enemyClone.GetComponent<Enemy>();

                enemy.Wave = wave;

                MovementManager.Instance.AddEnemy(enemy);

                waveDescriptor.spawnCount--;
                waveDescriptor.ResetSpawnTimer();
            }

            waveDescriptor.spawnRate -= Time.deltaTime;
        }
    }

    private void CheckWave() 
    {
        if (waveDescriptors.Count == 0 || wave.AreAllEnemiesDead())
        {
            wave = RoundManager.Instance.GetNextWave(ref waveDescriptors);

            if (wave == null)
            {
                return;
            }
            waveTimer = wave.TimeForWave();
            onWaveTimerChanged?.Invoke(waveTimer);
        }
    }
}
