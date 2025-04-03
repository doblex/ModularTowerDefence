using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;


    [SerializeField] List<Round> waves;

    int currentRoundIndex = 0;
    int currentWaveIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public bool GetNextWave(ref List<WaveDescriptor> waveDescriptors)
    {
        if (waveDescriptors.Count > 0)
        { 
            Debug.LogError("RoundManager.GetNextWave : the list must be empty");
            return false;
        }

        List<EnemyWave> enemyWaves = waves[currentRoundIndex].Waves[currentWaveIndex].EnemyWaves;

        foreach (var enemyWave in enemyWaves)
        {
            waveDescriptors.Add(enemyWave.GetWaveDescriptor());
        }

        if (currentWaveIndex >= waves[currentRoundIndex].Waves.Count - 1)
        {
            currentRoundIndex++;

            if (currentRoundIndex >= waves.Count - 1)
            {
                //GameManager.Win
            }
        }
        else
        { 
            currentWaveIndex++;
        }

        return false;
    }
}
